using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.DTO;

namespace Exata.Helpers.Interfaces;

public interface IErrorRequest
{
    string Titulo { get; set; }

    ErrorRequestDTO BadRequest(string Mensagem, string Titulo = "");

    ErrorRequestDTO BadRequest(string[] Mensagens, string Titulo = "");
    
    ErrorRequestDTO BadRequest(IdentityResult Result, string Titulo = "");

    ErrorRequestDTO BadRequest(ModelStateDictionary model, string Titulo = "");

    ErrorRequestDTO NotFound(string Mensagem = "Registro Não Encontrado!", string Titulo = "");

    ErrorRequestDTO NotFound(string[] Mensagens, string Titulo = "");
    
    ErrorRequestDTO InternalServerError(string Mensagem, string Titulo = "");

    ErrorRequestDTO Unauthorized(string Mensagem, string Titulo = "");
}
