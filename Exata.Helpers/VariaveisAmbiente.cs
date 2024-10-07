using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Helpers.Interfaces;

namespace Exata.Helpers;

public class VariaveisAmbiente : IVariaveisAmbiente
{
    public string SecretKey { get ; set ; }

    public string ValidAudience { get ; set ; }

    public string ValidIssuer { get ; set ; }

    public string TokenValidityInMinutes { get ; set ; }

    public string RefreshTokenValidityInMinutes { get ; set ; }

    public string UsuarioADM { get; set; }

    public string[] LogarRequisicoes { get; set; }

    public string TotvsRMUsuario { get; set; }

    public string TotvsRMSenha { get; set; }

    public string TotvsRMSenhaCripto { get; set; }

    public string TotvsRMAPI { get; set; }

    public string TotvsRMEndPointToken { get; set; }

    public string TotvsRMEndPointFuncionarios { get; set; }

    public string Licenca { get; }

    public VariaveisAmbiente(string secretKey,
                             string validAudience,
                             string validIssuer,
                             string tokenValidityInMinutes,
                             string refreshTokenValidityInMinutes,
                             string usuarioADM,
                             string logarRequisicoes,
                             string licenca)
    {
        SecretKey = secretKey;
        ValidAudience = validAudience;
        ValidIssuer = validIssuer;
        TokenValidityInMinutes = tokenValidityInMinutes;
        RefreshTokenValidityInMinutes = refreshTokenValidityInMinutes;
        UsuarioADM = usuarioADM;
        LogarRequisicoes = logarRequisicoes.Split(",");
        Licenca = licenca;
    }
}
