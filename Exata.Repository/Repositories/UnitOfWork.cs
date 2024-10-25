using Microsoft.AspNetCore.Http;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private ICliente _cliente;
    private IControllerAction _controllerAction;
    private ILogRequisicao _logRequisicao;
    private IPerfil _perfil;
    private IPerfilControllerAction _perfilControllerAction;
    
    public ApiContext _ctx;
    public IHttpContextAccessor _httpContext;
    public ICampo _campo;
    public IUsuario _usuario;

    public UnitOfWork(ApiContext ctx,
                      IHttpContextAccessor httpContextAccessor,
                      ICampo campo,
                      IUsuario usuario)
    {
        _ctx = ctx;
        _httpContext = httpContextAccessor;
        _campo = campo;
        _usuario = usuario;
    }

    public ICampo Campo 
    {
        get
        {
            return _campo = _campo ?? new CampoRepository(_ctx);
        }
    }

    public ICliente Cliente
    {
        get
        {
            return _cliente = _cliente ?? new ClienteRepository(_ctx, _campo, _usuario);
        }
    }

    public IControllerAction ControllerAction
    {
        get
        {
            return _controllerAction = _controllerAction ?? new ControllerActionRepository(_ctx);
        }
    }

    public ILogRequisicao LogRequisicao
    {
        get
        {
            return _logRequisicao = _logRequisicao ?? new LogRequisicoesRepository(_ctx);
        }
    }

    public IPerfil Perfil
    {
        get
        {
            return _perfil = _perfil ?? new PerfilRepository(_ctx, _campo, _usuario);
        }
    }

    public IPerfilControllerAction PerfilControllerAction
    {
        get
        {
            return _perfilControllerAction = _perfilControllerAction ?? new PerfilControllerActionRepository(_ctx);
        }
    }

    public IUsuario Usuario
    {
        get
        {
            return _usuario = _usuario ?? new UsuarioRepository(_ctx, _httpContext, _campo);
        }
    }

    public async Task Commit()
    {
        await _ctx.SaveChangesAsync();
    }

    public void Dispose()
    {
        _ctx.Dispose();
    }

}
