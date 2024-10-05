using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("Telefone")]
public class Telefones() : Base()
{
    [Key]
    public int TelefoneID { get; set; }

    [StringLength(30, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [MinLength(11, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public string Telefone { get; set; }

    [StringLength(200, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [MinLength(1, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public bool Ativo { get; set; }

    [JsonIgnore]
    public List<Secao> Secao { get; set; }
}
