using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Domain.DTO;

public class LoginDTO
{
    [Required(ErrorMessage = "Usuário não informado")]
    public string Usuario { get; set; }

    [Required(ErrorMessage = "Senha não informada")]
    public string Senha { get; set; }
}
