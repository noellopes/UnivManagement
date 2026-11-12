using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Candidatura
    {
        [Key]
        public int IdCandidatura { get; set; }

        public DateTime DataCandidatura { get; set; }
        public string Estado { get; set; }
        public string Curriculo { get; set; }

        [ForeignKey("AntigoAluno")]
        public int IdAluno { get; set; }
        public AntigoAluno AntigoAluno { get; set; }

        [ForeignKey("Oferta")]
        public int IdOferta { get; set; }
        public Oferta Oferta { get; set; }

        public void Submeter() { }
        public void ConsultarEstado() { }
        public void AtualizarEstado() { }
    }
}
