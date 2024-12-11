namespace Exata.Domain.DTO
{
	public record ClientePlanoDTO
	{
		public ClientePlanoDTO(string nomeCliente, string plano)
		{
			NomeCliente = nomeCliente;
			Plano = plano;
		}

		public string NomeCliente { get; set; }
		public string Plano { get; set; }

	}
}
