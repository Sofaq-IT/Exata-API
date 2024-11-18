using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces
{
    public interface IAmostra
    {
        Task<Amostra> Inserir(Amostra amostra);
    }
}
