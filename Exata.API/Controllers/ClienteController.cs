using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Helpers.Interfaces;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para manter dados do Cliente
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IErrorRequest _error;
    private readonly IFuncoes _funcoes;

    /// <summary>
    /// Controller utilizado para manter dados do Cliente
    /// </summary>
    /// <param name="uof"></param>
    /// <param name="erro"></param>
    /// <param name="funcoes"></param>
    public ClienteController(IUnitOfWork uof,
                            IErrorRequest erro,
                            IFuncoes funcoes)
    {
        _uof = uof;
        _error = erro;
        _error.Titulo = "Cliente";
        _funcoes = funcoes;
    }

    /// <summary>
    /// Inclui o Cliente
    /// </summary>
    /// <param name="cliente">Objeto Cliente</param>
    /// <returns>O objeto inserido</returns>
    [HttpPost]
    public async Task<ActionResult<Cliente>> Post([FromBody] Cliente cliente)
    {
        if (_uof.Cliente.Existe(cliente.ClienteID) == true)
            return BadRequest(_error.BadRequest("Cliente já cadastrado."));

        Cliente clienteNovo = await _uof.Cliente.Inserir(cliente);
        await _uof.Commit();
        return Ok(clienteNovo);
    }

    /// <summary>
    /// Alterar o Cliente
    /// </summary>
    /// <param name="c">Objeto Cliente</param>
    /// <returns>O objeto alterado.</returns>
    [HttpPut]
    public async Task<ActionResult<Cliente>> Put([FromBody] Cliente cliente)
    {
        if (_uof.Cliente.Existe(cliente.ClienteID) == false)
            return NotFound(_error.NotFound());

        Cliente clienteAlterado = await _uof.Cliente.Atualizar(cliente);
        await _uof.Commit();
        return Ok(clienteAlterado);
    }

    /// <summary>
    /// Excluir o Cliente
    /// </summary>
    /// <param name="id">Id do Cliente</param>
    /// <returns></returns>
    [HttpDelete()]
    public async Task<ActionResult> Delete(int id)
    {
        if (_uof.Cliente.Existe(id) == false)
            return NotFound(_error.NotFound());

        Cliente clienteExcluido = await _uof.Cliente.Excluir(id);
        await _uof.Commit();
        return Ok(clienteExcluido);
    }

    /// <summary>
    /// Retorna o Cliente
    /// </summary>
    /// <param name="id">ID do Cliente</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet]
    public async Task<ActionResult<Cliente>> Get(int id)
    {
        if (_uof.Cliente.Existe(id) == false)
            return NotFound(_error.NotFound());

        await _uof.Commit();
        return Ok(await _uof.Cliente.Abrir(id));
    }

    /// <summary>
    /// Retorna a Lista de Clientes
    /// </summary>
    /// <returns>Lista solicitada</returns>
    [HttpGet, Route("Listar")]
    public async Task<IActionResult> Listar()
    {
        if (!_funcoes.ValidacaoHeaderPaginacao(Request.Headers))
        {
            return BadRequest(_error.BadRequest(_funcoes.errors));
        }

        PagedList<Cliente> clientes = await _uof.Cliente.Listar(_funcoes.Paginacao);

        if (clientes.TotalPaginas == 0)
            return NotFound(_error.NotFound());

        if (_funcoes.Paginacao.Pagina > clientes.TotalPaginas)
        {
            _funcoes.Paginacao.Pagina = clientes.TotalPaginas;
            clientes = await _uof.Cliente.Listar(_funcoes.Paginacao);
        }

        Response.Headers.Append("x-Paginacao", clientes.JsonHeaderPaginacao());
        await _uof.Commit();
        return Ok(clientes);
    }

    /// <summary>
    /// Retorna a Lista de Campos utilizados na pesquisa e ordenação do GRID Cliente
    /// </summary>
    /// <returns>Lista de Campos</returns>
    [HttpGet, Route("Campos")]
    public async Task<ActionResult<Campo>> Campos()
    {
        await _uof.Commit();
        return Ok(await _uof.Campo.Campos("Cliente"));
    }

}
