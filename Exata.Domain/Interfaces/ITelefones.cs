using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Helpers;

namespace Exata.Domain.Interfaces;

public interface ITelefones
{
    Task Inserir(Telefones telefone);
    Task Atualizar(Telefones telefone);
    Task Excluir(int id);
    Task<Telefones> Abrir(int id);
    bool Existe(string numero);
    bool Existe(int id);
    Task<PagedList<Telefones>> Listar(PaginacaoDTO paginacao);
}

