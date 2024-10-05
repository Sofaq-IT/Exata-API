using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Helpers.Interfaces;

namespace Exata.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContatoController : ControllerBase
{
    private readonly IContato _contato;
    private readonly IContatoTipo _contatoTipo;
    private readonly ITipoContato _tipoContato;
    private readonly ICampo _campo;
    private readonly JsonSerializerOptions _jsonOptions;

    public ContatoController(IContato contato, IContatoTipo contatoTipo, ITipoContato tipoContato, ICampo campo)
    {
        _contato = contato;
        _contatoTipo = contatoTipo;
        _tipoContato = tipoContato;
        _campo = campo;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Inclui o Contato
    /// </summary>
    /// <param name="Contato">Objeto Contato</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Contato>> Post([FromBody] Contato contato)
    {
        try
        {
            if (_contato.Existe(contato.TelefoneID) == true)
                throw new Exception("Contato já cadastrado.");
            
            await _contato.Inserir(contato);
            return Ok(contato);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Alterar o Contato
    /// </summary>
    /// <param name="Contato">Objeto Contato</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Contato>> Put([FromBody] Contato contato)
    {
        try
        {
            if (_contato.Existe(contato.TelefoneID) == false)
                return NotFound();
            
            await _contato.Atualizar(contato);
            return Ok(contato);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir o Contato
    /// </summary>
    /// <param name="id">Id do Contato</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(string id)
    {
        try
        {
            if (_contato.Existe(id) == false)
                return NotFound();

            await _contato.Excluir(id);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna o Contato
    /// </summary>
    /// <param name="id">ID do Contato</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Contato>> Get(string id)
    {
        try
        {
            if (_contato.Existe(id) == false)
                return NotFound();

            return Ok(await _contato.Abrir(id));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna a Lista de Contatos
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

            PagedList<Contato> contatos = await _contato.Listar(_paginacao);

            if (contatos.TotalPaginas == 0)
                return NotFound();

            if (_paginacao.Pagina > contatos.TotalPaginas)
            {
                _paginacao.Pagina = contatos.TotalPaginas;
                contatos = await _contato.Listar(_paginacao);
            }

            Response.Headers.Append("x-Paginacao", contatos.JsonHeaderPaginacao());
            return Ok(contatos);
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
            return Ok(await _campo.Campos("Contato"));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Inclui o Contato Tipo
    /// </summary>
    /// <param name="ContatoTipo">Objeto Contato Tipo</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost, Route("ContatoTipo")]
    public async Task<ActionResult<ContatoTipo>> InserirContatoTipo([FromBody] ContatoTipo contatoTipo)
    {
        try
        {
            if (_contato.Existe(contatoTipo.TelefoneID) == false)
                return NotFound();

            if (_contatoTipo.Existe(contatoTipo) == true)
                throw new Exception("Contato Tipo já cadastrado.");

            await _contatoTipo.Inserir(contatoTipo);
            return Ok(contatoTipo);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir o Contato Tipo
    /// </summary>
    /// <param name="id">Id do Contato</param>
    /// <returns></returns>
    [HttpDelete(), Route("ContatoTipo")]
    public async Task<ActionResult> DeleteContatoTipo([FromBody] ContatoTipo contatoTipo)
    {
        try
        {
            if (_contatoTipo.Existe(contatoTipo) == false)
                return NotFound();

            await _contatoTipo.Excluir(contatoTipo);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna o Tipos de Contatos
    /// </summary>
    /// <param name="id">ID do Tipos de Contatos</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet, Route("ContatoTipo")]
    public async Task<ActionResult<TipoContato>> Get()
    {
        try
        {
            return Ok(await _tipoContato.Listar());
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
