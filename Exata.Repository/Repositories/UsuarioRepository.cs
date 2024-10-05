using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class UsuarioRepository : IUsuario
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;

    public UsuarioRepository(ApiContext context, ICampo campo)
    {
        _ctx = context;
        _campo = campo;
    }

    public async Task Inserir(Usuario usuario)
    {
        usuario.booNovo = true;
        _ctx.Add(usuario);
        await _ctx.SaveChangesAsync();
    }

    public async Task Atualizar(Usuario usuario)
    {
        usuario.booNovo = false;
        var update = _ctx.Entry(usuario);
        update.State = EntityState.Modified;
        update.Property("Senha").IsModified = false;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        await _ctx.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        Usuario usuario = await _ctx.Usuario.Where(x => x.UsuarioID == id).FirstOrDefaultAsync();
        _ctx.Usuario.Remove(usuario);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Usuario> Abrir(int id)
    {
        return await _ctx.Usuario
            .AsNoTracking()
            .Include("Perfil")
            .Where(x => x.UsuarioID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(int id)
    {
        return _ctx.Usuario.Any(x => x.UsuarioID == id);
    }

    public async Task AlterarSenha(Usuario usuario)
    {
        usuario.booNovo = false;
        var update = _ctx.Entry(usuario);
        update.State = EntityState.Unchanged;
        update.Property("Senha").IsModified = true;
        update.Property("DataAlteracao").IsModified = true;
        update.Property("UserAlteracao").IsModified = true;
        await _ctx.SaveChangesAsync();
    }

    public async Task AlterarSenhaADM(Usuario usuario)
    {
        usuario.booNovo = false;
        var update = _ctx.Entry(usuario);
        update.State = EntityState.Unchanged;
        update.Property("Senha").IsModified = true;
        update.Property("DataAlteracao").IsModified = true;
        update.Property("UserAlteracao").IsModified = true;
        await _ctx.SaveChangesAsync();
    }

    public async Task<PagedList<Usuario>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Usuario";
        PagedList<Usuario> usuarios = null;

        IQueryable<Usuario> iUsuarios = _ctx.Usuario
            .AsNoTracking()
            .Include(x => x.Perfil);

        if (paginacao.Ativos != null)
            iUsuarios = iUsuarios.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception("Campo não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "Login":
                    iUsuarios = iUsuarios.Where(x => x.Login.Contains(paginacao.PesquisarValor));
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
            }
        }
        
        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception("Campo não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iUsuarios = iUsuarios.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iUsuarios = iUsuarios.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        usuarios = await PagedList<Usuario>
           .ToPagedList(
                iUsuarios,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return usuarios;
    }
}
