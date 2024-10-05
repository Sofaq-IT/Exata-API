using Exata.Domain.Interfaces;
using Exata.Domain.Entities;
using Exata.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Helpers;

namespace Exata.Repository.Repositories;

public class ContatoRepository : IContato
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;

    public ContatoRepository(ApiContext context, ICampo campo)
    {
        _ctx = context;
        _campo = campo;
    }

    public async Task Inserir(Contato contato)
    {
        contato.booNovo = true;
        _ctx.Add(contato);
        await _ctx.SaveChangesAsync();
    }

    public async Task Atualizar(Contato contato)
    {
        //_ctx.Contato.Update(contato);
        contato.booNovo = false;
        var update = _ctx.Entry(contato);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(string id)
    {
        Contato contato = await _ctx.Contato.Where(x => x.TelefoneID == id).FirstOrDefaultAsync();
        _ctx.Contato.Remove(contato);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Contato> Abrir(string id)
    {
        return await _ctx.Contato
            .AsNoTracking()
            .Include(x => x.ContatoTipo)
            .ThenInclude(y => y.TipoContato)
            .Where(x => x.TelefoneID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(string id)
    {
        return _ctx.Contato.Any(x => x.TelefoneID == id);
    }

    public async Task<PagedList<Contato>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Contato";
        PagedList<Contato> contatos = null;

        IQueryable<Contato> iContatos = _ctx.Contato.AsNoTracking();

        if (paginacao.Ativos != null)
            iContatos = iContatos.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception("Campo não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "TelefoneID":
                    iContatos = iContatos.Where(x => x.TelefoneID.Contains(paginacao.PesquisarValor));
                    break;

                case "Nome":
                    iContatos = iContatos.Where(x => x.Nome.Contains(paginacao.PesquisarValor));
                    break;
            }
        }

        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception("Campo não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iContatos = iContatos.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iContatos = iContatos.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        contatos = await PagedList<Contato>
           .ToPagedList(
                iContatos,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return contatos;
    }
}
