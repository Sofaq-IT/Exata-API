using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exata.Helpers.Interfaces;

public interface IVariaveisAmbiente
{
    public string SecretKey { get; set; }

    public string ValidAudience { get; set; }

    public string ValidIssuer { get; set; }

    public string TokenValidityInMinutes { get; set; }

    public string RefreshTokenValidityInMinutes { get; set; }

    public string UsuarioADM { get; set; }

    public string[] LogarRequisicoes { get; set; }

    public string Licenca { get; }
}
