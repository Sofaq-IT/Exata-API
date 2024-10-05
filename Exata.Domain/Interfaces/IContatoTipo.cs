using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface IContatoTipo
{
    Task Inserir(ContatoTipo contatoTipo);
    Task Excluir(ContatoTipo contatoTipo);
    bool Existe(ContatoTipo contatoTipo);
}
