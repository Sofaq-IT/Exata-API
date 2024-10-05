using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Exata.Domain.Entities;

public class Base
{
    private bool _booNovo;

    [JsonIgnore]
    public DateTime DataCadastro { get; set; }

    [JsonIgnore]
    public int? UserCadastro { get; set; }

    [JsonIgnore]
    public DateTime DataAlteracao { get; set; }

    [JsonIgnore]
    public int? UserAlteracao { get; set; }

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
            }
        }
    }
}
