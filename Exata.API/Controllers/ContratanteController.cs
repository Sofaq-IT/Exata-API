using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para manter dados do Contratante
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ContratanteController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IErrorRequest _error;
    private readonly ILicenca _licenca;

    /// <summary>
    /// Controller utilizado para manter dados do Contratante
    /// </summary>
    /// <param name="uof"></param>
    /// <param name="error"></param>
    /// <param name="licenca"></param>
    public ContratanteController(IUnitOfWork uof,
                                 IErrorRequest error,
                                 ILicenca licenca)
    {
        _uof = uof;
        _error = error;
        _error.Titulo = "Contratante";
        _licenca = licenca;
    }

    /// <summary>
    /// Alterar o Contratante
    /// </summary>
    /// <param name="contratante">Objeto Contratante</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Contratante>> Put([FromBody] Contratante contratante)
    {
        Contratante contratanteNovo;

        if (_uof.Contratante.Existe() == false)
            contratanteNovo = await _uof.Contratante.Inserir(contratante);
        else
        {
            Contratante contratanteExistente = await _uof.Contratante.Abrir();
            contratante.Id = contratanteExistente.Id;
            contratanteNovo = _uof.Contratante.Alterar(contratante);
        }
            

        await _uof.Commit();
        return Ok(contratanteNovo);
    }

    /// <summary>
    /// Retorna o Contratante
    /// </summary>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<ContratanteDTO>> Get()
    {
        ContratanteDTO contratanteDTO = new()
        {
            CNPJ = _licenca.DadosLicenca.CNPJ,
            Empresa = _licenca.DadosLicenca.Empresa
        };

        if (_uof.Contratante.Existe()) 
        {
            Contratante contratante = await _uof.Contratante.Abrir();
            contratanteDTO.Rua = contratante.Rua;
            contratanteDTO.Numero = contratante.Numero;
            contratanteDTO.Complemento = contratante.Complemento;
            contratanteDTO.Bairro = contratante.Bairro;
            contratanteDTO.Cidade = contratante.Cidade;
            contratanteDTO.Estado = contratante.Estado;
            contratanteDTO.Logo = contratante.Logo;
        }

        await _uof.Commit();
        return Ok(contratanteDTO);
    }

    /// <summary>
    /// Incluir/Alterar Logo da Empresa
    /// </summary>
    /// <param name="contratante">Objeto Logo da Empresa</param>
    /// <returns>O objeto incluido/alterado.</returns>
    [HttpPatch, Route("IncluirLogo")]
    public async Task<ActionResult<Contratante>> IncluirLogo([FromBody] Contratante contratante)
    {
        Contratante contratanteNovo;

        if (_uof.Contratante.Existe())
        {
            contratanteNovo = await _uof.Contratante.Abrir();
            contratanteNovo.Logo = contratante.Logo;
            contratanteNovo = _uof.Contratante.AlterarLogo(contratanteNovo);
        }
        else
            contratanteNovo = await _uof.Contratante.InserirLogo(contratante);

        await _uof.Commit();
        return Ok(contratanteNovo);
    }

    /// <summary>
    /// Retorna o Logo do Contratante
    /// </summary>
    /// <returns>O objeto solicitado</returns>
    [AllowAnonymous]
    [HttpGet, Route("Logo")]
    public async Task<ActionResult<string>> Logo()
    {
        if (!_uof.Contratante.Existe())
        {
            return "";
        }

        return Ok(await _uof.Contratante.Logo());
    }

}
