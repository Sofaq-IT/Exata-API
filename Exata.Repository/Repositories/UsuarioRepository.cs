using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class UsuarioRepository : IUsuario
{
    private readonly ApiContext _ctx;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ICampo _campo;

    public UsuarioRepository(ApiContext context,
                             IHttpContextAccessor httpContextAccessor,
                             ICampo campo)
    {
        _ctx = context;
        _httpContext = httpContextAccessor;
        _campo = campo;
    }

    public async Task<ApplicationUser> Abrir(string id)
    {
        return await _ctx.Users
            .AsNoTracking()
            .Include(x => x.Perfil)
            .Include(x => x.UsuarioAvatar)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<PagedList<ApplicationUser>> Listar(PaginacaoDTO paginacao, string idAdm)
    {
        string tabela = "Usuario";
        PagedList<ApplicationUser> usuarios = null;

        IQueryable<ApplicationUser> iUsuarios = _ctx.Users
            .AsNoTracking()
            .Include(x => x.Perfil)
            .Where(x => x.UserName != idAdm);

        if (paginacao.Ativos != null)
            iUsuarios = iUsuarios.Where(x => x.Ativo == paginacao.Ativos);
        
        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception($"Campo ({paginacao.PesquisarCampo}) não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "UserName":
                    iUsuarios = iUsuarios.Where(x => x.UserName.Contains(paginacao.PesquisarValor));
                    break;

                case "Nome":
                    iUsuarios = iUsuarios.Where(x => x.Nome.Contains(paginacao.PesquisarValor));
                    break;

                case "Email":
                    iUsuarios = iUsuarios.Where(x => x.Email.Contains(paginacao.PesquisarValor));
                    break;

                case "Perfil":
                    iUsuarios = iUsuarios.Where(x => x.Perfil.Descricao.Contains(paginacao.PesquisarValor));
                    break;

                default:
                    throw new Exception($"Campo ({paginacao.PesquisarCampo}) não pode ser pesquisado!");
            }
        }

        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception($"Campo ({paginacao.OrderCampo}) não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iUsuarios = iUsuarios.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iUsuarios = iUsuarios.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        usuarios = await PagedList<ApplicationUser>
           .ToPagedList(
                iUsuarios,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return usuarios;
    }

    public async Task<string> UserID()
    {
        string sUserName = _httpContext.HttpContext.User.Identity.Name;

        if (string.IsNullOrEmpty(sUserName))
            return null;

        return await _ctx.Users
            .AsNoTracking()
            .Where(x => x.UserName == sUserName)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> QtdeAtivos(string idAdm)
    {
        return await _ctx.Users
            .AsNoTracking()
            .CountAsync(x => x.Ativo == true &&
                        x.UserName != idAdm);
    }

    public async Task<UsuarioAvatar> InserirAvatar(UsuarioAvatar usuarioAvatar)
    {
        await _ctx.AddAsync(usuarioAvatar);
        return usuarioAvatar;
    }

    public UsuarioAvatar AlterarAvatar(UsuarioAvatar usuarioAvatar)
    {
        _ctx.Update(usuarioAvatar);
        return usuarioAvatar;
    }

    public bool ExisteAvatar(string id)
    {
        return _ctx.UsuarioAvatar.Any(x => x.UsuarioID == id);
    }

    public async Task<string> Avatar(string id)
    {
        return await _ctx.UsuarioAvatar
            .Where(x => x.UsuarioID == id)
            .Select(x => x.Avatar)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicationUser> BuscarPorEmail(string email)
    {
        return await _ctx.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
    }

    public async Task<ApplicationUser> VerificarCodigo(VerificacaoCodigoDTO verificacaoCodigo)
    {
        return await _ctx.Users.Where(x => x.Email == verificacaoCodigo.Email && x.CodigoVerificacaoEsqueciMinhaSenha == verificacaoCodigo.Codigo).SingleOrDefaultAsync();
    }
}
