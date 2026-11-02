
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Candidatura
    {
        [Key]
        public int IdCandidatura { get; set; }

        
        public DateTime DataSubmissao { get; set; }

        public int IdCandidato { get; set; }

        public int IdPrograma { get; set; }

        public int IdTipoCandidatura { get; set; }

        public int IdEstado { get; set; }
    }
}
