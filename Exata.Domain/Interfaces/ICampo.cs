using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface ICampo
{
    Task<List<Campo>> Campos(string tabela);
    bool ExistePesquisa(string tabela, string campo);
    bool ExisteOrdenacao(string tabela, string campo);
}
