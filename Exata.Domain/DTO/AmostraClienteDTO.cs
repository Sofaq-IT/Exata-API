namespace Exata.Domain.DTO
{
	public record AmostraClienteDTO
	{
		public AmostraClienteDTO(string nomeCliente, int numeroAmostras)
		{
			NomeCliente = nomeCliente;
			NumeroAmostras = numeroAmostras;
		}

		public string NomeCliente { get; set; }
		public int NumeroAmostras { get; set; } = 0;
	}
}
