using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exata.Domain.Entities
{

    [Table("Cliente")]
    public class Cliente() : Base()
    {
        [Key]
        public int ClienteID { get; set; }

        [MaxLength(1), Required]
        public string FisicaJuridica { get; set; }

        [MaxLength(14), Required]
        public string CpfCnpj { get; set; }

        [MaxLength(350), Required]
        public string NomeRazaoSocial { get; set; }

        [MaxLength(150)]
        public string ApelidoNomeFantasia { get; set; }

        [MaxLength(150), Required]
        public string Email { get; set; }

        [MaxLength(30)]
        public string Telefone { get; set; }

        [MaxLength(9), Required]
        public string Cep { get; set; }

        [MaxLength(250)]
        public string Logradouro { get; set; }

        [MaxLength(10)]
        public string Numero { get; set; }

        [MaxLength(250)]
        public string Complemento { get; set; }

        [MaxLength(250)]
        public string Bairro { get; set; }

        [MaxLength(250)]
        public string Cidade { get; set; }

        [MaxLength(2)]
        public string Estado { get; set; }

        [Required]
        public bool Ativo { get; set; }
    }
}
