using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Domain.DTO;

public class ErrorRequestDTO
{
    public string type { get; set; } = null;

    public string title { get; set; }

    public int status { get; set; } = 400;

    public string errors { get; set; } = null;

    public string[] mensagens { get; set; }

    public string traceID { get; set; } = null;

}
