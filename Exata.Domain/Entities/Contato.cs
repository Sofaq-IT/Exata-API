using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("Contato")]
public class Contato() : Base()
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(20, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [MinLength(11, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Display(Name = "Telefone")]
    public string TelefoneID { get; set; }

    [StringLength(200, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [MinLength(8, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public bool Ativo { get; set; }

    public List<ContatoTipo> ContatoTipo { get; set; }
}
