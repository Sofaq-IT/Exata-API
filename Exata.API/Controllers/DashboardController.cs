using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Filters;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para popular dados Dashboard
/// </summary>
//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
	private readonly IUnitOfWork _uof;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IErrorRequest _error;
	private readonly IFuncoes _funcoes;

	public DashboardController(IUnitOfWork uof,
							IErrorRequest erro,
							IFuncoes funcoes)
	{
		_uof = uof;
		_error = erro;
		_error.Titulo = "Dashboard";
		_funcoes = funcoes;
	}

	[HttpPost]
	public async Task<ActionResult<DashboardDTO>> Dashboard([FromBody] DashboardFilter filterDashboard)
	{
		var dashboard = await _uof.Dashboard.GetDashboard(filterDashboard);

		await _uof.Commit();

		return Ok(dashboard);
	}

	[HttpPost, Route("GetRadar")]
	public async Task<ActionResult<List<RadarDTO>>> GetRadar([FromBody] DashboardFilter filterDashboard)
	{
		var radar = await _uof.Dashboard.GetRadar(filterDashboard);

		await _uof.Commit();

		return Ok(radar);
	}

	[HttpPost, Route("GetFazendas")]
	public async Task<ActionResult<List<FazendaDTO>>> GetFazendas([FromBody] DashboardFilter filterDashboard)
	{
		var fazendas = await _uof.Dashboard.GetFazendas(filterDashboard);

		await _uof.Commit();

		return Ok(fazendas);
	}

	[HttpPost, Route("GetTalhoes")]
	public async Task<ActionResult<List<TalhaoDTO>>> GetTalhoes([FromBody] DashboardFilter filterDashboard)
	{
		var talhoes = await _uof.Dashboard.GetTalhoes(filterDashboard);

		await _uof.Commit();

		return Ok(talhoes);
	}

	[HttpPost, Route("GetGlebas")]
	public async Task<ActionResult<List<GlebaDTO>>> GetGlebas([FromBody] DashboardFilter filterDashboard)
	{
		var glebas = await _uof.Dashboard.GetGlebas(filterDashboard);

		await _uof.Commit();

		return Ok(glebas);
	}

	[HttpPost, Route("GetPontos")]
	public async Task<ActionResult<List<PontoDTO>>> GetPontos([FromBody] DashboardFilter filterDashboard)
	{
		var pontos = await _uof.Dashboard.GetPontos(filterDashboard);

		await _uof.Commit();

		return Ok(pontos);
	}

	[HttpPost, Route("GetProfundidades")]
	public async Task<ActionResult<List<ProfundidadeDTO>>> GetProfundidades([FromBody] DashboardFilter filterDashboard)
	{
		var profundidades = await _uof.Dashboard.GetProfundidades(filterDashboard);

		await _uof.Commit();

		return Ok(profundidades);
	}

	[HttpPost, Route("GetAnos")]
	public async Task<ActionResult<List<AnoDTO>>> GetAnos([FromBody] DashboardFilter filterDashboard)
	{
		var anos = await _uof.Dashboard.GetAnos(filterDashboard);

		await _uof.Commit();

		return Ok(anos);
	}
}


