using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Exata.Domain.Enums;
using Exata.Domain.DTO;
using Newtonsoft.Json;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para gerenciar Uploads
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class UploadController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IErrorRequest _error;
    private readonly IFuncoes _funcoes;
    private readonly IUsuario _usuario;
    private readonly IBlobStorage _blobStorage;
    private readonly IUpload _upload;
    private readonly IAmostra _amostra;
    private readonly IAmostraResultado _amostraResultado;

    /// <summary>
    /// Controller utilizado para manter dados do Cliente
    /// </summary>
    /// <param name="uof"></param>
    /// <param name="userManager"></param>
    /// <param name="erro"></param>
    /// <param name="funcoes"></param>
    /// <param name="usuario"></param>
    public UploadController(IUnitOfWork uof,
                            UserManager<ApplicationUser> userManager,
                            IErrorRequest erro,
                            IFuncoes funcoes,
                            IBlobStorage blobStorage,
                            IUpload upload,
                            IUsuario usuario,
                            IAmostra amostra,
                            IAmostraResultado amostraResultado)
    {
        _uof = uof;
        _userManager = userManager;
        _error = erro;
        _error.Titulo = "Cliente";
        _funcoes = funcoes;
        _upload = upload;
        _blobStorage = blobStorage;
        _usuario = usuario;
        _amostra = amostra;
        _amostraResultado = amostraResultado;

    }

    [HttpGet, Route("Listar")]
    public virtual async Task<IActionResult> Listar()
    {
        try
        {
            var uploads = await _upload.Listar();

            return Ok(uploads);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet, Route("ListarAnexos/{uploadId}")]
    public virtual async Task<IActionResult> Listar(int uploadId)
    {
        try
        {
            var upload = await _uof.Upload.Abrir(uploadId);

            if (upload == null)
                return NotFound("Upload não localizado");

            var anexos = await _uof.Amostra.ListarAnexos(upload.AmostraId);

            return Ok(anexos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost, Route("ImportarArquivo")]
    public async Task<IActionResult> ImportarArquivo([FromForm] string uploadDTO, [FromForm] IFormFile file, [FromForm] IFormFile[] attachments)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        try
        {
            var up = JsonConvert.DeserializeObject<UploadDTO>(uploadDTO);

            var fileInfo = new FileInfo(file.FileName);

            var fileName = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + fileInfo.Extension;

            var fullPath = "uploads-realizados/" + fileName;

            var amostra = await _amostra.Inserir(new Amostra()
            {
                Codigo = string.Empty,
                ClienteId = up.ClienteID
            });

            var upload = new Upload()
            {
                AmostraId = amostra.AmostraId,
                DataReferencia = up.DataReferencia,
                NomeArquivoArmazenado = fileName,
                NomeArquivoEntrada = file.FileName,
                StatusAtual = StatusUploadEnum.Importado,
                Tamanho = file.Length / 1024,
                TipoUpload = TipoUploadEnum.Resultado
            };

            using (var stream = file.OpenReadStream())
            {
                upload.UrlStorage = await _blobStorage.UploadFileAsync(stream, fullPath);
            }

            await _blobStorage.ReadExcelFileAsync(fullPath, upload);

            await _uof.Upload.Inserir(upload);

            if (attachments != null || attachments.Length != 0)
                await _uof.Amostra.SalvarAnexos(attachments, amostra.AmostraId, upload.DataReferencia);

            await _uof.Commit();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest("Erro ao realizar upload do arquivo: " + ex.Message);
        }
    }

    [HttpGet("processar/{id}")]
    public virtual async Task<IActionResult> Processar(int id)
    {
        var upload = await _upload.Abrir(id);

        if (upload == null)
            return NotFound("Não localizamos um Upload com o ID informado!");

        try
        {
            var excelData = await _blobStorage.ReadDataTableExcelFileAsync("uploads-realizados/" + upload.NomeArquivoArmazenado);

            await _uof.AmostraResultado.InserirTodos(excelData, upload.AmostraId);

            upload.StatusAtual = StatusUploadEnum.Processado;

            await _uof.Upload.Atualizar(upload);

            await _uof.Commit();

            return Ok(upload);
        }
        catch (Exception ex)
        {
            upload.StatusAtual = StatusUploadEnum.Erro;

            await _upload.Atualizar(upload);

            return BadRequest(ex.Message);
        }
    }

    [HttpPost, Route("ValidarArquivo")]
    public async Task<IActionResult> ValidarArquivo([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        try
        {
            var fileInfo = new FileInfo(file.FileName);

            var fileName = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + fileInfo.Extension;

            var fullPath = "uploads-realizados/" + fileName;

            using (var stream = file.OpenReadStream())
            {
                await _blobStorage.UploadFileAsync(stream, fullPath);
            }

            var excelData = await _blobStorage.ReadExcelFileAsync(fullPath);

            return Ok(excelData);
        }
        catch (Exception ex)
        {
            return BadRequest("Erro ao validar arquivo: " + ex.Message);
        }
    }

    [HttpGet, Route("Detalhes/{uploadID}")]
    public virtual async Task<IActionResult> Detalhes(int uploadID)
    {
        try
        {
            var upload = await _upload.Abrir(uploadID);

            if (upload == null)
                return NotFound("Não localizamos um Upload com o ID informado!");

            var excelData = await _blobStorage.ReadExcelFileAsync("uploads-realizados/" + upload.NomeArquivoArmazenado, upload);

            return Ok(excelData);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
