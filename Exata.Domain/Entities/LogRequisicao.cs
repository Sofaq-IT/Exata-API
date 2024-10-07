using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Domain.Entities;

[Table("LogRequisicoes")]
public class LogRequisicao
{
    public DateTime Data { get; set; }

    [StringLength(450)]
    [Display(Name = "Usuário")]
    public string UsuarioID { get; set; }

    [StringLength(10)]
    [Display(Name = "Método")]
    public string Metodo { get; set; }

    [StringLength(50)]
    public string Controller { get; set; }

    [StringLength(50)]
    public string Action { get; set; }

    public string Body { get; set; }

    [StringLength(50)]
    public string RemoteAddress { get; set; }

    [StringLength(50)]
    public string TraceID { get; set; }
}
