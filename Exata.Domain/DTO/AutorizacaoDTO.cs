using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Domain.DTO;

public class AutorizacaoDTO
{
    [Required(ErrorMessage = "Token não informado")]
    public string Token { get; set; }

    [Required(ErrorMessage = "Token de Atualização não informado")]
    public string TokenAtualizacao { get; set; }

    public DateTime ExpiraEm { get; set; }

    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Tema { get; set; }

    public int PerfilID { get; set; }

    public string Perfil { get; set; }

    public DateTime ValidadeLicenca { get; set; }

    public List<string> Menus { get; set; }

    public string Avatar { get; set; }
}
