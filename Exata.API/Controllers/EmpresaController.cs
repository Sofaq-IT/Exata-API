using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para manter dados do Empresa
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EmpresaController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IErrorRequest _error;
    private readonly IFuncoes _funcoes;

    /// <summary>
    /// Controller utilizado para manter dados do Empresa
    /// </summary>
    /// <param name="uof"></param>
    /// <param name="userManager"></param>
    /// <param name="erro"></param>
    /// <param name="funcoes"></param>
    public EmpresaController(IUnitOfWork uof,
                            UserManager<ApplicationUser> userManager,
                            IErrorRequest erro,
                            IFuncoes funcoes)
    {
        _uof = uof;
        _userManager = userManager;
        _error = erro;
        _error.Titulo = "Empresa";
        _funcoes = funcoes;
    }

    /// <summary>
    /// Inclui a Empresa
    /// </summary>
    /// <param name="empresa">Objeto Empresa</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Empresa>> Post([FromBody] Empresa empresa)
    {
        if (_uof.Empresa.Existe(empresa.EmpresaID) == true)
            return BadRequest(_error.BadRequest("Empresa já cadastrada."));

        if (_uof.Empresa.Existe(empresa.CpfCnpj) == true)
            return BadRequest(_error.BadRequest("Empresa já cadastrada."));

        Empresa empresaNova = await _uof.Empresa.Inserir(empresa);

        await _uof.Commit();

        ApplicationUser user = new()
        {
            Email = empresa.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = empresa.Email,
            PerfilID = 2, // 2 - Empresa/Laboratório
            Nome = empresa.ApelidoNomeFantasia,
            Ativo = true,
            booNovo = true,
            PhoneNumber = empresa.Telefone,
            EmpresaID = empresaNova.EmpresaID
        };

        var senha = Helpers.Funcoes.GenerateRandomString(10);

        var result = await _userManager.CreateAsync(user, senha);

        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

        await _uof.Commit();

        _uof.Usuario.EnviarEmailNovoUsuario(user.Nome, user.UserName, user.Email, senha);

        return Ok(empresaNova);
    }

    /// <summary>
    /// Alterar a Empresa
    /// </summary>
    /// <param name="empresa">Objeto Empresa</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Empresa>> Put([FromBody] Empresa empresa)
    {
        if (_uof.Empresa.Existe(empresa.EmpresaID) == false)
            return NotFound(_error.NotFound());

        Empresa empresaAlterada = await _uof.Empresa.Atualizar(empresa);
        await _uof.Commit();
        return Ok(empresaAlterada);
    }

    /// <summary>
    /// Excluir a Empresa
    /// </summary>
    /// <param name="id">Id da Empresa</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        if (_uof.Empresa.Existe(id) == false)
            return NotFound(_error.NotFound());

        Empresa empresaExcluida = await _uof.Empresa.Excluir(id);
        await _uof.Commit();
        return Ok(empresaExcluida);
    }

    /// <summary>
    /// Retorna a Empresa
    /// </summary>
    /// <param name="id">ID da Empresa</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Empresa>> Get(int id)
    {
        if (_uof.Empresa.Existe(id) == false)
            return NotFound(_error.NotFound());

        await _uof.Commit();
        return Ok(await _uof.Empresa.Abrir(id));
    }

    /// <summary>
    /// Retorna a Lista de Empresas
    /// </summary>
    /// <returns>Lista solicitada</returns>
    [HttpGet, Route("Listar")]
    public async Task<IActionResult> Listar()
    {
        if (!_funcoes.ValidacaoHeaderPaginacao(Request.Headers))
        {
            return BadRequest(_error.BadRequest(_funcoes.errors));
        }

        PagedList<Empresa> empresas = await _uof.Empresa.Listar(_funcoes.Paginacao);

        if (empresas.TotalPaginas > 0 && _funcoes.Paginacao.Pagina > empresas.TotalPaginas)
        {
            _funcoes.Paginacao.Pagina = empresas.TotalPaginas;
            empresas = await _uof.Empresa.Listar(_funcoes.Paginacao);
        }

        Response.Headers.Append("x-Paginacao", empresas.JsonHeaderPaginacao());
        await _uof.Commit();
        return Ok(empresas);
    }

    /// <summary>
    /// Retorna a Lista de Campos utilizados na pesquisa e ordenação do GRID Empresa
    /// </summary>
    /// <returns>Lista de Campos</returns>
    [HttpGet, Route("Campos")]
    public async Task<ActionResult<Campo>> Campos()
    {
        await _uof.Commit();
        return Ok(await _uof.Campo.Campos("Empresa"));
    }

    /// <summary>
    /// Retorna a Lista de Empresas
    /// </summary>
    /// <returns>Lista solicitada</returns>
    [HttpGet, Route("ListarTodos")]
    public async Task<IActionResult> ListarTodos()
    {
        List<Empresa> empresas = await _uof.Empresa.Listar();

        return Ok(empresas);
    }

}
