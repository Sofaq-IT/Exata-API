using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Helpers;

namespace Exata.Domain.Interfaces;

public interface ITipoContato
{
    Task Inserir(TipoContato tipoContato);
    Task Atualizar(TipoContato tipoContato);
    Task Excluir(int id);
    Task<TipoContato> Abrir(int id);
    bool Existe(int id);
    Task<PagedList<TipoContato>> Listar(PaginacaoDTO paginacao);
    Task<List<TipoContato>> Listar();
}
