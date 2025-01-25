using Exata.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Exata.Domain.Interfaces
{
    public interface IAmostra
    {
        Task<Amostra> Inserir(Amostra amostra);
        Task SalvarAnexos(IFormFile[] attachments, Guid amostraId, DateTime dataReferencia);
        Task<List<Upload>> ListarAnexos(Guid amostraId);
        Task Excluir(Guid amostraId);
        Task<Amostra> Abrir(Guid amostraId);
        Task ExcluirAnexos(Guid amostraId);
    }
}
