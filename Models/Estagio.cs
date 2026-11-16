using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class Estagio
    {
        [Key]
        public int EstagioId { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Duracao { get; set; }
        public string Estado { get; set; } = "";
        public string PlanoTrabalhos { get; set; } = "";

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }
        public Empresa? Empresa { get; set; }

        [ForeignKey("GestorEstagio")]
        public int? GestorId { get; set; }
        public GestorEstagio? GestorEstagio { get; set; }

        [ForeignKey("OrientadorEstagio")]
        public int? OrientadorId { get; set; }
        public OrientadorEstagio? OrientadorEstagio { get; set; }

        public ICollection<Avaliacao> Avaliacoes { get; set; } = new List<Avaliacao>();
        public ICollection<RelatorioEstagio> Relatorios { get; set; } = new List<RelatorioEstagio>();

        public void IniciarEstagio() { }
        public void AtualizarEstado() { }
        public void ConsultarProgresso() { }
        public void ConcluirEstagio() { }
    }
}