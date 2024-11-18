using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exata.Domain.Entities
{
    [Table("Amostra")]
    public class Amostra : Base
    {
        public Amostra()
        {
            AmostraId = Guid.NewGuid();
        }

        [Key]
        public Guid AmostraId { get; set; }

        [Required(ErrorMessage = "Campo Descrição é Obrigatório")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Toda amostra deve estar vinculada a um Cliente")]
        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set;}
    }
}
