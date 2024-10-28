using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Paginacao;

namespace Exata.Domain.Interfaces
{
    public interface IEmpresa
    {
        Task<Empresa> Inserir(Empresa empresa);
        Task<Empresa> Atualizar(Empresa empresa);
        Task<Empresa> Excluir(int id);
        Task<Empresa> Abrir(int id);
        bool Existe(int id);
        bool Existe(string cpfCnpj);
        Task<PagedList<Empresa>> Listar(PaginacaoDTO paginacao);
        Task<List<Empresa>> Listar();
    }
}
