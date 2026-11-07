using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Carreira
    {
        [Key]
        public int IdCarreira { get; set; }

        public string CargoAtual { get; set; }
        public string EmpresaAtual { get; set; }
        public string Competencias { get; set; }
        public string Certificacoes { get; set; }
        public string HistoricoProfissional { get; set; }

        [ForeignKey("AntigoAluno")]
        public int IdAluno { get; set; }
        public AntigoAluno AntigoAluno { get; set; }

        public void AtualizarCarreira() { }
        public void ConsultarHistorico() { }
        public void AdicionarCompetencia() { }
        public void AdicionarCertificacao() { }
    }
}
