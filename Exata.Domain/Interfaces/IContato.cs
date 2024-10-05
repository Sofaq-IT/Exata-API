using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Helpers;

namespace Exata.Domain.Interfaces;

public interface IContato
{
    Task Inserir(Contato contato);
    Task Atualizar(Contato contato);
    Task Excluir(string id);
    Task<Contato> Abrir(string id);
    bool Existe(string id);
    Task<PagedList<Contato>> Listar(PaginacaoDTO paginacao);
}
