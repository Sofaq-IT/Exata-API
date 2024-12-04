namespace Exata.Domain.DTO
{
	public class PontoDTO
	{
		public string Nome { get; set; }
		public List<ProfundidadeDTO> Profundidades { get; set; } = new List<ProfundidadeDTO>();
	}
}
