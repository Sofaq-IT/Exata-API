using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Exata.Domain.DTO;
using Exata.Helpers.Interfaces;
using System.Text;

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

    public int GenerateRandomNumber()
    {
        Random random = new Random();
        // Gera um número entre 100000 e 999999, que são os limites para um número de 6 dígitos
        return random.Next(100000, 1000000);
    }

    public static string GenerateRandomString(int length)
    {
        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()";

        Random random = new Random();
        StringBuilder result = new StringBuilder(length);

        // Garantir pelo menos um de cada tipo de caractere necessário
        result.Append(uppercase[random.Next(uppercase.Length)]);
        result.Append(lowercase[random.Next(lowercase.Length)]);
        result.Append(digits[random.Next(digits.Length)]);
        result.Append(specialChars[random.Next(specialChars.Length)]);

        // Preencher os caracteres restantes aleatoriamente
        string allChars = uppercase + lowercase + digits + specialChars;
        for (int i = 3; i < length; i++)
        {
            result.Append(allChars[random.Next(allChars.Length)]);
        }

        // Embaralhar os caracteres para evitar que fiquem sempre na mesma posição
        return new string(result.ToString().OrderBy(_ => random.Next()).ToArray());
    }
}
