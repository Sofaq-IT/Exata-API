namespace Exata.Domain.DTO
{
	public class ProfundidadeDTO
	{
		public string Nome { get; set; }
		public List<AnoDTO> Anos { get; set; } = new List<AnoDTO>();
	}
}
