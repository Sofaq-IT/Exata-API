using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Entities;

namespace Exata.Domain.DTO;

public class ContratanteDTO : Contratante
{
    public string CNPJ { get; set; }
    public string Empresa { get; set; }
}
