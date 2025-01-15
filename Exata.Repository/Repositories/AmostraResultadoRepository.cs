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
                Al = item.Table.Columns.Contains("Al") ? item["Al"]?.ToString() ?? string.Empty : string.Empty,
                AmostraId = amostraId,
                AreiaFina = item.Table.Columns.Contains("Areia Fina") ? item["Areia Fina"]?.ToString() ?? string.Empty : string.Empty,
                AreiaGrossa = item.Table.Columns.Contains("Areia Grossa") ? item["Areia Grossa"]?.ToString() ?? string.Empty : string.Empty,
                AreiaTotal = item.Table.Columns.Contains("Areia Total") ? item["Areia Total"]?.ToString() ?? string.Empty : string.Empty,
                Argila = item.Table.Columns.Contains("Argila") ? item["Argila"]?.ToString() ?? string.Empty : string.Empty,
                B = item.Table.Columns.Contains("B") ? item["B"]?.ToString() ?? string.Empty : string.Empty,
                booNovo = true,
                Ca = item.Table.Columns.Contains("Ca") ? item["Ca"]?.ToString() ?? string.Empty : string.Empty,
                CaCTCEfetiva = item.Table.Columns.Contains("Ca/t ") ? item["Ca/t "]?.ToString() ?? string.Empty : string.Empty,
                CaCTCTotal = item.Table.Columns.Contains("Ca/T") ? item["Ca/T"]?.ToString() ?? string.Empty : string.Empty,
                CaK = item.Table.Columns.Contains("Ca/K") ? item["Ca/K"]?.ToString() ?? string.Empty : string.Empty,
                CaMg = item.Table.Columns.Contains("Ca/Mg") ? item["Ca/Mg"]?.ToString() ?? string.Empty : string.Empty,
                CaplusMgK = item.Table.Columns.Contains("(Ca+Mg)/K") ? item["(Ca+Mg)/K"]?.ToString() ?? string.Empty : string.Empty,
                CaplusMgplusKplusNaT = item.Table.Columns.Contains("(Ca+Mg+K+Na)/T") ? item["(Ca+Mg+K+Na)/T"]?.ToString() ?? string.Empty : string.Empty,
                CaplusMgplusKT = item.Table.Columns.Contains("(Ca+Mg+K)/T") ? item["(Ca+Mg+K)/T"]?.ToString() ?? string.Empty : string.Empty,
                CaplusMgT = item.Table.Columns.Contains("(Ca+Mg)/T") ? item["(Ca+Mg)/T"]?.ToString() ?? string.Empty : string.Empty,
                CO = item.Table.Columns.Contains("CO") ? item["CO"]?.ToString() ?? string.Empty : string.Empty,
                CTCEfetiva = item.Table.Columns.Contains("t") ? item["t"]?.ToString() ?? string.Empty : string.Empty,
                CTCTotal = item.Table.Columns.Contains("T ") ? item["T "]?.ToString() ?? string.Empty : string.Empty,
                Cu = item.Table.Columns.Contains("Cu") ? item["Cu"]?.ToString() ?? string.Empty : string.Empty,
                Fazenda = item.Table.Columns.Contains("FAZENDA") ? item["FAZENDA"]?.ToString() ?? string.Empty : string.Empty,
                Fe = item.Table.Columns.Contains("Fe") ? item["Fe"]?.ToString() ?? string.Empty : string.Empty,
                Gleba = item.Table.Columns.Contains("GLEBA") ? item["GLEBA"]?.ToString() ?? string.Empty : string.Empty,
                HplusAl = item.Table.Columns.Contains("H + Al") ? item["H + Al"]?.ToString() ?? string.Empty : string.Empty,
                HplusAlT = item.Table.Columns.Contains("(H+Al)/T") ? item["(H+Al)/T"]?.ToString() ?? string.Empty : string.Empty,
                IdAmostraLab = item.Table.Columns.Contains("ID AMOSTRA LAB") ? item["ID AMOSTRA LAB"]?.ToString() ?? string.Empty : string.Empty,
                K = item.Table.Columns.Contains("K") ? item["K"]?.ToString() ?? string.Empty : string.Empty,
                KT = item.Table.Columns.Contains("K/T ") ? item["K/T "]?.ToString() ?? string.Empty : string.Empty,
                m = item.Table.Columns.Contains("m") ? item["m"]?.ToString() ?? string.Empty : string.Empty,
                Mg = item.Table.Columns.Contains("Mg") ? item["Mg"]?.ToString() ?? string.Empty : string.Empty,
                MgCTCEfetiva = item.Table.Columns.Contains("Mg/t ") ? item["Mg/t "]?.ToString() ?? string.Empty : string.Empty,
                MgCTCTotal = item.Table.Columns.Contains("Mg/T") ? item["Mg/T"]?.ToString() ?? string.Empty : string.Empty,
                MgK = item.Table.Columns.Contains("Mg/K") ? item["Mg/K"]?.ToString() ?? string.Empty : string.Empty,
                Mn = item.Table.Columns.Contains("Mn") ? item["Mn"]?.ToString() ?? string.Empty : string.Empty,
                MO = item.Table.Columns.Contains("MO") ? item["MO"]?.ToString() ?? string.Empty : string.Empty,
                Na = item.Table.Columns.Contains("Na") ? item["Na"]?.ToString() ?? string.Empty : string.Empty,
                NaT = item.Table.Columns.Contains("Na/T") ? item["Na/T"]?.ToString() ?? string.Empty : string.Empty,
                pHCaCl = item.Table.Columns.Contains("pH CaCl") ? item["pH CaCl"]?.ToString() ?? string.Empty : string.Empty,
                pHH2O = item.Table.Columns.Contains("pH H2O") ? item["pH H2O"]?.ToString() ?? string.Empty : string.Empty,
                pHSMP = item.Table.Columns.Contains("pH SMP") ? item["pH SMP"]?.ToString() ?? string.Empty : string.Empty,
                Pmeh = item.Table.Columns.Contains("P meh") ? item["P meh"]?.ToString() ?? string.Empty : string.Empty,
                PontoColeta = item.Table.Columns.Contains("PONTO DE COLETA") ? item["PONTO DE COLETA"]?.ToString() ?? string.Empty : string.Empty,
                Prem = item.Table.Columns.Contains("P rem") ? item["P rem"]?.ToString() ?? string.Empty : string.Empty,
                Pres = item.Table.Columns.Contains("P res") ? item["P res"]?.ToString() ?? string.Empty : string.Empty,
                Profundidade = item.Table.Columns.Contains("PROFUNDIDADE") ? item["PROFUNDIDADE"]?.ToString() ?? string.Empty : string.Empty,
                Ptotal = item.Table.Columns.Contains("P total") ? item["P total"]?.ToString() ?? string.Empty : string.Empty,
                S = item.Table.Columns.Contains("S") ? item["S"]?.ToString() ?? string.Empty : string.Empty,
                SB = item.Table.Columns.Contains("SB") ? item["SB"]?.ToString() ?? string.Empty : string.Empty,
                Silite = item.Table.Columns.Contains("Silte") ? item["Silte"]?.ToString() ?? string.Empty : string.Empty,
                Talhao = item.Table.Columns.Contains("TALHÃO") ? item["TALHÃO"]?.ToString() ?? string.Empty : string.Empty,
                UserCadastro = userCadastro,
                V = item.Table.Columns.Contains("V") ? item["V"]?.ToString() ?? string.Empty : string.Empty,
                Zn = item.Table.Columns.Contains("Zn") ? item["Zn"]?.ToString() ?? string.Empty : string.Empty
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