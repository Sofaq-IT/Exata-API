using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Filters;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Exata.Repository.Repositories;

public class RelatorioRepository : IRelatorio
{
	private readonly ApiContext _ctx;
	private readonly IHttpContextAccessor _httpContext;

	public RelatorioRepository(ApiContext context,
							 IHttpContextAccessor httpContextAccessor)
	{
		_ctx = context;
		_httpContext = httpContextAccessor;
	}

	public async Task<List<AmostraClienteDTO>> GetAmostraClientes(RelatorioFilter filter)
	{
		List<Cliente> clientes = await GetClientes(filter);

		var amostraClientes = new List<AmostraClienteDTO>();

		foreach (var c in clientes)
		{
			var numeroAmostra = _ctx.Amostra.Where(x => x.ClienteId == c.ClienteID).Count();
			var amostraCliente = new AmostraClienteDTO(c.NomeRazaoSocial, numeroAmostra);
			amostraClientes.Add(amostraCliente);
		}

		return amostraClientes;
	}

	public async Task<List<PlanoAmostraDTO>> GetAmostraPlanos(RelatorioFilter filter)
	{
		List<Cliente> clientes = await GetClientes(filter);
		var clientesBasicos = clientes.Where(x => x.Plano == Domain.Enums.PlanoEnum.Basico).Select(x => x.ClienteID).ToList();
		var clientesIntermediarios = clientes.Where(x => x.Plano == Domain.Enums.PlanoEnum.Intermediario).Select(x => x.ClienteID).ToList();
		var clientesAvancados = clientes.Where(x => x.Plano == Domain.Enums.PlanoEnum.Avancado).Select(x => x.ClienteID).ToList();

		var countBasico = _ctx.Amostra.Where(x => clientesBasicos.Contains(x.ClienteId)).Count();
		var countIntermediario = _ctx.Amostra.Where(x => clientesIntermediarios.Contains(x.ClienteId)).Count();
		var countAvancado = _ctx.Amostra.Where(x => clientesAvancados.Contains(x.ClienteId)).Count();

		var planoAmostras = new List<PlanoAmostraDTO>();

		planoAmostras.Add(new PlanoAmostraDTO("Basico", countBasico));
		planoAmostras.Add(new PlanoAmostraDTO("Intermediario", countIntermediario));
		planoAmostras.Add(new PlanoAmostraDTO("Avancado", countAvancado));

		return planoAmostras;
	}

	public async Task<List<ClienteDTO>> GetInformacaoClientes(RelatorioFilter filter)
	{
		List<Cliente> clientes = await GetClientes(filter);

		var informacaoClientes = new List<ClienteDTO>();

		foreach (var c in clientes)
		{
			var clienteDto = new ClienteDTO(
				razaoSocial: c.NomeRazaoSocial,
				nomeFantasia: c.ApelidoNomeFantasia,
				documento: c.CpfCnpj,
				telefone: c.Telefone,
				cep: c.Cep,
				logradouro: c.Logradouro,
				numero: c.Numero,
				complemento: c.Complemento,
				bairro: c.Bairro,
				cidade: c.Cidade,
				estado: c.Estado,
				email: c.Email);

			informacaoClientes.Add(clienteDto);
		}

		return informacaoClientes;
	}


	public async Task<List<ClientePlanoDTO>> GetPlanoClientes(RelatorioFilter filter)
	{
		List<Cliente> clientes = await GetClientes(filter);
		var clientePlanos = new List<ClientePlanoDTO>();

		foreach (var c in clientes)
		{
			clientePlanos.Add(new ClientePlanoDTO(c.NomeRazaoSocial, c.Plano.ToString()));
		}

		return clientePlanos;
	}

	private async Task<List<Cliente>> GetClientes(RelatorioFilter filter)
	{
		var empresaClientes = await _ctx.EmpresaCliente.Where(x => x.EmpresaID == filter.EmpresaId).Select(x => x.ClienteID).ToListAsync();
		var clientesQuery = _ctx.Cliente.Where(x => empresaClientes.Contains(x.ClienteID)).AsQueryable();

		if (filter.DataInicio != null)
			clientesQuery = clientesQuery.Where(x => x.DataCadastro >= filter.DataInicio);
		if (filter.DataFim != null)
			clientesQuery = clientesQuery.Where(x => x.DataCadastro <= filter.DataFim);

		var clientes = await clientesQuery.ToListAsync();
		return clientes;
	}
}
