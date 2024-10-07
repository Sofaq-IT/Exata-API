using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Exata.Domain.DTO;
using Exata.Helpers.Interfaces;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para Autenticação
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
    private readonly IToken _token;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _uof;
    private readonly ICripto _cripto;
    private readonly IErrorRequest _error;
    private readonly ILicenca _licenca;
    private readonly IVariaveisAmbiente _varAmbiente;

    /// <summary>
    /// Controller utilizado para Autenticação
    /// </summary>
    /// <param name="token"></param>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="uof"></param>
    /// <param name="cripto"></param>
    /// <param name="error"></param>
    /// <param name="licenca"></param>
    /// <param name="varAmbiente"></param>
    public AutenticacaoController(IToken token,
                                  UserManager<ApplicationUser> userManager,
                                  RoleManager<IdentityRole> roleManager,
                                  IUnitOfWork uof,
                                  ICripto cripto,
                                  IErrorRequest error,
                                  ILicenca licenca,
                                  IVariaveisAmbiente varAmbiente)
    {
        _token = token;
        _userManager = userManager;
        _roleManager = roleManager;
        _uof = uof;
        _cripto = cripto;
        _error = error;
        _error.Titulo = "Autorização";
        _licenca = licenca;
        _varAmbiente = varAmbiente;
    }

    /// <summary>
    /// Realiza o Login no sistema
    /// </summary>
    /// <param name="login">Objeto Login</param>
    /// <returns></returns>
    [HttpPost, Route("Login")]
    public async Task<ActionResult<AutorizacaoDTO>> Login([FromBody] LoginDTO login)
    {
        if (!string.Equals(login.Usuario, _varAmbiente.UsuarioADM))
            if (DateTime.Now >= _licenca.DadosLicenca.DataValidade.AddDays(20))
                return BadRequest(_error.BadRequest($"Expirado a Validade da Licença! ({_licenca.DadosLicenca.DataValidade})"));

        var user = await _userManager.FindByNameAsync(login.Usuario!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, login.Senha!))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = _token.GenerateAccessToken(authClaims, _varAmbiente);
            var refreshToken = _token.GenerateRefreshToken();
            _ = int.TryParse(_varAmbiente.RefreshTokenValidityInMinutes,
                               out int refreshTokenValidityInMinutes);

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
            user.RefreshToken = refreshToken;
            user.DataUltLogin = DateTime.Now;
            await _userManager.UpdateAsync(user);

            AutorizacaoDTO autoriza = new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenAtualizacao = refreshToken,
                ExpiraEm = token.ValidTo,
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Tema = user.Tema
            };

            if (user.PerfilID != null)
            {
                autoriza.PerfilID = (int)user.PerfilID;
                Perfil perfil = await _uof.Perfil.Abrir(autoriza.PerfilID);
                autoriza.Perfil = perfil.Descricao;

                if (!string.Equals(login.Usuario, _varAmbiente.UsuarioADM))
                    autoriza.Menus = perfil.PerfilControllerAction.Select(x => x.ControllerAction.Controller).OrderBy(x => x).Distinct().ToList();
                else
                {
                    List<ControllerAction> controllerAction = await _uof.ControllerAction.Listar();
                    autoriza.Menus = controllerAction.Select(x => x.Controller).OrderBy(x => x).Distinct().ToList();
                }
            }

            if (_uof.Usuario.ExisteAvatar(user.Id))
                autoriza.Avatar = await _uof.Usuario.Avatar(user.Id);

            if (_licenca.DadosLicenca != null)
                autoriza.ValidadeLicenca = _licenca.DadosLicenca.DataValidade;
            return Ok(autoriza);
        }
        return Unauthorized();
    }

    /// <summary>
    /// Realiza a atualização do Token quando expirado
    /// </summary>
    /// <param name="token">Objeto Token</param>
    /// <returns></returns>
    [HttpPost, Route("Token-atualizacao")]
    public async Task<ActionResult<AutorizacaoDTO>> RefreshToken([FromBody] AutorizacaoDTO token)
    {
        if (token is null)
        {
            return BadRequest(_error.BadRequest("Informe um registro válido!"));
        }

        var principal = _token.GetPrincipalFromExpiredToken(token.Token, _varAmbiente);

        if (principal == null)
        {
            return BadRequest(_error.BadRequest("Token informado inválido!"));
        }

        string usuario = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(usuario);
        if (user == null || user.RefreshToken != token.TokenAtualizacao
                         || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest(_error.BadRequest("Faça novo Login"));
        }

        var newAccessToken = _token.GenerateAccessToken(principal.Claims.ToList(), _varAmbiente);
        var newRefreshToken = _token.GenerateRefreshToken();
        _ = int.TryParse(_varAmbiente.RefreshTokenValidityInMinutes,
                               out int refreshTokenValidityInMinutes);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
        await _userManager.UpdateAsync(user);

        AutorizacaoDTO autoriza = new AutorizacaoDTO()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            TokenAtualizacao = newRefreshToken,
            ExpiraEm = newAccessToken.ValidTo
        };

        return Ok(autoriza);
    }

    /// <summary>
    /// Faz o Logout do sistema
    /// </summary>
    /// <param name="token">Objeto Token</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost, Route("Logout")]
    public async Task<IActionResult> Logout([FromBody] AutorizacaoDTO token)
    {
        if (token is null)
        {
            return BadRequest(_error.BadRequest("Informe um registro válido!"));
        }

        var principal = _token.GetPrincipalFromExpiredToken(token.Token, _varAmbiente);

        if (principal == null)
        {
            return BadRequest(_error.BadRequest("Token informado inválido!"));
        }

        string usuario = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(usuario);

        if (user == null)
            return BadRequest(_error.BadRequest("Usuário Inválido!"));

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    /// <summary>
    /// Criptografar string
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet, Route("Chave")]
    public IActionResult Chave(string id)
    {
        if (!string.Equals(HttpContext.User.Identity.Name.ToLower(), _varAmbiente.UsuarioADM.ToLower()))
            return Ok("");

        return Ok(_cripto.Criptografar(id));
    }

    /// <summary>
    /// Consulta Dados da Licença
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet, Route("Licenca")]
    public ActionResult<LicencaDTO> Licenca()
    {
        return Ok(_licenca.DadosLicenca);
    }
}
