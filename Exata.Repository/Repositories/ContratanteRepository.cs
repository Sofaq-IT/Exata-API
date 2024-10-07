using Microsoft.EntityFrameworkCore;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class ContratanteRepository : IContratante
{
    private readonly ApiContext _ctx;

    public ContratanteRepository(ApiContext context)
    {
        _ctx = context;
    }

    public async Task<Contratante> Inserir(Contratante contratante)
    {
        await _ctx.AddAsync(contratante);
        return contratante;
    }

    public Contratante Alterar(Contratante contratante)
    {
        _ctx.Update(contratante);
        return contratante;
    }

    public bool Existe()
    {
        return _ctx.Contratante.Any();
    }

    public async Task<Contratante> Abrir()
    {
        return await _ctx.Contratante
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<Contratante> InserirLogo(Contratante contratante)
    {
        await _ctx.AddAsync(contratante);
        return contratante;
    }

    public Contratante AlterarLogo(Contratante contratante)
    {
        var update = _ctx.Entry(contratante);
        update.Property("Logo").IsModified = true;
        return contratante;
    }

    public async Task<string> Logo()
    {
        return await _ctx.Contratante
            .Select(x => x.Logo)
            .FirstOrDefaultAsync();
    }
}
