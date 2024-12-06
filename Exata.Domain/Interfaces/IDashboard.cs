using Exata.Domain.DTO;
using Exata.Domain.Filters;

namespace Exata.Domain.Interfaces
{
	public interface IDashboard
	{
		Task<DashboardDTO> GetDashboard(DashboardFilter filter);
		Task<RadarDTO> GetRadar(DashboardFilter filter);
		Task<List<FazendaDTO>> GetFazendas(DashboardFilter filter);
		Task<List<TalhaoDTO>> GetTalhoes(DashboardFilter filter);
		Task<List<GlebaDTO>> GetGlebas(DashboardFilter filter);
		Task<List<PontoDTO>> GetPontos(DashboardFilter filter);
		Task<List<ProfundidadeDTO>> GetProfundidades(DashboardFilter filter);
		Task<List<AnoDTO>> GetAnos(DashboardFilter filter);
	}
}
