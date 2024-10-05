using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("ContatoTipo")]
public class ContatoTipo
{
    [StringLength(20, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    public string TelefoneID { get; set; }

    public int TipoContatoID { get; set; }

    [JsonIgnore]
    public virtual Contato Contato { get; set; }

    public virtual TipoContato TipoContato { get; set; }
}
