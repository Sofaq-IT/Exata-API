using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System;
using System.Text;
using System.Text.Json;
using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Helpers.Interfaces;
using Exata.Repository.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Exata.API.Filters;

/// <summary>
/// Filtro usado para verificar acesso do usuário, e loggar informações
/// </summary>
public class PermissaoFilter : IActionFilter
{
    private readonly ILogger<PermissaoFilter> _logger;
    private readonly IErrorRequest _error;
    private readonly IVariaveisAmbiente _varAmbiente;

    /// <summary>
    /// Filtro usado para verificar acesso do usuário, e loggar informações
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="error"></param>
    /// <param name="varAmbiente"></param>
    public PermissaoFilter(ILogger<PermissaoFilter> logger,
                           IErrorRequest error,
                           IVariaveisAmbiente varAmbiente)
    {
        _logger = logger;
        _error = error;
        _varAmbiente = varAmbiente;
    }

    /// <summary>
    /// Quando uma action e acionada, esse função entra em execução, e faz a validação do acesso do usuário
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        string metodo = context.HttpContext.Request.Method;
        string remoteAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
        string controller = actionDescriptor.ControllerName;
        string action = actionDescriptor.ActionName;
        string user = context.HttpContext.User.Identity.Name;
        string traceId = context.HttpContext.TraceIdentifier;
        if (string.IsNullOrEmpty(user))
            user = "";

        _logger.LogInformation("# Executando -> {metodo} {controller}.{action} ({user}) ({remoteAddress}) ({traceID})- {DateTime.Now.ToLocalTime()}",
            metodo, controller, action, user, remoteAddress, traceId, DateTime.Now.ToLocalTime());

        var _db = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

        if (!string.Equals(controller, "Autenticacao") &&
            !(string.Equals(controller, "Contratante") && string.Equals(action, "Logo")) &&
            !(string.Equals(controller, "Usuario") && string.Equals(action, "UsuarioADM")))
        {
            ControllerAction controllerAction = new()
            {
                Metodo = metodo,
                Controller = controller,
                Action = action,
            };

            ControllerAction controllerActionPermissao = _db.ControllerAction.Abrir(controllerAction);

            if (controllerActionPermissao == null) 
            { 
                controllerActionPermissao = _db.ControllerAction.Inserir(controllerAction);
            }
            if (!string.Equals(user, _varAmbiente.UsuarioADM))
            {
                if (!_db.ControllerAction.PermissaoValida(controllerActionPermissao, user))
                {
                    _logger.LogInformation("# Sem Autorização -> {metodo} {controller}.{action} ({user}) - {DateTime.Now.ToLocalTime()}",
                        metodo, controller, action, user, DateTime.Now.ToLocalTime());
                    context.Result = new ObjectResult(_error.Unauthorized("Sem autorização para utilizar a funcionalidade!", "Sem Autorização!"))
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
            }
        }

        if (_varAmbiente.LogarRequisicoes.Contains(metodo.ToUpper()) && !string.IsNullOrEmpty(user))
        {
            StringBuilder sb = new StringBuilder();
            foreach (var arg in context.ActionArguments)
            {
                sb.Append(arg.Key.ToString() + ":" + JsonSerializer.Serialize(arg.Value) + "\n");
            }
            var body = sb.ToString();
            _logger.LogInformation("# Body -> {metodo} {controller}.{action} ({user}) ({remoteAddress}) ({traceID}) ({body}) - {DateTime.Now.ToLocalTime()}",
                metodo, controller, action, user, remoteAddress, traceId, body, DateTime.Now.ToLocalTime());

            if (_varAmbiente.LogarRequisicoes.Contains(metodo))
            {
                LogRequisicao logRequisicao = new()
                {
                    Data = DateTime.Now,
                    UsuarioID = user,
                    Metodo = metodo,
                    Controller = controller,
                    Action = action,
                    RemoteAddress = remoteAddress,
                    TraceID = traceId,
                    Body = body
                };

                try {
                    logRequisicao = _db.LogRequisicao.Inserir(logRequisicao);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("# Body -> {erro} - {DateTime.Now.ToLocalTime()}",
                        ex.ToString(), DateTime.Now.ToLocalTime());
                }
            }
        }
    }

    /// <summary>
    /// Ao finalizar a execução de uma action, esta função é executada, gerando log do fim de execução
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        string metodo = context.HttpContext.Request.Method;
        string controller = actionDescriptor.ControllerName;
        string action = actionDescriptor.ActionName;
        string user = context.HttpContext.User.Identity.Name;
        string traceId = context.HttpContext.TraceIdentifier;
        if (string.IsNullOrEmpty(user))
            user = "";

        _logger.LogInformation("# Finalizando -> {metodo} {controller}.{action} ({user}) ({traceId}) - {DateTime.Now.ToLocalTime()}",
            metodo, controller, action, user, traceId, DateTime.Now.ToLocalTime());
    }
}
