using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Helpers;

namespace Exata.Domain.Interfaces;

public interface IPerfil
{
    Task Inserir(Perfil perfil);
    Task Atualizar(Perfil perfil);
    Task Excluir(int id);
    Task<Perfil> Abrir(int id);
    bool Existe(int id);
    Task<PagedList<Perfil>> Listar(PaginacaoDTO paginacao);
}
