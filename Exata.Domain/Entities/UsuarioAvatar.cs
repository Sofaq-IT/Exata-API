using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Exata.Domain.Entities;

[Table("UsuarioAvatar")]
public class UsuarioAvatar
{
    [JsonIgnore]
    [Key]
    [StringLength(450)]
    public string UsuarioID { get; set; }

    public string Avatar { get; set; }

    [NotMapped]
    public string Login { get; set; }

    [JsonIgnore]
    public ApplicationUser Usuario { get; set; }
}
