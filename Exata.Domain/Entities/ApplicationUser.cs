using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Exata.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [NotMapped]
    private bool _booNovo;

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    public bool Ativo { get; set; } = true;

    public string Nome { get; set; }

    [MaxLength(10)]
    public string Tema { get; set; } = "light";

    public DateTime DataUltLogin { get; set; }

    [JsonIgnore]
    public DateTime DataCadastro { get; set; }

    [JsonIgnore]
    public DateTime DataAlteracao { get; set; }

    public int? PerfilID { get; set; }

    public UsuarioAvatar UsuarioAvatar { get; set; }

    public virtual Perfil Perfil { get; set; }
        
    public List<Perfil> PerfilCriacao { get; set; }

    public List<Perfil> PerfilAlteracao { get; set; }
    public int? CodigoVerificacaoEsqueciMinhaSenha { get; set; }

    [NotMapped]
    public string DataCadastroFormatada
    {
        get { return DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"); }
    }

    [NotMapped]
    public string DataAlteracaoFormatada
    {
        get { return DataAlteracao.ToString("dd/MM/yyyy HH:mm:ss"); }
    }

    [JsonIgnore]
    [NotMapped]
    public bool booNovo
    {
        get
        {
            return _booNovo;
        }
        set
        {
            _booNovo = value;
            DataAlteracao = DateTime.Now;
            if (_booNovo)
            {
                DataCadastro = DataAlteracao;
            }
        }
    }
}