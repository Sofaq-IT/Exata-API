using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exata.Domain.Entities;

[Table("Campo")]
public class Campo
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(20, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [MinLength(3, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Display(Name = "Tabela")]
    public string TabelaID { get; set; }

    [StringLength(20, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [MinLength(3, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [Display(Name = "Campo")]
    public string CampoID { get; set; }

    [StringLength(30, ErrorMessage = "O campo {0} deve conter, no máximo {1} caracteres")]
    [MinLength(3, ErrorMessage = "O campo {0} deve conter, no mínimo {1} caracteres!")]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public bool Pesquisa { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public bool Ordena { get; set; }
}
