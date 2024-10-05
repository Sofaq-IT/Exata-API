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
public class TelefoneController : ControllerBase
{
    private readonly ITelefones _telefone;
    private readonly ICampo _campo;
    private readonly JsonSerializerOptions _jsonOptions;

    public TelefoneController(ITelefones telefone, ICampo campo)
    {
        _telefone = telefone;
        _campo = campo;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Inclui o Telefone
    /// </summary>
    /// <param name="Telefones">Objeto Telefone</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Telefones>> Post([FromBody] Telefones telefone)
    {
        try
        {
            if (_telefone.Existe(telefone.Telefone) == true)
                throw new Exception("Telefone já cadastrado.");

            await _telefone.Inserir(telefone);
            return Ok(telefone);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Alterar o Telefone
    /// </summary>
    /// <param name="Telefones">Objeto Contato</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Telefones>> Put([FromBody] Telefones telefone)
    {
        try
        {
            if (_telefone.Existe(telefone.TelefoneID) == false)
                return NotFound();

            if (_telefone.Existe(telefone.Telefone) == true)
                throw new Exception("Telefone já cadastrado.");

            await _telefone.Atualizar(telefone);
            return Ok(telefone);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir o Telefone
    /// </summary>
    /// <param name="id">Id do Telefone</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (_telefone.Existe(id) == false)
                return NotFound();

            await _telefone.Excluir(id);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna o Telefone
    /// </summary>
    /// <param name="id">ID do Telefone</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Telefones>> Get(int id)
    {
        try
        {
            if (_telefone.Existe(id) == false)
                return NotFound();

            return Ok(await _telefone.Abrir(id));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna a Lista de Usuários
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

            PagedList<Telefones> telefones = await _telefone.Listar(_paginacao);

            if (telefones.TotalPaginas == 0)
                return NotFound();

            if (_paginacao.Pagina > telefones.TotalPaginas)
            {
                _paginacao.Pagina = telefones.TotalPaginas;
                telefones = await _telefone.Listar(_paginacao);
            }

            Response.Headers.Append("x-Paginacao", telefones.JsonHeaderPaginacao());
            return Ok(telefones);
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
            return Ok(await _campo.Campos("Telefone"));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
