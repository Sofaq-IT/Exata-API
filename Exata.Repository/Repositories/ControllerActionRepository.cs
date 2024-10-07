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

public class ControllerActionRepository : IControllerAction
{
    private readonly ApiContext _ctx;

    public ControllerActionRepository(ApiContext context)
    {
        _ctx = context;
    }

    public ControllerAction Inserir(ControllerAction controllerAction)
    {
        _ctx.Add(controllerAction);
        return controllerAction;
    }

    public bool Existe(ControllerAction controllerAction)
    {
        return _ctx.ControllerAction.Any(x => x.Metodo == controllerAction.Metodo &&
                                              x.Controller == controllerAction.Controller &&
                                              x.Action == controllerAction.Action);
    }

    public bool Existe(int id)
    {
        return _ctx.ControllerAction.Any(x => x.ControllerActionID == id);
    }

    public async Task<List<ControllerAction>> Listar()
    {
        return await _ctx.ControllerAction
            .AsNoTracking()
            .ToListAsync();
    }

    public PerfilControllerAction Excluir(PerfilControllerAction perfilControllerAction)
    {
        _ctx.PerfilControllerAction.Remove(perfilControllerAction);
        return perfilControllerAction;
    }

    public ControllerAction Abrir(ControllerAction controllerAction)
    {
        return _ctx.ControllerAction.FirstOrDefault(x => x.Metodo == controllerAction.Metodo &&
                                                         x.Controller == controllerAction.Controller &&
                                                         x.Action == controllerAction.Action);
    }

    public bool PermissaoValida(ControllerAction controllerAction, string usuario)
    {
        ApplicationUser user = _ctx.Users.FirstOrDefault(x => x.UserName == usuario);

        return _ctx.PerfilControllerAction
            .Any(x => x.PerfilID == user.PerfilID &&
                      x.ControllerActionID == controllerAction.ControllerActionID);
    }
}
