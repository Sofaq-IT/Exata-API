using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Helpers.Interfaces;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para manter dados do Perfil
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class PerfilController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IErrorRequest _error;
    private readonly IFuncoes _funcoes;

    /// <summary>
    /// Controller utilizado para manter dados do Perfil
    /// </summary>
    /// <param name="uof"></param>
    /// <param name="erro"></param>
    /// <param name="funcoes"></param>
    public PerfilController(IUnitOfWork uof,
                            IErrorRequest erro,
                            IFuncoes funcoes)
    {
        _uof = uof;
        _error = erro;
        _error.Titulo = "Perfil";
        _funcoes = funcoes;
    }

    /// <summary>
    /// Inclui o Perfil que será vinculado ao usuário
    /// </summary>
    /// <param name="perfil">Objeto Perfil</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Perfil>> Post([FromBody] Perfil perfil)
    {
        if (_uof.Perfil.Existe(perfil.PerfilID) == true)
            return BadRequest(_error.BadRequest("Perfil já cadastrado."));

        Perfil perfilNovo = await _uof.Perfil.Inserir(perfil);
        await _uof.Commit();
        return Ok(perfilNovo);
    }

    /// <summary>
    /// Alterar o Perfil
    /// </summary>
    /// <param name="perfil">Objeto Perfil</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Perfil>> Put([FromBody] Perfil perfil)
    {
        if (_uof.Perfil.Existe(perfil.PerfilID) == false)
            return NotFound(_error.NotFound());

        Perfil perfilAlterado = await _uof.Perfil.Atualizar(perfil);
        await _uof.Commit();
        return Ok(perfilAlterado);
    }

    /// <summary>
    /// Excluir o Perfil
    /// </summary>
    /// <param name="id">Id do Perfil</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        if (_uof.Perfil.Existe(id) == false)
            return NotFound(_error.NotFound());

        Perfil perfilExcluido = await _uof.Perfil.Excluir(id);
        await _uof.Commit();
        return Ok(perfilExcluido);
    }

    /// <summary>
    /// Retorna o Perfil
    /// </summary>
    /// <param name="id">ID do Perfil</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Perfil>> Get(int id)
    {
        if (_uof.Perfil.Existe(id) == false)
            return NotFound(_error.NotFound());

        await _uof.Commit();
        return Ok(await _uof.Perfil.Abrir(id));
    }

    /// <summary>
    /// Retorna a Lista de Perfis
    /// </summary>
    /// <returns>Lista solicitada</returns>
    [HttpGet, Route("Listar")]
    public async Task<IActionResult> Listar()
    {
        if (!_funcoes.ValidacaoHeaderPaginacao(Request.Headers))
        {
            return BadRequest(_error.BadRequest(_funcoes.errors));
        }

        PagedList<Perfil> perfis = await _uof.Perfil.Listar(_funcoes.Paginacao);

        if (perfis.TotalPaginas == 0)
            return NotFound(_error.NotFound());

        if (_funcoes.Paginacao.Pagina > perfis.TotalPaginas)
        {
            _funcoes.Paginacao.Pagina = perfis.TotalPaginas;
            perfis = await _uof.Perfil.Listar(_funcoes.Paginacao);
        }

        Response.Headers.Append("x-Paginacao", perfis.JsonHeaderPaginacao());
        await _uof.Commit();
        return Ok(perfis);
    }

    /// <summary>
    /// Retorna a Lista de Campos utilizados na pesquisa e ordenação do GRID Perfil
    /// </summary>
    /// <returns>Lista de Campos</returns>
    [HttpGet, Route("Campos")]
    public async Task<ActionResult<Campo>> Campos()
    {
        await _uof.Commit();
        return Ok(await _uof.Campo.Campos("Perfil"));
    }

    /// <summary>
    /// Retorna os Controllers/Actions do sistema
    /// </summary>
    /// <returns>O objeto solicitado</returns>
    [HttpGet, Route("ControllerAction")]
    public async Task<ActionResult<ControllerAction>> ControllerAction()
    {
        List<ControllerAction> controllerAction = await _uof.ControllerAction.Listar();
        await _uof.Commit();
        return Ok(controllerAction.OrderBy(x => x.DescricaoJson).ToList());
    }

    /// <summary>
    /// Vincula o Controller/Action ao Perfil
    /// </summary>
    /// <param name="perfilControllerAction">Objeto Perfil Controller/Action</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost, Route("ControllerAction")]
    public async Task<ActionResult<PerfilControllerAction>> PerfilControllerAction([FromBody] PerfilControllerAction perfilControllerAction)
    {
        if (!_uof.Perfil.Existe(perfilControllerAction.PerfilID))
            return NotFound(_error.NotFound("Perfil não encontrado."));

        if (!_uof.ControllerAction.Existe(perfilControllerAction.ControllerActionID))
            return BadRequest(_error.BadRequest("Controller/Action não encontrado."));

        if (_uof.PerfilControllerAction.Existe(perfilControllerAction))
            return BadRequest(_error.BadRequest("Controller/Action já vinculado ao Perfil."));

        PerfilControllerAction perfilControllerActionNovo = _uof.PerfilControllerAction.Inserir(perfilControllerAction);
        await _uof.Commit();
        return Ok(perfilControllerActionNovo);
    }

    /// <summary>
    /// Excluir o vinculo entre o Perfil e o Controller/Action
    /// </summary>
    /// <param name="perfilControllerAction">Objeto Perfil Controller/Action</param>
    /// <returns>O objeto excluído</returns>
    [HttpDelete, Route("ControllerAction")]
    public async Task<ActionResult<PerfilControllerAction>> DeletePerfilControllerAction([FromBody] PerfilControllerAction perfilControllerAction)
    {
        if (!_uof.Perfil.Existe(perfilControllerAction.PerfilID))
            return NotFound(_error.NotFound("Perfil não encontrado."));

        if (!_uof.ControllerAction.Existe(perfilControllerAction.ControllerActionID))
            return BadRequest(_error.BadRequest("Controller/Action não encontrado."));

        if (!_uof.PerfilControllerAction.Existe(perfilControllerAction))
            return BadRequest(_error.BadRequest("Controller/Action não está vinculada ao Perfil."));

        PerfilControllerAction perfilControllerActionNovo = _uof.PerfilControllerAction.Excluir(perfilControllerAction);
        await _uof.Commit();
        return Ok(perfilControllerActionNovo);
    }
}
