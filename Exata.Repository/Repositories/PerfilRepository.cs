using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class PerfilRepository : IPerfil
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;

    public PerfilRepository(ApiContext context, ICampo campo)
    {
        _ctx = context;
        _campo = campo;
    }

    public async Task Inserir(Perfil perfil)
    {
        perfil.booNovo = true;
        _ctx.Add(perfil);
        await _ctx.SaveChangesAsync();
    }

    public async Task Atualizar(Perfil perfil)
    {
        perfil.booNovo = false;
        var update = _ctx.Entry(perfil);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        Perfil perfil = await _ctx.Perfil.Where(x => x.PerfilID == id).FirstOrDefaultAsync();
        _ctx.Perfil.Remove(perfil);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Perfil> Abrir(int id)
    {
        return await _ctx.Perfil
            .AsNoTracking()
            .Include(x => x.PerfilSecao)
            .ThenInclude(y => y.Secao)
            .Where(x => x.PerfilID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(int id)
    {
        return _ctx.Perfil.Any(x => x.PerfilID == id);
    }

    public async Task<PagedList<Perfil>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Perfil";
        PagedList<Perfil> perfis = null;

        IQueryable<Perfil> iPerfis = _ctx.Perfil.AsNoTracking();

        if (paginacao.Ativos != null)
            iPerfis = iPerfis.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception("Campo não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "Descricao":
                    iPerfis = iPerfis.Where(x => x.Descricao.Contains(paginacao.PesquisarValor));
                    break;
            }
        }

        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception("Campo não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iPerfis = iPerfis.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iPerfis = iPerfis.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        perfis = await PagedList<Perfil>
           .ToPagedList(
                iPerfis,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return perfis;
    }
}