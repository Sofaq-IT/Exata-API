namespace Exata.Domain.DTO
{
	public class TalhaoDTO
	{
		public string Nome { get; set; }
		public List<GlebaDTO> Glebas { get; set; } = new List<GlebaDTO>();
	}
}
