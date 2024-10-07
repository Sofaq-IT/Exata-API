using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Paginacao;

namespace Exata.Domain.Interfaces;

public interface IPerfil
{
    Task<Perfil> Inserir(Perfil perfil);
    Task<Perfil> Atualizar(Perfil perfil);
    Task<Perfil> Excluir(int id);
    Task<Perfil> Abrir(int id);
    bool Existe(int id);
    Task<PagedList<Perfil>> Listar(PaginacaoDTO paginacao);
    Task<List<Perfil>> Listar();
}
