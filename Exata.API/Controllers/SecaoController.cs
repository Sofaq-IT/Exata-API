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
public class SecaoController : ControllerBase
{
    private readonly ISecao _secao;
    private readonly ICampo _campo;
    private readonly JsonSerializerOptions _jsonOptions;

    public SecaoController(ISecao secao, ICampo campo)
    {
        _secao = secao;
        _campo = campo;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Inclui a Seção
    /// </summary>
    /// <param name="Secao">Objeto Seção</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Secao>> Post([FromBody] Secao secao)
    {
        try
        {
            if (_secao.Existe(secao.SecaoID) == true)
                throw new Exception("Seção já cadastrada.");

            await _secao.Inserir(secao);
            return Ok(secao);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Alterar o Seção
    /// </summary>
    /// <param name="Secao">Objeto Secao</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Secao>> Put([FromBody] Secao secao)
    {
        try
        {
            if (_secao.Existe(secao.SecaoID) == false)
                return NotFound();

            await _secao.Atualizar(secao);
            return Ok(secao);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir a Seção
    /// </summary>
    /// <param name="id">Id da Seção</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (_secao.Existe(id) == false)
                return NotFound();

            await _secao.Excluir(id);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna a Seção
    /// </summary>
    /// <param name="id">ID da Seção</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Secao>> Get(int id)
    {
        try
        {
            if (_secao.Existe(id) == false)
                return NotFound();

            return Ok(await _secao.Abrir(id));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna a Lista de Seções
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

            PagedList<Secao> secoes = await _secao.Listar(_paginacao);

            if (secoes.TotalPaginas == 0)
                return NotFound();

            if (_paginacao.Pagina > secoes.TotalPaginas)
            {
                _paginacao.Pagina = secoes.TotalPaginas;
                secoes = await _secao.Listar(_paginacao);
            }

            Response.Headers.Append("x-Paginacao", secoes.JsonHeaderPaginacao());
            return Ok(secoes);
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
            return Ok(await _campo.Campos("Secao"));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
