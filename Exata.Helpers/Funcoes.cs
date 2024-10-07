using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Helpers.Interfaces;

namespace Exata.Helpers;

public class Funcoes : IFuncoes
{
    private readonly JsonSerializerOptions _jsonOptions;

    public PaginacaoDTO Paginacao { get; set; }

    public string[] errors { get; set; }

    public Funcoes()
    {
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public bool ValidacaoHeaderPaginacao(IHeaderDictionary header)
    {
        if (header.TryGetValue("x-Paginacao", out var hPaginacao))
        {
            Paginacao = JsonSerializer.Deserialize<PaginacaoDTO>(hPaginacao, _jsonOptions);
            var validationResultList = new List<ValidationResult>();
            if (!Validator.TryValidateObject(Paginacao, new ValidationContext(Paginacao), validationResultList, true))
            {
                errors = ArrMsgs(validationResultList);
                return false;
            }
        }
        else
        {
            errors = ["Header x-Paginacao não informado."];
            return false;
        }

        return true;
    }

    private string[] ArrMsgs(List<ValidationResult> validationResult)
    {
        string[] _errors = new string[validationResult.Count()];
        int i = 0;
        foreach(var item  in validationResult)
        {
            _errors[i] = item.ErrorMessage;
            i++;
        }

        return _errors;
    }
}
