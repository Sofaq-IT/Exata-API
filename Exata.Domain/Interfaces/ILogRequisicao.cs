using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface ILogRequisicao
{
    LogRequisicao Inserir(LogRequisicao logRequisicoes);
}
