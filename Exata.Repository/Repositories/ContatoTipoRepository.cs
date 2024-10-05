using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class ContatoTipoRepository : IContatoTipo
{
    private readonly ApiContext _ctx;

    public ContatoTipoRepository(ApiContext context)
    {
        _ctx = context;
    }

    public async Task Inserir(ContatoTipo contatoTipo)
    {
        _ctx.Add(contatoTipo);
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(ContatoTipo contatoTipo)
    {
        _ctx.ContatoTipo.Remove(contatoTipo);
        await _ctx.SaveChangesAsync();
    }

    public bool Existe(ContatoTipo contatoTipo)
    {
        return _ctx.ContatoTipo.Any(x => x.TelefoneID == contatoTipo.TelefoneID &&
                                         x.TipoContatoID == contatoTipo.TipoContatoID);
    }
}
