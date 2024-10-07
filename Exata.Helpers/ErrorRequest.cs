using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Helpers.Interfaces;

namespace Exata.Helpers;

public class ErrorRequest : IErrorRequest
{

    private readonly ILogger<ErrorRequest> _logger;

    public string Titulo { get; set; }

    public ErrorRequest(ILogger<ErrorRequest> logger)
    {
        _logger = logger;
    }

    public ErrorRequestDTO BadRequest(string mensagem, string titulo = "")
    {
        return RetornoBadRequest([mensagem], titulo);
    }

    public ErrorRequestDTO BadRequest(string[] mensagens, string titulo = "")
    {
        return RetornoBadRequest(mensagens, titulo);
    }

    public ErrorRequestDTO BadRequest(IdentityResult result, string titulo = "")
    {
        return RetornoBadRequest(arrMsgs(result), titulo);
    }

    public ErrorRequestDTO BadRequest(ModelStateDictionary model, string titulo = "")
    {
        return RetornoBadRequest(arrMsgs(model), titulo);
    }

    private ErrorRequestDTO RetornoBadRequest(string[] mensagem, string titulo = "")
    {
        ErrorRequestDTO badRequest = new()
        {
            title = tituloRetorno(titulo),
            mensagens = mensagem
        };

        _logger.LogInformation("BadRequest - {badrequest}", JsonSerializer.Serialize(badRequest));
        return badRequest;
    }

    public ErrorRequestDTO NotFound(string mensagem = "Registro Não Encontrado!", string titulo = "")
    {
        return RetornoNotFound([mensagem], titulo);
    }

    public ErrorRequestDTO NotFound(string[] mensagens, string titulo = "")
    {
        return RetornoNotFound(mensagens, titulo);
    }

    private ErrorRequestDTO RetornoNotFound(string[] mensagens, string titulo = "")
    {
        ErrorRequestDTO notFountRequest = new()
        {
            title = tituloRetorno(titulo),
            mensagens = mensagens,
            status = 404
        };

        _logger.LogInformation("NotFound - {notfound}", JsonSerializer.Serialize(notFountRequest));
        return notFountRequest;
    }

    public ErrorRequestDTO InternalServerError(string mensagem, string titulo = "")
    {
        return RetornoInternalServerError([mensagem], titulo);
    }

    private ErrorRequestDTO RetornoInternalServerError(string[] mensagens, string titulo = "")
    {
        ErrorRequestDTO internalServerError = new()
        {
            title = tituloRetorno(titulo),
            mensagens = mensagens,
            status = 500
        };

        _logger.LogError("InternalServerError - {internalServerError}", JsonSerializer.Serialize(internalServerError));
        return internalServerError;
    }

    public ErrorRequestDTO Unauthorized(string mensagem, string titulo = "")
    {
        return RetornoUnauthorized([mensagem], titulo);
    }

    private ErrorRequestDTO RetornoUnauthorized(string[] mensagens, string titulo = "")
    {
        ErrorRequestDTO unauthorized = new()
        {
            title = tituloRetorno(titulo),
            mensagens = mensagens,
            status = 401
        };

        _logger.LogError("Unauthorized - {Unauthorized}", JsonSerializer.Serialize(unauthorized));
        return unauthorized;
    }

    private string tituloRetorno(string retTitulo)
    {
        return (!string.IsNullOrEmpty(retTitulo) ? retTitulo : Titulo );
    }

    private string[] arrMsgs(IdentityResult result)
    {
        string[] msgErro = new string[result.Errors.Count()];
        int i = 0;
        foreach (var erro in result.Errors)
        {
            msgErro[i] = erro.Description;
            i++;
        }

        return msgErro;
    }

    private string[] arrMsgs(ModelStateDictionary model)
    {
        string[] msgErro = (string[])model.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);

        return msgErro;
    }
}
