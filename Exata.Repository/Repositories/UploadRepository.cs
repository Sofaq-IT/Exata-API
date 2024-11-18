using Microsoft.EntityFrameworkCore;
using Exata.Domain.DTO;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Repository.Context;
using Exata.Domain.Enums;

namespace Exata.Repository.Repositories;

public class UploadRepository : IUpload
{
    private readonly ApiContext _ctx;
    private readonly IUsuario _usuario;

    public UploadRepository(ApiContext context, IUsuario usuario)
    {
        _ctx = context;
        _usuario = usuario;
    }

    public async Task<Upload> Inserir(Upload upload)
    {
        upload.UserCadastro = await _usuario.UserID();
        upload.booNovo = true;
        _ctx.Add(upload);
        return upload;
    }

    public async Task<Upload> Atualizar(Upload upload)
    {
        upload.UserAlteracao = await _usuario.UserID();
        upload.booNovo = false;
        var update = _ctx.Entry(upload);
        update.State = EntityState.Modified;
        update.Property("DataCadastro").IsModified = false;
        update.Property("UserCadastro").IsModified = false;
        return upload;
    }

    public async Task<PagedList<Upload>> Listar(PaginacaoDTO paginacao)
    {
        string tabela = "Upload";
        PagedList<Upload> uploads = null;

        IQueryable<Upload> iUploads = _ctx.Upload.AsNoTracking();

        //if (!string.IsNullOrEmpty(paginacao.PesquisarCampo) && !string.IsNullOrEmpty(paginacao.PesquisarValor))
        //{
        //    if (!_campo.ExistePesquisa(tabela, paginacao.PesquisarCampo))
        //        throw new Exception($"Campo ({paginacao.PesquisarCampo}) não pode ser pesquisado!");

        //    switch (paginacao.PesquisarCampo)
        //    {
        //        case "CpfCnpj":
        //            iEmpresas = iEmpresas.Where(x => x.CpfCnpj.Contains(paginacao.PesquisarValor));
        //            break;

        //        case "NomeRazaoSocial":
        //            iEmpresas = iEmpresas.Where(x => x.NomeRazaoSocial.Contains(paginacao.PesquisarValor));
        //            break;

        //        case "ApelidoNomeFantasia":
        //            iEmpresas = iEmpresas.Where(x => x.ApelidoNomeFantasia.Contains(paginacao.PesquisarValor));
        //            break;

        //        default:
        //            throw new Exception($"Campo ({paginacao.PesquisarCampo}) não pode ser pesquisado!");
        //    }
        //}

        //if (!string.IsNullOrEmpty(paginacao.OrderCampo))
        //{
        //    if (!_campo.ExisteOrdenacao(tabela, paginacao.OrderCampo))
        //        throw new Exception($"Campo ({paginacao.OrderCampo}) não pode ser Ordenado!");

        //    if (paginacao.OrderTipoAsc == true)
        //        iEmpresas = iEmpresas.OrderBy(x => EF.Property<object>(x!, paginacao.OrderCampo));
        //    else
        //        iEmpresas = iEmpresas.OrderByDescending(x => EF.Property<object>(x!, paginacao.OrderCampo));
        //}

        uploads = await PagedList<Upload>
           .ToPagedList(
                iUploads,
                paginacao.Pagina,
                paginacao.RegistroPorPagina);

        return uploads;
    }

    public async Task<List<Upload>> Listar()
    {
        var userID = await _usuario.UserID();

        var user = _ctx.Users.Where(x => x.Id == userID).FirstOrDefault();

        if (user.EmpresaID != null)
        {
            return await _ctx.Upload
                         .Where(x => x.UserCadastro == user.Id && x.TipoUpload == TipoUploadEnum.Resultado)
                         .AsNoTracking()
                         .Include("Amostra.Cliente")
                         .OrderByDescending(x => x.DataCadastro)
                         .ToListAsync();
        }

        return await _ctx.Upload
            .Where(x => x.TipoUpload == TipoUploadEnum.Resultado)
            .AsNoTracking()
            .Include("Amostra.Cliente")
            .OrderByDescending(x => x.DataCadastro)
            .ToListAsync();
    }

    public async Task<Upload> Abrir(int id)
    {
        return await _ctx.Upload
            .AsNoTracking()
            .Where(x => x.UploadID == id)
            .FirstOrDefaultAsync();
    }
}