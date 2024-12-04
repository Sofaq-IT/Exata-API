namespace Exata.Domain.DTO
{
	public class FazendaDTO
	{
		public string Nome { get; set; }
		public List<TalhaoDTO> Talhoes { get; set; } = new List<TalhaoDTO>();

	}
}
