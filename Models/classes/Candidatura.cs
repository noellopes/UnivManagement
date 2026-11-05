using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Candidatura
    {
        [Key]
        public int IdCandidatura { get; set; }

        [ForeignKey("Estudante")]
        public int IdAluno { get; set; }
        public Estudante? Estudante { get; set; }

        [ForeignKey("Oferta")]
        public int IdOferta { get; set; }
        public Oferta? Oferta { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCandidatura { get; set; }

        public string Estado { get; set; } = string.Empty;
        public string Curriculo { get; set; } = string.Empty;
    }
}