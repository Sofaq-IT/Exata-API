using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Paginacao;

namespace Exata.Domain.Interfaces
{
    public interface ICliente
    {
        Task<Cliente> Inserir(Cliente cliente);
        Task<Cliente> Atualizar(Cliente cliente);
        Task<Cliente> Excluir(int id);
        Task<Cliente> Abrir(int id);
        bool Existe(int id);
        Task<PagedList<Cliente>> Listar(PaginacaoDTO paginacao);
        Task<List<Cliente>> Listar();
    }
}
