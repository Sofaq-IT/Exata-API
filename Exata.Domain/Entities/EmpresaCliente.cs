using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("EmpresaCliente")]
public class EmpresaCliente
{
    public int EmpresaID { get; set; }

    public int ClienteID { get; set; }

    [JsonIgnore]
    public virtual Empresa Empresa { get; set; }

    [JsonIgnore]
    public virtual Cliente Cliente { get; set; }
}
