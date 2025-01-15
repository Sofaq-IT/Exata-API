using Exata.Domain.DTO;
using Exata.Domain.Filters;

namespace Exata.Domain.Interfaces
{
	public interface IRelatorio
	{
		Task<List<ClienteDTO>> GetInformacaoClientes(RelatorioFilter filter);
		Task<List<AmostraClienteDTO>> GetAmostraClientes(RelatorioFilter filter);
		Task<List<ClientePlanoDTO>> GetPlanoClientes(RelatorioFilter filter);
		Task<List<PlanoAmostraDTO>> GetAmostraPlanos(RelatorioFilter filter);
	}
}
