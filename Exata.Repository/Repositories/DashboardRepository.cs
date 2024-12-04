using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Filters;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Exata.Repository.Repositories;

public class DashboardRepository : IDashboard
{
	private readonly ApiContext _ctx;
	private readonly IHttpContextAccessor _httpContext;

	public DashboardRepository(ApiContext context,
							 IHttpContextAccessor httpContextAccessor)
	{
		_ctx = context;
		_httpContext = httpContextAccessor;
	}

	public async Task<ApplicationUser> Abrir(string id)
	{
		return await _ctx.Users
			.AsNoTracking()
			.Include(x => x.Perfil)
			.Include(x => x.UsuarioAvatar)
			.Where(x => x.Id == id)
			.FirstOrDefaultAsync();
	}

	public async Task<List<AnoDTO>> GetAnos(DashboardFilter filter)
	{
		var anos = await _ctx.AmostraResultado.Include("Amostra").Where(x => x.Amostra.ClienteId == filter.ClienteId).Select(y => y.DataCadastro.Year).Distinct().ToListAsync();

		if (anos.Any())
			return anos.Select(s => new AnoDTO() { Nome = s }).ToList();
		else
			return new List<AnoDTO>();
	}

	public Task<DashboardDTO> GetDashboard(DashboardFilter filter)
	{
		throw new NotImplementedException();
	}

	public async Task<List<FazendaDTO>> GetFazendas(DashboardFilter filter)
	{
		var fazendas = await _ctx.AmostraResultado.Include("Amostra").Where(x => x.Amostra.ClienteId == filter.ClienteId &&
																			x.TipoInformacao == "R" &&
																			!string.IsNullOrEmpty(x.Fazenda)).Select(y => y.Fazenda).Distinct().ToListAsync();

		if (fazendas.Any())
			return fazendas.Select(s => new FazendaDTO() { Nome = s }).ToList();
		else
			return new List<FazendaDTO>();
	}

	public async Task<List<GlebaDTO>> GetGlebas(DashboardFilter filter)
	{
		var glebas = await _ctx.AmostraResultado.Include("Amostra").Where(x => x.Amostra.ClienteId == filter.ClienteId &&
																	x.TipoInformacao == "R" &&
																	filter.Fazendas.Contains(x.Fazenda) &&
																	filter.Talhoes.Contains(x.Talhao) &&
																	!string.IsNullOrEmpty(x.Gleba)).Select(y => y.Gleba).Distinct().ToListAsync();
		if (glebas.Any())
			return glebas.Select(s => new GlebaDTO() { Nome = s }).ToList();
		else
			return new List<GlebaDTO>();
	}

	public async Task<List<PontoDTO>> GetPontos(DashboardFilter filter)
	{
		var pontos = await _ctx.AmostraResultado.Include("Amostra").Where(x => x.Amostra.ClienteId == filter.ClienteId &&
															x.TipoInformacao == "R" &&
															filter.Fazendas.Contains(x.Fazenda) &&
															filter.Talhoes.Contains(x.Talhao) &&
															filter.Glebas.Contains(x.Gleba) &&
															!string.IsNullOrEmpty(x.PontoColeta)).Select(y => y.PontoColeta).Distinct().ToListAsync();
		if (pontos.Any())
			return pontos.Select(s => new PontoDTO() { Nome = s }).ToList();
		else
			return new List<PontoDTO>();
	}

	public async Task<List<ProfundidadeDTO>> GetProfundidades(DashboardFilter filter)
	{
		var profundidades = await _ctx.AmostraResultado.Include("Amostra").Where(x => x.Amostra.ClienteId == filter.ClienteId &&
													x.TipoInformacao == "R" &&
													filter.Fazendas.Contains(x.Fazenda) &&
													filter.Talhoes.Contains(x.Talhao) &&
													filter.Glebas.Contains(x.Gleba) &&
													filter.Pontos.Contains(x.PontoColeta) &&
													!string.IsNullOrEmpty(x.Profundidade)).Select(y => y.Profundidade).Distinct().ToListAsync();
		if (profundidades.Any())
			return profundidades.Select(s => new ProfundidadeDTO() { Nome = s }).ToList();
		else
			return new List<ProfundidadeDTO>();
	}

	public async Task<List<TalhaoDTO>> GetTalhoes(DashboardFilter filter)
	{
		var talhoes = await _ctx.AmostraResultado.Include("Amostra").Where(x => x.Amostra.ClienteId == filter.ClienteId &&
																			x.TipoInformacao == "R" &&
																			filter.Fazendas.Contains(x.Fazenda) &&
																			!string.IsNullOrEmpty(x.Talhao)).Select(y => y.Talhao).Distinct().ToListAsync();
		if (talhoes.Any())
			return talhoes.Select(s => new TalhaoDTO() { Nome = s }).ToList();
		else
			return new List<TalhaoDTO>();
	}
}
