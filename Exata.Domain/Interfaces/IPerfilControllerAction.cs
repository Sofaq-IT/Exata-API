using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface IPerfilControllerAction
{
    PerfilControllerAction Inserir(PerfilControllerAction perfilControllerAction);
    bool Existe(PerfilControllerAction perfilControllerAction);
    PerfilControllerAction Excluir(PerfilControllerAction perfilControllerAction);
}
