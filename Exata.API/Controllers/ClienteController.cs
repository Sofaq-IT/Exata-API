using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;

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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IErrorRequest _error;
    private readonly IFuncoes _funcoes;
    private readonly IUsuario _usuario;

    /// <summary>
    /// Controller utilizado para manter dados do Cliente
    /// </summary>
    /// <param name="uof"></param>
    /// <param name="userManager"></param>
    /// <param name="erro"></param>
    /// <param name="funcoes"></param>
    /// <param name="usuario"></param>
    public ClienteController(IUnitOfWork uof,
                            UserManager<ApplicationUser> userManager,
                            IErrorRequest erro,
                            IFuncoes funcoes,
                            IUsuario usuario)
    {
        _uof = uof;
        _userManager = userManager;
        _error = erro;
        _error.Titulo = "Cliente";
        _funcoes = funcoes;
        _usuario = usuario;

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

        var userID = await _usuario.UserID();

        if (userID == null)
            return BadRequest(_error.BadRequest("Usuário não está autenticado."));

        var user = await _uof.Usuario.Abrir(userID);

        Cliente clienteNovo = await _uof.Cliente.Inserir(cliente);

        await _uof.Commit();

        _uof.EmpresaCliente.Inserir(new EmpresaCliente { ClienteID = clienteNovo.ClienteID, EmpresaID = Convert.ToInt32(user.EmpresaID) });

        await _uof.Commit();

        ApplicationUser userCliente = new()
        {
            Email = clienteNovo.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = clienteNovo.Email,
            PerfilID = 3, // 3 - Cliente
            Nome = clienteNovo.ApelidoNomeFantasia,
            Ativo = true,
            booNovo = true,
            PhoneNumber = clienteNovo.Telefone,
            ClienteID = clienteNovo.ClienteID
        };

        var result = await _userManager.CreateAsync(userCliente, Helpers.Funcoes.GenerateRandomString(10));

        if (!result.Succeeded)
        {
            return BadRequest(_error.BadRequest(result));
        }

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
        var userID = await _usuario.UserID();

        if (userID == null)
            return BadRequest(_error.BadRequest("Usuário não está autenticado."));

        var user = await _uof.Usuario.Abrir(userID);

        if (user.PerfilID == 2) // Empresa/Laboratório
        {
            _uof.EmpresaCliente.Inserir(new EmpresaCliente { ClienteID = cliente.ClienteID, EmpresaID = Convert.ToInt32(user.EmpresaID) });

            await _uof.Commit();

            return Ok(cliente);
        }

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

        var userID = await _usuario.UserID();

        if (userID == null)
            return BadRequest(_error.BadRequest("Usuário não está autenticado."));

        var user = await _uof.Usuario.Abrir(userID);

        if (user.PerfilID == 2) // Empresa/Laboratório
        {
            _uof.EmpresaCliente.Excluir(new EmpresaCliente { ClienteID = id, EmpresaID = Convert.ToInt32(user.EmpresaID) });

            await _uof.Commit();

            return Ok();
        }

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
    /// Retorna o Cliente de acordo com CPF/CNPJ informado
    /// </summary>
    /// <param name="cpfCnpj">CPF/CNPJ do Cliente</param>
    /// <returns>O objeto solicitado</returns>
    [HttpGet, Route("Buscar/{cpfCnpj}")]
    public async Task<ActionResult<Cliente>> GetByCpfCnpj(string cpfCnpj)
    {
        return Ok(await _uof.Cliente.BuscarPorCpfCnpj(cpfCnpj));
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

        if (clientes.TotalPaginas > 0 && _funcoes.Paginacao.Pagina > clientes.TotalPaginas)
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
