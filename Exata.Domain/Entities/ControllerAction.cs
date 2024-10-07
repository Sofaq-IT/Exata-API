using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Exata.Domain.Entities;

[Table("ControllerAction")]
public class ControllerAction
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ControllerActionID { get; set; }

    [StringLength(10)]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public string Metodo { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public string Controller { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Campo {0} obrigatório")]
    public string Action { get; set; }

    [JsonIgnore]
    [StringLength(200)]
    public string Descricao { get; set; }

    public List<PerfilControllerAction> PerfilControllerAction { get; set; }

    [NotMapped]
    public string DescricaoJson {
        get
        {
            if (string.IsNullOrEmpty(Descricao))
                return $"{Controller}.{Action} ({Metodo})";
            else
                return Descricao;
        }
    }
}
