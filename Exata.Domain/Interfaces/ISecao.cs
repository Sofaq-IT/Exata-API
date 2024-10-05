using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Helpers;

namespace Exata.Domain.Interfaces;

public interface ISecao
{
    Task Inserir(Secao secao);
    Task Atualizar(Secao secao);
    Task Excluir(int id);
    Task<Secao> Abrir(int id);
    bool Existe(int id);
    Task<PagedList<Secao>> Listar(PaginacaoDTO paginacao);
    Task<List<Secao>> Listar();
}
