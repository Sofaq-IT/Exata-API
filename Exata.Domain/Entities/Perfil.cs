using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

[Table("Perfil")]
public class Perfil() : Base()
{
    [Key]
    public int PerfilID { get; set; }

    [StringLength(100, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(5, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public bool Ativo { get; set; }

    [JsonIgnore]
    public List<ApplicationUser> Users { get; set; }
        
    public List<PerfilControllerAction> PerfilControllerAction { get; set; }

    public virtual ApplicationUser UsuarioPerfilCriacao { get; set; }

    public virtual ApplicationUser UsuarioPerfilAlteracao { get; set; }
}
