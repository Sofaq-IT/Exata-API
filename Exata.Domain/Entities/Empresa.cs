using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Exata.Domain.Entities
{

    [Table("Empresa")]
    public class Empresa() : Base()
    {
        [Key]
        public int EmpresaID { get; set; }

        [MaxLength(1), Required]
        public string FisicaJuridica { get; set; }

        private string _cpfCnpj;

        [MaxLength(14), Required]
        public string CpfCnpj
        {
            get { return _cpfCnpj; }
            set
            {
                _cpfCnpj = Regex.Replace(value, @"[^0-9]", "");
            }
        }

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
