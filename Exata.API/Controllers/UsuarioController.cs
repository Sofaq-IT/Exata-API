using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;
using System.Linq;
using Exata.Helpers;

namespace Exata.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuario _usuario;
    private readonly ICampo _campo;
    private readonly ICripto _cripto;
    private readonly JsonSerializerOptions _jsonOptions;

    public UsuarioController(IUsuario usuario, ICampo campo, ICripto cripto)
    {
        _usuario = usuario;
        _campo = campo;
        _cripto = cripto;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Inclui o Usuário
    /// </summary>
    /// <param name="Usuario">Objeto Usuario</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Usuario>> Post([FromBody] Usuario usuario)
    {
        try
        {            
            if (_usuario.Existe(usuario.UsuarioID) == true)
                throw new Exception("Contato já cadastrado.");

            usuario.Senha = _cripto.Criptografar(usuario.Senha);

            await _usuario.Inserir(usuario);
            return Ok(usuario);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Alterar o Usuário
    /// </summary>
    /// <param name="Usuario">Objeto Usuário</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Usuario>> Put([FromBody] Usuario usuario)
    {
        try
        {
            if (_usuario.Existe(usuario.UsuarioID) == false)
                return NotFound();

            await _usuario.Atualizar(usuario);
            return Ok(usuario);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Excluir o Usuário
    /// </summary>
    /// <param name="id">Id do Usuário</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (_usuario.Existe(id) == false)
                return NotFound();

            await _usuario.Excluir(id);
            return Ok();
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Usuário Alterando a Senha
    /// </summary>
    /// <param name="Usuario">Objeto Usuário</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut, Route("AlterarSenha")]
    public async Task<ActionResult<UsuarioSenhaDTO>> AlterarSenha([FromBody] UsuarioSenhaDTO usuarioDTO)
    {
        try
        {
            if (usuarioDTO.SenhaNova != usuarioDTO.SenhaNovaConfirmacao)
                throw new Exception("Senhas novas não conferem!");

            if (_usuario.Existe(usuarioDTO.UsuarioID) == false)
                return NotFound();

            Usuario usuario = await _usuario.Abrir(usuarioDTO.UsuarioID);

            if (_cripto.Descriptografar(usuario.Senha) != usuarioDTO.SenhaAntiga)
                throw new Exception("Senha utilizada não confere com senha antiga digitada.");

            if (_cripto.Descriptografar(usuario.Senha) == usuarioDTO.SenhaNova)
                throw new Exception("Não pode ser igual a última senha utilizada.");

            usuario.Senha = _cripto.Criptografar(usuarioDTO.SenhaNova);
            await _usuario.AlterarSenha(usuario);
            return Ok(usuario);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// ADM Alterando a Senha
    /// </summary>
    /// <param name="Usuario">Objeto Usuário</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut, Route("AlterarSenhaADM")]
    public async Task<ActionResult<UsuarioSenhaDTO>> AlterarSenhaADM([FromBody] UsuarioSenhaDTO usuarioDTO)
    {
        try
        {            
            if (_usuario.Existe(usuarioDTO.UsuarioID) == false)
                return NotFound();

            Usuario usuario = new Usuario {
                UsuarioID = usuarioDTO.UsuarioID,
                Senha = _cripto.Criptografar(usuarioDTO.SenhaNova)
            };
            
            await _usuario.AlterarSenhaADM(usuario);
            return Ok(usuario);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retorna o Usuário
    /// </summary>
    /// <param name="id">ID do Usuário</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Usuario>> Get(int id)
    {
        try
        {
            if (_usuario.Existe(id) == false)
                return NotFound();

            Usuario usuario = await _usuario.Abrir(id);
            usuario.Senha = null;
            return Ok(usuario);
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

            PagedList<Usuario> usuarios = await _usuario.Listar(_paginacao);

            if (usuarios.TotalPaginas == 0)
                return NotFound();

            if (_paginacao.Pagina > usuarios.TotalPaginas)
            {
                _paginacao.Pagina = usuarios.TotalPaginas;
                usuarios = await _usuario.Listar(_paginacao);
            }

            Response.Headers.Append("x-Paginacao", usuarios.JsonHeaderPaginacao());
            return Ok(usuarios);
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
            return Ok(await _campo.Campos("Usuario"));
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
