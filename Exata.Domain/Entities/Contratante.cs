using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exata.Domain.Entities;

[Table("Contratante")]
public class Contratante
{
    [Key]
    public int Id { get; set; }

    [MaxLength(250)]
    public string Rua { get; set; }

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

    public string Logo { get; set; }
}
