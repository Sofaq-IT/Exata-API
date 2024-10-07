using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface IContratante
{
    Task<Contratante> Inserir(Contratante contratante);

    Contratante Alterar(Contratante contratante);

    bool Existe();

    Task<Contratante> Abrir();

    Task<Contratante> InserirLogo(Contratante contratante);

    Contratante AlterarLogo(Contratante contratante);

    Task<string> Logo();
}
