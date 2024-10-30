using Exata.Domain.Entities;

namespace Exata.Domain.Interfaces;

public interface IEmpresaCliente
{
    EmpresaCliente Inserir(EmpresaCliente empresaCliente);
    EmpresaCliente Excluir(EmpresaCliente empresaCliente);
}
