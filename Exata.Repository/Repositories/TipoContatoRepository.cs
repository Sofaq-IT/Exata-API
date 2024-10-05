using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class TipoContatoRepository : ITipoContato
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;

    public TipoContatoRepository(ApiContext context, ICampo campo)
    {
        _ctx = context;
        _campo = campo;
    }

    public async Task Inserir(TipoContato tipoContato)
    {
        if (tipoContato == null)
            throw new Exception("Tipo de Contato não Informado!");

        tipoContato.booNovo = true;
        _ctx.Add(tipoContato);
        await _ctx.SaveChangesAsync();
    }

    public async Task Atualizar(TipoContato tipoContato)
    {
        tipoContato.booNovo = false;
        var update = _ctx.Entry(tipoContato);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        TipoContato tipoContato = await _ctx.TipoContato.Where(x => x.TipoContatoID == id).FirstOrDefaultAsync();
        _ctx.TipoContato.Remove(tipoContato);
        await _ctx.SaveChangesAsync();
    }

    public async Task<TipoContato> Abrir(int id)
    {
        return await _ctx.TipoContato
            .AsNoTracking()
            .Where(x => x.TipoContatoID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(int id)
    {
        return _ctx.TipoContato.Any(x => x.TipoContatoID == id);
    }

    public async Task<PagedList<TipoContato>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "TipoContato";
        PagedList<TipoContato> tiposContatos = null;

        IQueryable<TipoContato> iTiposContatos = _ctx.TipoContato.AsNoTracking();

        if (paginacao.Ativos != null)
            iTiposContatos = iTiposContatos.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception("Campo não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "Descricao":
                    iTiposContatos = iTiposContatos.Where(x => x.Descricao.Contains(paginacao.PesquisarValor));
                    break;
            }
        }

        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception("Campo não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iTiposContatos = iTiposContatos.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iTiposContatos = iTiposContatos.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        tiposContatos = await PagedList<TipoContato>
           .ToPagedList(
                iTiposContatos,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return tiposContatos;
    }

    public async Task<List<TipoContato>> Listar()
    {                
        return await _ctx.TipoContato
            .AsNoTracking()
            .Where(x => x.Ativo == true)
            .OrderBy(x => x.Descricao)
            .ToListAsync();
    }
}
