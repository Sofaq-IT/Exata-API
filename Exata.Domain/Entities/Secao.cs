using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("Secao")]
public class Secao() : Base()
{
    [Key]
    public int SecaoID { get; set; }
    
    [StringLength(100, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(5, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public bool Ativo { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public int TelefoneID { get; set; }

    public virtual Telefones Telefone { get; set; }

    [JsonIgnore]
    public List<PerfilSecao> PerfilSecao { get; set; }
}
