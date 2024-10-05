using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class CampoRepository : ICampo
{
    private readonly ApiContext _ctx;

    public CampoRepository(ApiContext context)
    {
        _ctx = context;
    }

    public async Task<List<Campo>> Campos(string tabela)
    {
        return await _ctx.Campo
            .AsNoTracking()
            .Where(x => x.TabelaID == tabela)
            .Select(x => new Campo()
            {
                CampoID = x.CampoID,
                Descricao = x.Descricao,
                Pesquisa = x.Pesquisa,
                Ordena = x.Ordena
            })
            .ToListAsync();
    }

    public bool ExisteOrdenacao(string tabela, string campo)
    {
        return _ctx.Campo.Any(x => x.TabelaID == tabela &&
                                   x.CampoID == campo &&
                                   x.Ordena == true);
    }

    public bool ExistePesquisa(string tabela, string campo)
    {
        return _ctx.Campo.Any(x => x.TabelaID == tabela &&
                                   x.CampoID == campo &&
                                   x.Pesquisa == true);
    }
}
