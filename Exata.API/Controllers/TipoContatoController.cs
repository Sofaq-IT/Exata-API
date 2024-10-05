using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Helpers.Interfaces;

namespace Exata.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoContatoController : ControllerBase
{
    private readonly ITipoContato _tipoContato;
    private readonly ICampo _campo;
    private readonly JsonSerializerOptions _jsonOptions;

    public TipoContatoController(ITipoContato tipoContato, ICampo campo)
    {
        _tipoContato = tipoContato;
        _campo = campo;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Inclui o Tipo de Contato
    /// </summary>
    /// <param name="tipoContato">Objeto Tipo de Contato</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<TipoContato>> Post([FromBody] TipoContato tipoContato)
    {
        try
        {
            if (_tipoContato.Existe(tipoContato.TipoContatoID) == true)
                throw new Exception("Tipo de Contato já cadastrado.");

            await _tipoContato.Inserir(tipoContato);                
            return Ok(tipoContato);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Alterar o Tipo de Contato
    /// </summary>
    /// <param name="tipoContato">Objeto do Tipo de Contato</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<TipoContato>> Put([FromBody] TipoContato tipoContato)
    {
        try
        {                
            if (_tipoContato.Existe(tipoContato.TipoContatoID) == false)
                return NotFound();

            await _tipoContato.Atualizar(tipoContato);
            return Ok(tipoContato);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir o Tipo de Contato
    /// </summary>
    /// <param name="id">Id do Tipo de Contato</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (_tipoContato.Existe(id) == false)
                return NotFound();
                                            
            await _tipoContato.Excluir(id);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna o Tipo de Contato
    /// </summary>
    /// <param name="id">ID do Tipo de Contato</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<TipoContato>> Get(int id)
    {
        try
        {
            if (_tipoContato.Existe(id) == false)
                return NotFound();

            return Ok(await _tipoContato.Abrir(id));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna a Lista de Tipos de Contatos
    /// </summary>
    /// <returns>Lista solicitada</returns>
    [HttpGet, Route("Listar")]
    public async Task<IActionResult> Listar()
    {
        try
        {
            PaginacaoDTO _paginacao = null;

            if (Request.Headers.TryGetValue("x-Paginacao", out var hPaginacao))
            {
                _paginacao = JsonSerializer.Deserialize<PaginacaoDTO>(hPaginacao, _jsonOptions);
                if (!TryValidateModel(_paginacao))
                    return BadRequest(ModelState);
            }
            else
            {
                ModelState.AddModelError("Pagina",
                            "Header x-Paginacao não informado.");
                return BadRequest(ModelState);
            }

            PagedList<TipoContato> tiposContatos = await _tipoContato.Listar(_paginacao);

            if (tiposContatos.TotalPaginas == 0)
                return NotFound();

            if (_paginacao.Pagina > tiposContatos.TotalPaginas)
            {
                _paginacao.Pagina = tiposContatos.TotalPaginas;
                tiposContatos = await _tipoContato.Listar(_paginacao);
            }

            Response.Headers.Append("x-Paginacao", tiposContatos.JsonHeaderPaginacao());
            return Ok(tiposContatos);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna a Lista de Campos
    /// </summary>
    /// <returns>Lista de Campos</returns>
    [HttpGet, Route("Campos")]
    public async Task<ActionResult<Campo>> Campos()
    {
        try
        {
            return Ok(await _campo.Campos("TipoContato"));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
