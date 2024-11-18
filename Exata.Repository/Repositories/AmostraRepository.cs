using Exata.Domain.Entities;
using Exata.Domain.Enums;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;
using Exata.Repository.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Exata.Repository.Repositories;

public class AmostraRepository : IAmostra
{
    private readonly ApiContext _ctx;
    private readonly IUsuario _usuario;
    private readonly IBlobStorage _blobStorage;

    public AmostraRepository(ApiContext context, IUsuario usuario, IBlobStorage blobStorage)
    {
        _ctx = context;
        _usuario = usuario;
        _blobStorage = blobStorage;
    }

    public async Task<Amostra> Inserir(Amostra amostra)
    {
        amostra.UserCadastro = await _usuario.UserID();
        amostra.booNovo = true;
        _ctx.Add(amostra);
        return amostra;
    }

    public async Task<List<Upload>> ListarAnexos(Guid amostraId)
    {
        return await _ctx.Upload
                .Where(x => x.AmostraId == amostraId && x.TipoUpload == TipoUploadEnum.Anexo)
                .ToListAsync();
    }

    public async Task SalvarAnexos(IFormFile[] attachments, Guid amostraId, DateTime dataReferencia)
    {
        if (attachments.Length > 0)
        {
            var userCadastro = await _usuario.UserID();

            foreach (var item in attachments)
            {
                var fileInfo = new FileInfo(item.FileName);

                var fileName = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + fileInfo.Extension;

                var fullPath = "uploads-realizados/" + fileName;

                var upload = new Upload()
                {
                    AmostraId = amostraId,
                    booNovo = true,
                    DataReferencia = dataReferencia,
                    NomeArquivoArmazenado = fileName,
                    NomeArquivoEntrada = item.FileName,
                    QtdeRegistros = 0,
                    StatusAtual = StatusUploadEnum.Importado,
                    Tamanho = item.Length / 1024,
                    TipoUpload = TipoUploadEnum.Anexo,
                    UserCadastro = userCadastro
                };

                using (var stream = item.OpenReadStream())
                {
                    upload.UrlStorage = await _blobStorage.UploadFileAsync(stream, fullPath);
                }

                _ctx.Add(upload);
            }
        }
    }
}