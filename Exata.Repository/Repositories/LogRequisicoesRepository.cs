using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exata.Domain.Interfaces;
using Exata.Domain.Entities;
using Exata.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Exata.Repository.Repositories;

public class LogRequisicoesRepository : ILogRequisicao
{
    private readonly ApiContext _ctx;

    public LogRequisicoesRepository(ApiContext context)
    {
        _ctx = context;
    }
    public LogRequisicao Inserir(LogRequisicao logRequisicao)
    {
        string userID = _ctx.Users.Where(x => x.UserName == logRequisicao.UsuarioID).Select(x => x.Id).FirstOrDefault();
        logRequisicao.UsuarioID = userID;
        _ctx.Add(logRequisicao);
        return logRequisicao;
    }
}
