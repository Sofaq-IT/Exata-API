namespace Exata.Domain.DTO
{
    public class EmailDTO
    {
        public string Assunto { get; set; }
        public string CorpoEmail { get; set; }
        public bool Html { get; set; }
        public string[] Destinatarios { get; set; }
    }
}
