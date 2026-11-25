using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Candidaturas
    {
        [Key]
        public int IdCandidatura { get; set; }

        [ForeignKey(nameof(Estudante))]
        public int IdAluno { get; set; }
        public Estudante? Estudante { get; set; }

        [ForeignKey(nameof(Oferta))]
        public int IdOferta { get; set; }
        public Ofertas? Oferta { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCandidatura { get; set; }

        public string Estado { get; set; } = string.Empty;
        public string Curriculo { get; set; } = string.Empty;
    }
}
