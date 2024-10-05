using Microsoft.EntityFrameworkCore;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class PerfilSecaoRepository : IPerfilSecao
{
    private readonly ApiContext _ctx;

    public PerfilSecaoRepository(ApiContext context)
    {
        _ctx = context;
    }

    public async Task Inserir(PerfilSecao perfilSecao)
    {
        _ctx.Add(perfilSecao);
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(PerfilSecao perfilSecao)
    {
        _ctx.PerfilSecao.Remove(perfilSecao);
        await _ctx.SaveChangesAsync();
    }

    public bool Existe(PerfilSecao perfilSecao)
    {
        return _ctx.PerfilSecao.Any(x => x.PerfilID == perfilSecao.PerfilID &&
                                         x.SecaoID == perfilSecao.SecaoID);
    }
}
