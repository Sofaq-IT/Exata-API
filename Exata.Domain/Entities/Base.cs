using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

public class Base
{
    [NotMapped]
    private bool _booNovo;

    [JsonIgnore]
    public DateTime DataCadastro { get; set; }

    [JsonIgnore]
    [StringLength(450)]
    public string UserCadastro { get; set; }

    [JsonIgnore]
    public DateTime DataAlteracao { get; set; }

    [JsonIgnore]
    [StringLength(450)]
    public string? UserAlteracao { get; set; }

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
    public bool booNovo {
        get {
            return _booNovo;            
        }
        set {
            _booNovo = value;
            DataAlteracao = DateTime.Now;
            if (_booNovo) { 
                DataCadastro = DataAlteracao;
                UserAlteracao = UserCadastro;
            }
        }
    }
}
