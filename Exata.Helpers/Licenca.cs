using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Exata.Domain.DTO;
using Exata.Helpers.Interfaces;

namespace Exata.Helpers;

public class Licenca : ILicenca
{
    private readonly ICripto _cripto;
    private readonly IVariaveisAmbiente _variaveis;
    private readonly JsonSerializerOptions _jsonOptions;

    public LicencaDTO DadosLicenca { get; }

    public Licenca(ICripto cripto,
                   IVariaveisAmbiente variaveisAmbiente)
    {
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _cripto = cripto;
        _variaveis = variaveisAmbiente;

        if (!string.IsNullOrEmpty(_variaveis.Licenca))
        {
            DadosLicenca = JsonSerializer.Deserialize<LicencaDTO>(
                _cripto.Descriptografar(_variaveis.Licenca), _jsonOptions);

            DadosLicenca.Licenca = _variaveis.Licenca;
        }
    }
}
