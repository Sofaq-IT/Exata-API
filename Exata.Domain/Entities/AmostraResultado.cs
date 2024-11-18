using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exata.Domain.Entities
{
    [Table("AmostraResultado")]
    public class AmostraResultado : Base
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid AmostraId { get; set; }

        [Required]
        [StringLength(1)]
        public string TipoInformacao { get; set; }
        
        public string IdAmostraLab { get; set; }
        public string Fazenda { get; set; }
        public string Talhao { get; set; }
        public string Gleba { get; set; }
        public string Profundidade { get; set; }
        public string PontoColeta { get; set; }
        public string pHH2O { get; set; }
        public string pHCaCl { get; set; }
        public string pHSMP { get; set; }
        public string Pmeh { get; set; }
        public string Prem { get; set; }
        public string Pres { get; set; }
        public string Ptotal { get; set; }
        public string Na { get; set; }
        public string K { get; set; }
        public string S { get; set; }
        public string Ca { get; set; }
        public string Mg { get; set; }
        public string Al { get; set; }
        public string HplusAl { get; set; }
        public string MO { get; set; }
        public string CO { get; set; }
        public string B { get; set; }
        public string Cu { get; set; }
        public string Fe { get; set; }
        public string Mn { get; set; }
        public string Zn { get; set; }
        public string SB { get; set; }
        public string CTCEfetiva { get; set; }
        public string CTCTotal { get; set; }
        public string V { get; set; }
        public string m { get; set; }
        public string CaMg { get; set; }
        public string CaK { get; set; }
        public string MgK { get; set; }
        public string CaplusMgK { get; set; }
        public string CaCTCEfetiva { get; set; }
        public string MgCTCEfetiva { get; set; }
        public string CaCTCTotal { get; set; }
        public string MgCTCTotal { get; set; }
        public string KT { get; set; }
        public string NaT { get; set; }
        public string HplusAlT { get; set; }
        public string CaplusMgT { get; set; }
        public string CaplusMgplusKT { get; set; }
        public string CaplusMgplusKplusNaT { get; set; }
        public string Argila { get; set; }
        public string Silite { get; set; }
        public string AreiaTotal { get; set; }
        public string AreiaGrossa { get; set; }
        public string AreiaFina { get; set; }

        public virtual Amostra Amostra { get; set; }
    }
}
