namespace Exata.Domain.DTO
{
	public record PlanoAmostraDTO
	{
		public PlanoAmostraDTO(string plano, int numeroAmostras)
		{
			Plano = plano;
			NumeroAmostras = numeroAmostras;
		}

		public string Plano { get; set; }
		public int NumeroAmostras { get; set; } = 0;

	}
}
