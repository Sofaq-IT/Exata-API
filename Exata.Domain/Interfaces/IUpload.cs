using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Paginacao;

namespace Exata.Domain.Interfaces
{
    public interface IUpload
    {
        Task<Upload> Abrir(int id);
        Task<Upload> Inserir(Upload upload);
        Task<Upload> Atualizar(Upload upload);
        Task<PagedList<Upload>> Listar(PaginacaoDTO paginacao);
        Task<List<Upload>> Listar();
    }
}
