using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Domain.Paginacao;
using Exata.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Exata.Domain.Enums;
using Exata.Helpers;
using DocumentFormat.OpenXml.Office2010.Excel;
using Exata.Domain.DTO;

namespace Exata.API.Controllers;

/// <summary>
/// Controller utilizado para gerenciar Uploads
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IErrorRequest _error;
    private readonly IFuncoes _funcoes;
    private readonly IUsuario _usuario;
    private readonly IBlobStorage _blobStorage;
    private readonly IUpload _upload;

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
                            IUsuario usuario)
    {
        _uof = uof;
        _userManager = userManager;
        _error = erro;
        _error.Titulo = "Cliente";
        _funcoes = funcoes;
        _upload = upload;
        _blobStorage = blobStorage;
        _usuario = usuario;

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

    [HttpPost("ImportarArquivo/{clienteID}")]
    public async Task<IActionResult> ImportarArquivo([FromBody] UploadDTO uploadDTO, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Nenhum arquivo enviado.");

        try
        {
            var fileInfo = new FileInfo(file.FileName);

            var fileName = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + fileInfo.Extension;

            var fullPath = "uploads-realizados/" + fileName;

            var upload = new Upload()
            {
                ClienteId = uploadDTO.ClienteID,
                DataReferencia = uploadDTO.DataReferencia,
                NomeArquivoArmazenado = fileName,
                NomeArquivoEntrada = file.FileName,
                StatusAtual = StatusUploadEnum.Importado,
                Tamanho = file.Length / 1024
            };

            using (var stream = file.OpenReadStream())
            {
                upload.UrlStorage = await _blobStorage.UploadFileAsync(stream, fullPath);
            }

            var excelData = await _blobStorage.ReadExcelFileAsync(fullPath, upload);

            await _uof.Upload.Inserir(upload);

            await _uof.Commit();

            return Ok(excelData);
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
            var excelData = await _blobStorage.ReadExcelFileAsync("uploads-realizados/" + upload.NomeArquivoArmazenado, upload);

            foreach (var item in excelData)
            {
                // Implementar processamento do Arquivo
            }

            upload.StatusAtual = StatusUploadEnum.Processado;

            await _upload.Atualizar(upload);

            return Ok(upload);
        }
        catch (Exception ex)
        {
            upload.StatusAtual = StatusUploadEnum.Erro;

            await _upload.Atualizar(upload);

            return BadRequest(ex.Message);
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
