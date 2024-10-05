using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;

namespace Exata.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PerfilController : ControllerBase
{
    private readonly IPerfil _perfil;
    private readonly IPerfilSecao _perfilSecao;
    private readonly ISecao _secao;
    private readonly ICampo _campo;
    private readonly JsonSerializerOptions _jsonOptions;

    public PerfilController(IPerfil perfil, IPerfilSecao perfilSecao, ISecao secao, ICampo campo)
    {
        _perfil = perfil;
        _perfilSecao = perfilSecao;
        _secao = secao;
        _campo = campo;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Inclui o Perfil
    /// </summary>
    /// <param name="Perfil">Objeto Perfil</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Perfil>> Post([FromBody] Perfil perfil)
    {
        try
        {
            if (_perfil.Existe(perfil.PerfilID) == true)
                throw new Exception("Perfil já cadastrado.");

            await _perfil.Inserir(perfil);
            return Ok(perfil);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Alterar o Perfil
    /// </summary>
    /// <param name="Perfil">Objeto Perfil</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Perfil>> Put([FromBody] Perfil perfil)
    {
        try
        {
            if (_perfil.Existe(perfil.PerfilID) == false)
                return NotFound();

            await _perfil.Atualizar(perfil);
            return Ok(perfil);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir o Perfil
    /// </summary>
    /// <param name="id">Id do Perfil</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (_perfil.Existe(id) == false)
                return NotFound();

            await _perfil.Excluir(id);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna o Perfil
    /// </summary>
    /// <param name="id">ID do Perfil</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Perfil>> Get(int id)
    {
        try
        {
            if (_perfil.Existe(id) == false)
                return NotFound();

            return Ok(await _perfil.Abrir(id));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna a Lista de Perfis
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

            PagedList<Perfil> perfis = await _perfil.Listar(_paginacao);

            if (perfis.TotalPaginas == 0)
                return NotFound();

            if (_paginacao.Pagina > perfis.TotalPaginas)
            {
                _paginacao.Pagina = perfis.TotalPaginas;
                perfis = await _perfil.Listar(_paginacao);
            }

            Response.Headers.Append("x-Paginacao", perfis.JsonHeaderPaginacao());
            return Ok(perfis);
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
            return Ok(await _campo.Campos("Perfil"));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Inclui o Contato Tipo
    /// </summary>
    /// <param name="perfilSecao">Objeto Contato Tipo</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost, Route("PerfilSecao")]
    public async Task<ActionResult<PerfilSecao>> InserirPerfilSecao([FromBody] PerfilSecao perfilSecao)
    {
        try
        {
            if (_perfil.Existe(perfilSecao.PerfilID) == false)
                return NotFound();

            if (_perfilSecao.Existe(perfilSecao) == true)
                throw new Exception("Seção já cadastrado.");

            await _perfilSecao.Inserir(perfilSecao);
            return Ok(perfilSecao);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir o Perfil Seção
    /// </summary>
    /// <param name="id">Id do Contato</param>
    /// <returns></returns>
    [HttpDelete(), Route("PerfilSecao")]
    public async Task<ActionResult> DeletePerfilSecao([FromBody] PerfilSecao perfilSecao)
    {
        try
        {
            if (_perfilSecao.Existe(perfilSecao) == false)
                return NotFound();

            await _perfilSecao.Excluir(perfilSecao);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna o Perfil Seção
    /// </summary>
    /// <param name="id">ID do Perfil Seção</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet, Route("PerfilSecao")]
    public async Task<ActionResult<PerfilSecao>> Get()
    {
        try
        {
            return Ok(await _secao.Listar());
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
