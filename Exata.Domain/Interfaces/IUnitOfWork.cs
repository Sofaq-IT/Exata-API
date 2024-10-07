﻿namespace Exata.Domain.Interfaces;

public interface IUnitOfWork
{
    ICampo Campo { get; }

    IControllerAction ControllerAction { get; }

    IContratante Contratante { get; }

    ILogRequisicao LogRequisicao { get; }

    IPerfil Perfil { get; }

    IPerfilControllerAction PerfilControllerAction { get; }

    IUsuario Usuario { get; }

    Task Commit();
}
