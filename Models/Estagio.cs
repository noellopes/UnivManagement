using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Estagio
    {
        [Key]
        public int IdEstagio { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Duracao { get; set; }
        public string Estado { get; set; }
        public string PlanoTrabalhos { get; set; }

        [ForeignKey("AntigoAluno")]
        public int IdAluno { get; set; }
        public AntigoAluno AntigoAluno { get; set; }

        [ForeignKey("Empregador")]
        public int IdEmpregador { get; set; }
        public Empregador Empregador { get; set; }

        [ForeignKey("GestorEstagio")]
        public int IdGestor { get; set; }
        public GestorEstagio GestorEstagio { get; set; }

        [ForeignKey("OrientadorEstagio")]
        public int? IdOrientador { get; set; }
        public OrientadorEstagio OrientadorEstagio { get; set; }

        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public ICollection<RelatorioEstagio> Relatorios { get; set; } = new List<RelatorioEstagio>();

        public void IniciarEstagio() { }
        public void AtualizarEstado() { }
        public void ConsultarProgresso() { }
        public void ConcluirEstagio() { }
    }
}
