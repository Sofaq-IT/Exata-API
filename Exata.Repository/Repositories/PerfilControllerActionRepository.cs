using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class PerfilControllerActionRepository : IPerfilControllerAction
{
    private readonly ApiContext _ctx;

    public PerfilControllerActionRepository(ApiContext context)
    {
        _ctx = context;
    }

    public PerfilControllerAction Inserir(PerfilControllerAction perfilControllerAction)
    {
        _ctx.Add(perfilControllerAction);
        return perfilControllerAction;
    }

    public bool Existe(PerfilControllerAction perfilControllerAction)
    {
        return _ctx.PerfilControllerAction.Any(x => x.ControllerActionID == perfilControllerAction.ControllerActionID &&
                                                    x.PerfilID == perfilControllerAction.PerfilID);
    }

    public PerfilControllerAction Excluir(PerfilControllerAction perfilControllerAction)
    {
        _ctx.Remove(perfilControllerAction);
        return perfilControllerAction;
    }
}
