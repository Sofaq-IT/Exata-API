using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Repository.Context;
using System.Linq;

namespace Exata.Repository.Repositories;

public class ClienteRepository : ICliente
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;
    private readonly IUsuario _usuario;

    public ClienteRepository(ApiContext context, ICampo campo, IUsuario usuario)
    {
        _ctx = context;
        _campo = campo;
        _usuario = usuario;
    }

    public async Task<Cliente> Inserir(Cliente cliente)
    {
        cliente.UserCadastro = await _usuario.UserID();
        cliente.booNovo = true;
        _ctx.Add(cliente);
        return cliente;
    }

    public async Task<Cliente> Atualizar(Cliente cliente)
    {
        cliente.UserAlteracao = await _usuario.UserID();
        cliente.booNovo = false;
        var update = _ctx.Entry(cliente);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        return cliente;
    }

    public async Task<Cliente> Excluir(int id)
    {
        Cliente cliente = await _ctx.Cliente.Where(x => x.ClienteID == id).FirstOrDefaultAsync();
        _ctx.Cliente.Remove(cliente);
        return cliente;
    }

    public async Task<Cliente> Abrir(int id)
    {
        return await _ctx.Cliente
            .AsNoTracking()
            .Where(x => x.ClienteID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(int id)
    {
        return _ctx.Cliente.Any(x => x.ClienteID == id);
    }

    public async Task<PagedList<Cliente>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Cliente";
        PagedList<Cliente> clientes = null;

        IQueryable<Cliente> iClientes = _ctx.Cliente.AsNoTracking();

        var userID = await _usuario.UserID();

        var user = _ctx.Users.Where(x => x.Id == userID).FirstOrDefault();

        if (user != null && user.EmpresaID != null)
            iClientes = from c in iClientes
                        join ec in _ctx.EmpresaCliente on c.ClienteID equals ec.ClienteID
                        where ec.EmpresaID == user.EmpresaID
                        select c;

        if (paginacao.Ativos != null)
            iClientes = iClientes.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception($"Campo ({paginacao.PesquisarCampo}) não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "CpfCnpj":
                    iClientes = iClientes.Where(x => x.CpfCnpj.Contains(paginacao.PesquisarValor));
                    break;

                case "NomeRazaoSocial":
                    iClientes = iClientes.Where(x => x.NomeRazaoSocial.Contains(paginacao.PesquisarValor));
                    break;

                case "ApelidoNomeFantasia":
                    iClientes = iClientes.Where(x => x.ApelidoNomeFantasia.Contains(paginacao.PesquisarValor));
                    break;

                default:
                    throw new Exception($"Campo ({paginacao.PesquisarCampo}) não pode ser pesquisado!");
            }
        }

        if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        {
            if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
                throw new Exception($"Campo ({paginacao.OrderCampo}) não pode ser Ordenado!");

            if (paginacao.OrderTipoAsc == true)
                iClientes = iClientes.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iClientes = iClientes.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        clientes = await PagedList<Cliente>
           .ToPagedList(
                iClientes,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return clientes;
    }

    public async Task<List<Cliente>> Listar()
    {
        IQueryable<Cliente> iClientes = _ctx.Cliente.AsNoTracking();

        var userID = await _usuario.UserID();

        var user = _ctx.Users.Where(x => x.Id == userID).FirstOrDefault();

        if (user != null && user.EmpresaID != null)
            iClientes = from c in iClientes
                        join ec in _ctx.EmpresaCliente on c.ClienteID equals ec.ClienteID
                        where ec.EmpresaID == user.EmpresaID
                        select c;

        return await iClientes
            .Where(x => x.Ativo == true)
            .OrderBy(x => x.NomeRazaoSocial)
            .ToListAsync();
    }

    public async Task<Cliente> BuscarPorCpfCnpj(string cpfCnpj)
    {
        return await _ctx.Cliente
            .AsNoTracking()
            .Where(x => x.CpfCnpj == cpfCnpj)
            .FirstOrDefaultAsync();
    }
}