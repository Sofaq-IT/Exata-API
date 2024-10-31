namespace Exata.Domain.Interfaces;

public interface IUnitOfWork
{
    ICampo Campo { get; }

    ICliente Cliente { get; }

    IControllerAction ControllerAction { get; }

    IEmpresa Empresa { get; }

    IEmpresaCliente EmpresaCliente { get; }

    ILogRequisicao LogRequisicao { get; }

    IPerfil Perfil { get; }

    IPerfilControllerAction PerfilControllerAction { get; }

    IUpload Upload { get; }

    IUsuario Usuario { get; }

    Task Commit();
}
