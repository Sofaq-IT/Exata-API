using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Paginacao;

namespace Exata.Domain.Interfaces;

public interface IUsuario
{
    Task<ApplicationUser> Abrir(string id);

    Task<PagedList<ApplicationUser>> Listar(PaginacaoDTO paginacao, string idAdm);

    Task<string> UserID();

    Task<int> QtdeAtivos(string idAdm);

    Task<UsuarioAvatar> InserirAvatar(UsuarioAvatar usuarioAvatar);

    UsuarioAvatar AlterarAvatar(UsuarioAvatar usuarioAvatar);

    bool ExisteAvatar(string id);

    Task<string> Avatar(string id);

    Task<ApplicationUser> BuscarPorEmail(string email);

    Task<ApplicationUser> VerificarCodigo(VerificacaoCodigoDTO verificacaoCodigo);

    void EnviarEmailNovoUsuario(string nome, string login, string email, string senha);
}
