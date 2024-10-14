using Exata.Domain.DTO;

namespace Exata.Helpers.Interfaces
{
    public interface IEmail
    {
        void Enviar(EmailDTO emailDTO);
    }
}
