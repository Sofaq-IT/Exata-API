namespace Exata.Domain.DTO
{
	public class GlebaDTO
	{
		public string Nome { get; set; }
		public List<PontoDTO> Pontos { get; set; } = new List<PontoDTO>();
	}
}
