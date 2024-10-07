using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Domain.DTO;

public class LicencaDTO
{
    public string Licenca { get; set; }
    public int QtdeUsuarios { get; set; }
    public DateTime DataValidade { get; set; }
    public string CNPJ { get; set; }
    public string Empresa { get; set; }
}
