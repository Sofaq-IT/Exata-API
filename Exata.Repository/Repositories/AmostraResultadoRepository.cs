using Exata.Domain.Entities;
using Exata.Domain.Interfaces;
using Exata.Repository.Context;
using System.Data;

namespace Exata.Repository.Repositories;

public class AmostraResultadoRepository : IAmostraResultado
{
    private readonly ApiContext _ctx;
    private readonly IUsuario _usuario;

    public AmostraResultadoRepository(ApiContext context, IUsuario usuario)
    {
        _ctx = context;
        _usuario = usuario;
    }

    public async Task<AmostraResultado> Inserir(AmostraResultado amostraResultado)
    {
        amostraResultado.UserCadastro = await _usuario.UserID();
        amostraResultado.booNovo = true;
        _ctx.Add(amostraResultado);
        return amostraResultado;
    }

    public async Task InserirTodos(DataTable dadosImportados, Guid amostraId)
    {
        var userCadastro = await _usuario.UserID();
        bool isFirstRow = true;
        
        foreach (DataRow item in dadosImportados.Rows)
        {
            var amostraResultado = new AmostraResultado()
            {
                Al = item["Al"].ToString(),
                AmostraId = amostraId,
                AreiaFina = item["Areia Fina"].ToString(),
                AreiaGrossa = item["Areia Grossa"].ToString(),
                AreiaTotal = item["Areia Total"].ToString(),
                Argila = item["Argila"].ToString(),                
                B = item["B"].ToString(),
                booNovo = true,
                Ca = item["Ca"].ToString(),
                CaCTCEfetiva = item["Ca/t "].ToString(),
                CaCTCTotal = item["Ca/T"].ToString(),
                CaK = item["Ca/K"].ToString(),
                CaMg = item["Ca/Mg"].ToString(),
                CaplusMgK = item["(Ca+Mg)/K"].ToString(),
                CaplusMgplusKplusNaT = item["(Ca+Mg+K+Na)/T"].ToString(),
                CaplusMgplusKT = item["(Ca+Mg+K)/T"].ToString(),
                CaplusMgT = item["(Ca+Mg)/T"].ToString(),
                CO = item["CO"].ToString(),
                CTCEfetiva = item["t"].ToString(),
                CTCTotal = item["T "].ToString(),
                Cu = item["Cu"].ToString(),
                Fazenda = item["FAZENDA"].ToString(),
                Fe = item["Fe"].ToString(),
                Gleba = item["GLEBA"].ToString(),
                HplusAl = item["H + Al"].ToString(),
                HplusAlT = item["(H+Al)/T"].ToString(),
                IdAmostraLab = item["ID AMOSTRA LAB"].ToString(),
                K = item["K"].ToString(),
                KT = item["K/T "].ToString(),
                m = item["m"].ToString(),
                Mg = item["Mg"].ToString(),
                MgCTCEfetiva = item["Mg/t "].ToString(),
                MgCTCTotal = item["Mg/T"].ToString(),
                MgK = item["Mg/K"].ToString(),
                Mn = item["Mn"].ToString(),
                MO = item["MO"].ToString(),
                Na = item["Na"].ToString(),
                NaT = item["Na/T"].ToString(),
                pHCaCl = item["pH CaCl"].ToString(),
                pHH2O = item["pH H2O"].ToString(),
                pHSMP = item["pH SMP"].ToString(),
                Pmeh = item["P meh"].ToString(),
                PontoColeta = item["PONTO DE COLETA"].ToString(),
                Prem = item["P rem"].ToString(),
                Pres = item["P res"].ToString(),
                Profundidade = item["PROFUNDIDADE"].ToString(),
                Ptotal = item["P total"].ToString(),
                S = item["S"].ToString(),
                SB = item["SB"].ToString(),
                Silite = item["Silte"].ToString(),
                Talhao = item["TALHÃO"].ToString(),
                UserCadastro = userCadastro,
                V = item["V"].ToString(),
                Zn = item["Zn"].ToString()
            };
            
            if (isFirstRow)
            {
                amostraResultado.TipoInformacao = "M";
                isFirstRow = false;
            }                
            else
                amostraResultado.TipoInformacao = "R";

            _ctx.Add(amostraResultado);            
        }

        await _ctx.SaveChangesAsync();
    }
}