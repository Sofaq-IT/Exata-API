namespace Exata.Domain.DTO
{
    public class SmtpSettingsDTO
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}
