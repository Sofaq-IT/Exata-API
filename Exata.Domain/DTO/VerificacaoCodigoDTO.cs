using System.ComponentModel.DataAnnotations;

namespace Exata.Domain.DTO
{
    public class VerificacaoCodigoDTO
    {
        [Required(ErrorMessage = "Email não informado")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Código não informado")]
        public int Codigo { get; set; }
    }
}
