using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class TelefonesRepository : ITelefones
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;

    public TelefonesRepository(ApiContext context, ICampo campo)
    {
        _ctx = context;
        _campo = campo;
    }

    public async Task Inserir(Telefones telefone)
    {
        telefone.booNovo = true;
        _ctx.Add(telefone);
        await _ctx.SaveChangesAsync();
    }

    public async Task Atualizar(Telefones telefone)
    {
        telefone.booNovo = false;
        var update = _ctx.Entry(telefone);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        Telefones telefone = await _ctx.Telefone.Where(x => x.TelefoneID == id).FirstOrDefaultAsync();
        _ctx.Telefone.Remove(telefone);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Telefones> Abrir(int id)
    {
        return await _ctx.Telefone
            .AsNoTracking()
            .Where(x => x.TelefoneID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(string numero)
    {
        return _ctx.Telefone.Any(x => x.Telefone == numero);
    }

    public bool Existe(int id)
    {
        return _ctx.Telefone.Any(x => x.TelefoneID == id);
    }

    public async Task<PagedList<Telefones>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Telefone";
        PagedList<Telefones> telefones = null;

        IQueryable<Telefones> iTelefones = _ctx.Telefone.AsNoTracking();

        if (paginacao.Ativos != null)
            iTelefones = iTelefones.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception("Campo não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "Telefone":
                    iTelefones = iTelefones.Where(x => x.Telefone.Contains(paginacao.PesquisarValor));
                    break;

                case "Descricao":
                    iTelefones = iTelefones.Where(x => x.Descricao.Contains(paginacao.PesquisarValor));
                    break;
            }
        }

        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception("Campo não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iTelefones = iTelefones.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iTelefones = iTelefones.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        telefones = await PagedList<Telefones>
           .ToPagedList(
                iTelefones,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return telefones;
    }
}
