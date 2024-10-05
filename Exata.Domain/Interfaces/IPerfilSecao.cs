using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface IPerfilSecao
{
    Task Inserir(PerfilSecao perfilSecao);
    Task Excluir(PerfilSecao perfilSecao);
    bool Existe(PerfilSecao perfilSecao);
}
