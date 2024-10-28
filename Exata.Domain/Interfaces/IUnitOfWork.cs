namespace Exata.Domain.Interfaces;

public interface IUnitOfWork
{
    ICampo Campo { get; }

    ICliente Cliente { get; }

    IControllerAction ControllerAction { get; }

    IEmpresa Empresa { get; }

    ILogRequisicao LogRequisicao { get; }

    IPerfil Perfil { get; }

    IPerfilControllerAction PerfilControllerAction { get; }

    IUsuario Usuario { get; }

    Task Commit();
}
