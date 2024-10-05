using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("PerfilSecao")]
public class PerfilSecao()
{
    public int PerfilID { get; set; }

    public int SecaoID { get; set; }

    [JsonIgnore]
    public virtual Perfil Perfil { get; set; }

    public virtual Secao Secao { get; set; }
}
