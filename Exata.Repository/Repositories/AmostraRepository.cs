using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class AmostraRepository : IAmostra
{
    private readonly ApiContext _ctx;
    private readonly IUsuario _usuario;

    public AmostraRepository(ApiContext context, IUsuario usuario)
    {
        _ctx = context;
        _usuario = usuario;
    }

    public async Task<Amostra> Inserir(Amostra amostra)
    {
        amostra.UserCadastro = await _usuario.UserID();
        amostra.booNovo = true;
        _ctx.Add(amostra);
        return amostra;
    }
}