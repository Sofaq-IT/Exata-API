using Exata.Domain.Entities;
using System.Data;

namespace Exata.Domain.Interfaces
{
    public interface IAmostraResultado
    {
        Task<AmostraResultado> Inserir(AmostraResultado amostraResultado);
        Task InserirTodos(DataTable dadosImportados, Guid amostraId);
    }
}
