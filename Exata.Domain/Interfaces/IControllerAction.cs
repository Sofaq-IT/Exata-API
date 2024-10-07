using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface IControllerAction
{
    ControllerAction Inserir(ControllerAction controllerAction);

    bool Existe(ControllerAction controllerAction);

    bool Existe(int id);

    Task<List<ControllerAction>> Listar();

    ControllerAction Abrir(ControllerAction controllerAction);

    bool PermissaoValida(ControllerAction controllerAction, string usuario);
}
