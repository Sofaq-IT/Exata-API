namespace Exata.Domain.Filters;

public class DashboardFilter
{
	public int ClienteId { get; set; } = 0;
	public string Elemento { get; set; }
	public List<string> Fazendas { get; set; }
	public List<string> Talhoes { get; set; }
	public List<string> Glebas { get; set; }
	public List<string> Profundidades { get; set; }
	public List<string> Pontos { get; set; }
	public string Ano { get; set; }
}
