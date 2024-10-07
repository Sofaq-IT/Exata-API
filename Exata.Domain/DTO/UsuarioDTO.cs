using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Domain.DTO;

public class UsuarioDTO
{
    public string Id { get; set; }

    [StringLength(50, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(6, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    public string UserName { get; set; }

    [StringLength(150, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [EmailAddress(ErrorMessage = "O campo e-mail está em formato incorreto")]
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public bool Ativo { get; set; }

    [StringLength(200, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [MinLength(8, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    public string Nome { get; set; }

    public string Tema { get; set; } = "light";

    public int? PerfilID { get; set; }

    public string Senha { get; set; }
}
