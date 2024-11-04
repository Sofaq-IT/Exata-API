using System.ComponentModel.DataAnnotations;

namespace Exata.Domain.DTO
{
    public class UploadDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataReferencia { get; set; }
    }
}
