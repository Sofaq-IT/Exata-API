using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Exata.Domain.Enums;

namespace Exata.Domain.Entities
{
    [Table("Upload")]
    public class Upload : Base
    {
        [Key]
        public int UploadID { get; set; }

        [StringLength(150, ErrorMessage = "O Campo {0} deve possuir no máximo {1} caracteres")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string NomeArquivoEntrada { get; set; }

        [StringLength(50, ErrorMessage = "O Campo {0} deve possuir no máximo {1} caracteres")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string NomeArquivoArmazenado { get; set; }

        [StringLength(350, ErrorMessage = "O Campo {0} deve possuir no máximo {1} caracteres")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string UrlStorage { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public long Tamanho { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int QtdeRegistros { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public StatusUploadEnum StatusAtual { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
