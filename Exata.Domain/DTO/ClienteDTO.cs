namespace Exata.Domain.DTO
{
	public record ClienteDTO
	{
		public string RazaoSocial { get; set; }
		public string NomeFantasia { get; set; }
		public string Documento { get; set; }
		public string Telefone { get; set; }
		public string Cep { get; set; }
		public string Logradouro { get; set; }
		public string Numero { get; set; }
		public string Complemento { get; set; }
		public string Bairro { get; set; }
		public string Cidade { get; set; }
		public string Estado { get; set; }
		public string Email { get; set; }

		public ClienteDTO(string razaoSocial, string nomeFantasia, string documento, string telefone, string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string email)
		{
			RazaoSocial = razaoSocial;
			NomeFantasia = nomeFantasia;
			Documento = documento;
			Telefone = telefone;
			Cep = cep;
			Logradouro = logradouro;
			Numero = numero;
			Complemento = complemento;
			Bairro = bairro;
			Cidade = cidade;
			Estado = estado;
			Email = email;
		}


	}
}
