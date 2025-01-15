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

        if (user != null && user.EmpresaID != null)
        {
            return await _ctx.Upload
                         .Where(x => x.UserCadastro == user.Id && x.TipoUpload == TipoUploadEnum.Resultado)
                         .AsNoTracking()
                         .Include("Amostra.Cliente")
                         .OrderByDescending(x => x.DataCadastro)
                         .ToListAsync();
        }

        if (user != null && user.ClienteID != null)
        {
            var uploads = await (from u in _ctx.Upload
                                 join a in _ctx.Amostra on u.AmostraId equals a.AmostraId
                                 where a.ClienteId == user.ClienteID && u.TipoUpload == TipoUploadEnum.Resultado
                                 select u).AsNoTracking()
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