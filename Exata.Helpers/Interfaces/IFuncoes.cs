using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.DTO;

namespace Exata.Helpers.Interfaces;

public interface IFuncoes
{
    public string[] errors { get; set; }

    public PaginacaoDTO Paginacao { get; set; }

    public bool ValidacaoHeaderPaginacao(IHeaderDictionary header);

    public int GenerateRandomNumber();
}
