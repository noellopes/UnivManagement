using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Carreira
    {
        [Key]
        public int IdCarreira { get; set; }

        [ForeignKey("Estudante")]
        public int IdAluno { get; set; }
        public Estudante? Estudante { get; set; }

        public string CargoAtual { get; set; } = string.Empty;
        public string EmpresaAtual { get; set; } = string.Empty;
        public string Competencias { get; set; } = string.Empty;
        public string Certificacoes { get; set; } = string.Empty;
        public string HistoricoProfissional { get; set; } = string.Empty;
    }
}