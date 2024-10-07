using System.ComponentModel.DataAnnotations;

namespace Exata.Domain.DTO;

public class UsuarioTemaDTO
{
    [StringLength(50, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(6, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    public string UserName { get; set; }
        
    public string Tema { get; set; } = "light";
}
