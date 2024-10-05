using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class SecaoRepository : ISecao
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;

    public SecaoRepository(ApiContext context, ICampo campo)
    {
        _ctx = context;
        _campo = campo;
    }

    public async Task Inserir(Secao secao)
    {
        secao.booNovo = true;
        _ctx.Add(secao);
        await _ctx.SaveChangesAsync();
    }

    public async Task Atualizar(Secao secao)
    {
        secao.booNovo = false;
        var update = _ctx.Entry(secao);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        Secao secao = await _ctx.Secao.Where(x => x.SecaoID == id).FirstOrDefaultAsync();
        _ctx.Secao.Remove(secao);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Secao> Abrir(int id)
    {
        return await _ctx.Secao
            .AsNoTracking()
            .Where(x => x.SecaoID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(int id)
    {
        return _ctx.Secao.Any(x => x.SecaoID == id);
    }

    public async Task<PagedList<Secao>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Secao";
        PagedList<Secao> secoes = null;

        IQueryable<Secao> iSecoes = _ctx.Secao
            .AsNoTracking()
            .Include(x => x.Telefone);

        if (paginacao.Ativos != null)
            iSecoes = iSecoes.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception("Campo não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "Descricao":
                    iSecoes = iSecoes.Where(x => x.Descricao.Contains(paginacao.PesquisarValor));
                    break;

                case "Telefone":
                    iSecoes = iSecoes.Where(x => x.Telefone.Descricao.Contains(paginacao.PesquisarValor));
                    break;
            }
        }

        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception("Campo não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iSecoes = iSecoes.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iSecoes = iSecoes.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        secoes = await PagedList<Secao>
           .ToPagedList(
                iSecoes,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return secoes;
    }

    public async Task<List<Secao>> Listar()
    {
        return await _ctx.Secao
            .AsNoTracking()
            .Where(x => x.Ativo == true)
            .Select(x => new Secao
            {
                SecaoID = x.SecaoID,
                Descricao = x.Descricao,
                Ativo = x.Ativo
            })
            .OrderBy(x => x.Descricao)
            .ToListAsync();
    }
}
