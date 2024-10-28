using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Repository.Context;

namespace Exata.Repository.Repositories;

public class EmpresaRepository : IEmpresa
{
    private readonly ApiContext _ctx;
    private readonly ICampo _campo;
    private readonly IUsuario _usuario;

    public EmpresaRepository(ApiContext context, ICampo campo, IUsuario usuario)
    {
        _ctx = context;
        _campo = campo;
        _usuario = usuario;
    }

    public async Task<Empresa> Inserir(Empresa empresa)
    {
        empresa.UserCadastro = await _usuario.UserID();
        empresa.booNovo = true;
        _ctx.Add(empresa);
        return empresa;
    }

    public async Task<Empresa> Atualizar(Empresa empresa)
    {
        empresa.UserAlteracao = await _usuario.UserID();
        empresa.booNovo = false;
        var update = _ctx.Entry(empresa);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        return empresa;
    }

    public async Task<Empresa> Excluir(int id)
    {
        Empresa empresa = await _ctx.Empresa.Where(x => x.EmpresaID == id).FirstOrDefaultAsync();
        _ctx.Empresa.Remove(empresa);
        return empresa;
    }

    public async Task<Empresa> Abrir(int id)
    {
        return await _ctx.Empresa
            .AsNoTracking()
            .Where(x => x.EmpresaID == id)
            .FirstOrDefaultAsync();
    }

    public bool Existe(int id)
    {
        return _ctx.Empresa.Any(x => x.EmpresaID == id);
    }

    public async Task<PagedList<Empresa>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Empresa";
        PagedList<Empresa> empresas = null;

        IQueryable<Empresa> iEmpresas = _ctx.Empresa.AsNoTracking();

        if (paginacao.Ativos != null)
            iEmpresas = iEmpresas.Where(x => x.Ativo == paginacao.Ativos);

        if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        {
            if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
                throw new Exception($"Campo ({paginacao.PesquisarCampo}) não pode ser pesquisado!");

            switch (paginacao.PesquisarCampo)
            {
                case "CpfCnpj":
                    iEmpresas = iEmpresas.Where(x => x.CpfCnpj.Contains(paginacao.PesquisarValor));
                    break;

                case "NomeRazaoSocial":
                    iEmpresas = iEmpresas.Where(x => x.NomeRazaoSocial.Contains(paginacao.PesquisarValor));
                    break;

                case "ApelidoNomeFantasia":
                    iEmpresas = iEmpresas.Where(x => x.ApelidoNomeFantasia.Contains(paginacao.PesquisarValor));
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
                iEmpresas = iEmpresas.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
            else
                iEmpresas = iEmpresas.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        }

        empresas = await PagedList<Empresa>
           .ToPagedList(
                iEmpresas,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return empresas;
    }

    public async Task<List<Empresa>> Listar()
    {
        return await _ctx.Empresa
            .AsNoTracking()
            .Where(x => x.Ativo == true)
            .OrderBy(x => x.NomeRazaoSocial)
            .ToListAsync();
    }
}