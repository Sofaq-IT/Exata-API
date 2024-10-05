using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("Usuario")]
public class Usuario() : Base()
{
    [Key]
    public int UsuarioID { get; set; }

    [StringLength(50, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(6, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    public string Login { get; set; }

    [StringLength(200, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(8, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    public string Nome { get; set; }

    [StringLength(150, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [EmailAddress(ErrorMessage = "O campo e-mail está em formato incorreto")]
    public string Email { get; set; }

    [StringLength(500)]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(8, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public bool Ativo { get; set; }
    
    public int? PerfilID { get; set; }

    public virtual Perfil Perfil { get; set; }
}
