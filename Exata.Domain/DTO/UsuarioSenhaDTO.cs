using System.ComponentModel.DataAnnotations;

namespace Exata.Domain.DTO;

public class UsuarioSenhaDTO
{
    [Required]
    public string Login { get; set; }

    public string SenhaAntiga { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(8, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    public string SenhaNova { get; set; }

    public string SenhaNovaConfirmacao { get; set; }
}
