﻿using Exata.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Exata.Domain.Interfaces
{
    public interface IAmostra
    {
        Task<Amostra> Inserir(Amostra amostra);
        Task SalvarAnexos(IFormFile[] attachments, Guid amostraId, DateTime dataReferencia);
    }
}
