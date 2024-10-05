using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Helpers;

namespace Exata.Domain.Interfaces;

public interface IUsuario
{
    Task Inserir(Usuario usuario);
    Task Atualizar(Usuario usuario);
    Task Excluir(int id);
    Task<Usuario> Abrir(int id);
    bool Existe(int id);
    Task AlterarSenha(Usuario usuario);
    Task AlterarSenhaADM(Usuario usuario);
    Task<PagedList<Usuario>> Listar(PaginacaoDTO paginacao);
}
