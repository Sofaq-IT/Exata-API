using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;
using Exata.Repository.Context;

using Microsoft.AspNetCore.Http;

namespace Exata.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private IAmostra _amostra;
	private IAmostraResultado _amostraResultado;
	private IBlobStorage _blobStorage;
	private ICliente _cliente;
	private IControllerAction _controllerAction;
	private IEmpresa _empresa;
	private IEmpresaCliente _empresaCliente;
	private ILogRequisicao _logRequisicao;
	private IPerfil _perfil;
	private IPerfilControllerAction _perfilControllerAction;
	private IUpload _upload;
	private IEmail _email;
	private IDashboard _dashboard;
	private IRelatorio _relatorio;

	public ApiContext _ctx;
	public IHttpContextAccessor _httpContext;
	public ICampo _campo;
	public IUsuario _usuario;

	public UnitOfWork(ApiContext ctx,
					  IHttpContextAccessor httpContextAccessor,
					  ICampo campo,
					  IUsuario usuario,
					  IBlobStorage blobStorage)
	{
		_ctx = ctx;
		_httpContext = httpContextAccessor;
		_campo = campo;
		_usuario = usuario;
		_blobStorage = blobStorage;
	}

	public IAmostra Amostra
	{
		get
		{
			return _amostra = _amostra ?? new AmostraRepository(_ctx, _usuario, _blobStorage);
		}
	}

	public IAmostraResultado AmostraResultado
	{
		get
		{
			return _amostraResultado = _amostraResultado ?? new AmostraResultadoRepository(_ctx, _usuario);
		}
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

	public IEmpresa Empresa
	{
		get
		{
			return _empresa = _empresa ?? new EmpresaRepository(_ctx, _campo, _usuario);
		}
	}

	public IEmpresaCliente EmpresaCliente
	{
		get
		{
			return _empresaCliente = _empresaCliente ?? new EmpresaClienteRepository(_ctx);
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

	public IUpload Upload
	{
		get
		{
			return _upload = _upload ?? new UploadRepository(_ctx, _usuario);
		}
	}

	public IUsuario Usuario
	{
		get
		{
			return _usuario = _usuario ?? new UsuarioRepository(_ctx, _httpContext, _campo, _email);
		}
	}

	public IDashboard Dashboard
	{
		get
		{
			return _dashboard = _dashboard ?? new DashboardRepository(_ctx, _httpContext);
		}
	}

	public IRelatorio Relatorio
	{
		get
		{
			return _relatorio = _relatorio ?? new RelatorioRepository(_ctx, _httpContext);
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
