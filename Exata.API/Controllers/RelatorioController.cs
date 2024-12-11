using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Filters;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para popular dados Relatorio
/// </summary>
//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RelatorioController : ControllerBase
{
	private readonly IUnitOfWork _uof;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IErrorRequest _error;
	private readonly IFuncoes _funcoes;

	public RelatorioController(IUnitOfWork uof,
							IErrorRequest erro,
							IFuncoes funcoes)
	{
		_uof = uof;
		_error = erro;
		_error.Titulo = "Relatorio";
		_funcoes = funcoes;
	}

	[HttpPost, Route("GetInformacaoClientes")]
	public async Task<ActionResult<List<ClienteDTO>>> GetInformacaoClientes([FromBody] RelatorioFilter filterDashboard)
	{
		var dashboard = await _uof.Relatorio.GetInformacaoClientes(filterDashboard);

		await _uof.Commit();

		return Ok(dashboard);
	}

	[HttpPost, Route("GetAmostraClientes")]
	public async Task<ActionResult<List<AmostraClienteDTO>>> GetAmostraClientes([FromBody] RelatorioFilter filterDashboard)
	{
		var dashboard = await _uof.Relatorio.GetAmostraClientes(filterDashboard);

		await _uof.Commit();

		return Ok(dashboard);
	}

	[HttpPost, Route("GetPlanoClientes")]
	public async Task<ActionResult<List<ClientePlanoDTO>>> GetPlanoClientes([FromBody] RelatorioFilter filterDashboard)
	{
		var dashboard = await _uof.Relatorio.GetPlanoClientes(filterDashboard);

		await _uof.Commit();

		return Ok(dashboard);
	}

	[HttpPost, Route("GetAmostraPlanos")]
	public async Task<ActionResult<List<PlanoAmostraDTO>>> GetAmostraPlanos([FromBody] RelatorioFilter filterDashboard)
	{
		var dashboard = await _uof.Relatorio.GetAmostraPlanos(filterDashboard);

		await _uof.Commit();

		return Ok(dashboard);
	}

}


