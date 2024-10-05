using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAutom.Domain.DTO;

public class CamposDTO
{
    public string id { get; set; }
    public string descricao { get; set; }
    public bool pesquisa { get; set; }
    public bool ordena { get; set; }
}
