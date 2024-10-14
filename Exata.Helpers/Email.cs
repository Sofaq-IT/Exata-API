using Exata.Domain.DTO;
using Exata.Helpers.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Exata.Helpers
{
    public class Email : IEmail
    {
        private readonly SmtpSettingsDTO _smtpSettings;

        public Email(IOptions<SmtpSettingsDTO> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public void Enviar(EmailDTO emailDTO)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.User),
                    Subject = emailDTO.Assunto,
                    Body = emailDTO.CorpoEmail,
                    IsBodyHtml = emailDTO.Html
                };

                foreach (var email in emailDTO.Destinatarios)
                {
                    mail.To.Add(new MailAddress(email));
                }

                using (SmtpClient smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpSettings.User, _smtpSettings.Password);
                    smtpClient.EnableSsl = _smtpSettings.EnableSsl;

                    smtpClient.Send(mail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
