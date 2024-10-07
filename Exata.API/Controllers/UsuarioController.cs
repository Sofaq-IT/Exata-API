using Microsoft.AspNetCore.Mvc;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Exata.Domain.Paginacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para manter dados de Usuários
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IErrorRequest _error;
    private readonly ILicenca _licenca;
    private readonly IVariaveisAmbiente _varAmbiente;
    private readonly IFuncoes _funcoes;

    /// <summary>
    /// Controller utilizado para manter dados de Usuários
    /// </summary>
    /// <param name="uof"></param>
    /// <param name="userManager"></param>
    /// <param name="errorRequest"></param>
    /// <param name="funcoes"></param>
    /// <param name="licenca"></param>
    /// <param name="varAmbiente"></param>
    public UsuarioController(IUnitOfWork uof,
                             UserManager<ApplicationUser> userManager,
                             IErrorRequest errorRequest,
                             IFuncoes funcoes,
                             ILicenca licenca,
                             IVariaveisAmbiente varAmbiente)
    {
        _uof = uof;
        _userManager = userManager;
        _error = errorRequest;
        _error.Titulo = "Usuário";
        _varAmbiente = varAmbiente;
        _funcoes = funcoes;
        _licenca = licenca;
    }

    /// <summary>
    /// Inclui o Usuário
    /// </summary>
    /// <param name="usuario">Objeto Usuario</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<UsuarioDTO>> Post([FromBody] UsuarioDTO usuario)
    {
        if (usuario == null)
            return BadRequest(_error.BadRequest("Usuário não informado."));

        var userExists = await _userManager.FindByNameAsync(usuario.UserName);
        if (userExists != null)
            return BadRequest(_error.BadRequest("Usuário já existe."));

        if (usuario.Ativo == true)
            if (await _uof.Usuario.QtdeAtivos(_varAmbiente.UsuarioADM) >= _licenca.DadosLicenca.QtdeUsuarios)
                return BadRequest(_error.BadRequest($"Limite de licenças ativas atingida ({_licenca.DadosLicenca.QtdeUsuarios})."));

        if (string.IsNullOrEmpty(usuario.Senha))
            return BadRequest(_error.BadRequest("Informar a senha do usuário."));

        ApplicationUser user = new()
        {
            Email = usuario.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = usuario.UserName,
            PerfilID = usuario.PerfilID,
            Nome = usuario.Nome,
            Ativo = usuario.Ativo,
            booNovo = true,
            PhoneNumber = usuario.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, usuario.Senha);

        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

        await _uof.Commit();
        return Ok(usuario);
    }

    /// <summary>
    /// Alterar o Usuário
    /// </summary>
    /// <param name="usuario">Objeto Usuário</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<UsuarioDTO>> Put([FromBody] UsuarioDTO usuario)
    {
        var user = await _userManager.FindByNameAsync(usuario.UserName);
        if (user == null)
            return NotFound(_error.NotFound("Usuário não encontrado"));

        if (user.Ativo != usuario.Ativo && usuario.Ativo == true)
        {
            if (await _uof.Usuario.QtdeAtivos(_varAmbiente.UsuarioADM) >= _licenca.DadosLicenca.QtdeUsuarios)
                return BadRequest(_error.BadRequest($"Limite de licenças ativas atingida ({_licenca.DadosLicenca.QtdeUsuarios})."));
        }

        user.Email = usuario.Email;
        user.PerfilID = usuario.PerfilID;
        user.DataAlteracao = DateTime.Now;
        user.Nome = usuario.Nome;
        user.Ativo = usuario.Ativo;
        user.PhoneNumber = usuario.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

        await _uof.Commit();
        return Ok(usuario);
    }

    /// <summary>
    /// Excluir o Usuário
    /// </summary>
    /// <param name="id">Id do Usuário</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(_error.NotFound("Usuário não encontrado"));

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

        await _uof.Commit();
        return Ok();
    }

    /// <summary>
    /// Usuário Alterando a Senha
    /// </summary>
    /// <param name="usuarioDTO">Objeto Usuário</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPatch, Route("AlterarSenha")]
    public async Task<ActionResult<UsuarioSenhaDTO>> AlterarSenha([FromBody] UsuarioSenhaDTO usuarioDTO)
    {
        if (usuarioDTO.SenhaNova != usuarioDTO.SenhaNovaConfirmacao)
            return BadRequest(_error.BadRequest("Senhas novas não conferem!"));

        var user = await _userManager.FindByNameAsync(usuarioDTO.Login);
        if (user == null)
            return NotFound(_error.NotFound("Usuário não encontrado"));

        var result = await _userManager.ChangePasswordAsync(user, usuarioDTO.SenhaAntiga, usuarioDTO.SenhaNova);
        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

        await _uof.Commit();
        return Ok(usuarioDTO);
    }

    /// <summary>
    /// ADM Alterando a Senha do usuário
    /// </summary>
    /// <param name="usuarioDTO">Objeto Usuário</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPatch, Route("AlterarSenhaADM")]
    public async Task<ActionResult<UsuarioSenhaDTO>> AlterarSenhaADM([FromBody] UsuarioSenhaDTO usuarioDTO)
    {
        var user = await _userManager.FindByNameAsync(usuarioDTO.Login);
        if (user == null)
            return NotFound(_error.NotFound("Usuário não encontrado"));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, usuarioDTO.SenhaNova);
        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

        await _uof.Commit();
        return Ok(usuarioDTO);
    }

    /// <summary>
    /// Retorna o Usuário
    /// </summary>
    /// <param name="id">ID do Usuário</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<ApplicationUser>> Get(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(_error.NotFound("Usuário não encontrado"));

        user = await _uof.Usuario.Abrir(id);

        await _uof.Commit();
        return Ok(user);
    }

    /// <summary>
    /// Retorna a Lista de Usuários
    /// </summary>
    /// <returns>Lista solicitada</returns>
    [HttpGet, Route("Listar")]
    public async Task<IActionResult> Listar()
    {
        var user = await _userManager.Users.Select(x => x.Ativo == true).ToListAsync();

        if (!_funcoes.ValidacaoHeaderPaginacao(Request.Headers))
        {
            return BadRequest(_error.BadRequest(_funcoes.errors));
        }

        PagedList<ApplicationUser> usuarios = await _uof.Usuario.Listar(_funcoes.Paginacao, _varAmbiente.UsuarioADM);

        if (usuarios.TotalPaginas == 0)
            return NotFound(_error.NotFound("Não existe registros."));

        if (_funcoes.Paginacao.Pagina > usuarios.TotalPaginas)
        {
            _funcoes.Paginacao.Pagina = usuarios.TotalPaginas;
            usuarios = await _uof.Usuario.Listar(_funcoes.Paginacao, _varAmbiente.UsuarioADM);
        }

        Response.Headers.Append("x-Paginacao", usuarios.JsonHeaderPaginacao());

        foreach (ApplicationUser usuario in usuarios)
        {
            usuario.RefreshToken = null;
            usuario.PasswordHash = null;
            usuario.SecurityStamp = null;
            usuario.ConcurrencyStamp = null;
        }

        await _uof.Commit();
        return Ok(usuarios);
    }

    /// <summary>
    /// Retorna a Lista de Campos utilizados na pesquisa e ordenação do GRID Usuários
    /// </summary>
    /// <returns>Lista de Campos</returns>
    [HttpGet, Route("Campos")]
    public async Task<ActionResult<Campo>> Campos()
    {
        await _uof.Commit();
        return Ok(await _uof.Campo.Campos("Usuario"));
    }

    /// <summary>
    /// Cria o usuário ADM do Sistema
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet, Route("UsuarioADM")]
    public async Task<IActionResult> UsuarioADM()
    {
        ApplicationUser UsuarioADM = new()
        {
            UserName = "WAutom",
            Email = "wautom@Exata.com.br",
            SecurityStamp = Guid.NewGuid().ToString(),
            Nome = "WAutom ADM",
            Ativo = true,
            booNovo = true
        };

        var userExists = await _userManager.FindByNameAsync(UsuarioADM.UserName);
        if (userExists == null)
        {
            var result = await _userManager.CreateAsync(UsuarioADM, "W@utom7!");
            if (!result.Succeeded)
            {
                return BadRequest(_error.BadRequest(result));
            }
        }
        await _uof.Commit();
        return Ok();
    }

    /// <summary>
    /// Retorna os Perfis
    /// </summary>
    /// <returns>O objeto solicitado</returns>
    [HttpGet, Route("Perfil")]
    public async Task<ActionResult<Perfil>> Perfil()
    {
        await _uof.Commit();
        return Ok(await _uof.Perfil.Listar());
    }

    /// <summary>
    /// Alterar Tema do Usuário
    /// </summary>
    /// <param name="usuario">Objeto Usuário</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPatch, Route("AlterarTema")]
    public async Task<ActionResult<UsuarioTemaDTO>> AlterarTema([FromBody] UsuarioTemaDTO usuario)
    {
        var user = await _userManager.FindByNameAsync(usuario.UserName);
        if (user == null)
            return NotFound(_error.NotFound("Usuário não encontrado"));

        user.Tema = usuario.Tema;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

        await _uof.Commit();
        return Ok(usuario);
    }

    /// <summary>
    /// Incluir/Alterar Avatar do Usuário
    /// </summary>
    /// <param name="usuarioAvatar">Objeto Logo do Usuário</param>
    /// <returns>O objeto incluido/alterado.</returns>
    [HttpPatch, Route("IncluirAvatar")]
    public async Task<ActionResult<UsuarioAvatar>> IncluirAvatar([FromBody] UsuarioAvatar usuarioAvatar)
    {
        var user = await _userManager.FindByNameAsync(usuarioAvatar.Login);
        if (user == null)
            return NotFound(_error.NotFound("Usuário não encontrado."));

        if (usuarioAvatar == null)
            return BadRequest(_error.BadRequest("Avatar não informado."));

        if (string.IsNullOrEmpty(usuarioAvatar.Avatar))
            return BadRequest(_error.BadRequest("Logo não informado."));

        usuarioAvatar.UsuarioID = user.Id;
        UsuarioAvatar usuarioAvatarNovo;

        if (_uof.Usuario.ExisteAvatar(usuarioAvatar.UsuarioID))
            usuarioAvatarNovo = _uof.Usuario.AlterarAvatar(usuarioAvatar);
        else
            usuarioAvatarNovo = await _uof.Usuario.InserirAvatar(usuarioAvatar);

        await _uof.Commit();
        return Ok(usuarioAvatarNovo);
    }
}
