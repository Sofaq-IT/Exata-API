using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class EmpresaClienteRepository : IEmpresaCliente
{
    private readonly ApiContext _ctx;

    public EmpresaClienteRepository(ApiContext context)
    {
        _ctx = context;
    }

    public EmpresaCliente Inserir(EmpresaCliente empresaCliente)
    {
        var empresaClienteBD = _ctx.EmpresaCliente.Where(X => X.ClienteID == empresaCliente.ClienteID && X.EmpresaID == empresaCliente.EmpresaID).FirstOrDefault();
        
        if (empresaClienteBD == null)
            _ctx.Add(empresaCliente);

        return empresaCliente;
    }

    public EmpresaCliente Excluir(EmpresaCliente empresaCliente)
    {
        _ctx.Remove(empresaCliente);
        return empresaCliente;
    }
}
