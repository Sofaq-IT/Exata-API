using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Exata.Domain.Entities;

[Table("PerfilControllerAction")]
public class PerfilControllerAction
{
    public int PerfilID { get; set; }

    public int ControllerActionID { get; set; }

    [JsonIgnore]
    public virtual Perfil Perfil { get; set; }

    [JsonIgnore]
    public virtual ControllerAction ControllerAction { get; set; }
}
