using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Exata.Helpers.Interfaces;

namespace Exata.API.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;
    private readonly IErrorRequest _error;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger,
        IErrorRequest error)
    {
        _logger = logger;
        _error = error;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Ocorreu exceção não tratada.");

        context.Result = new ObjectResult(_error.InternalServerError($"{context.Exception.Message} - {context.Exception.InnerException}" , "Teste"))
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
