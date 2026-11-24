using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Avaliacao
    {
        [Key]
        public int IdAvaliacao { get; set; }

        public DateTime DataAvaliacao { get; set; }
        public double Classificacao { get; set; }
        public string Observacoes { get; set; }

        [ForeignKey("Estagio")]
        public int IdEstagio { get; set; }
        public Estagio Estagio { get; set; }

        [ForeignKey("OrientadorEstagio")]
        public int IdOrientador { get; set; }
        public OrientadorEstagio OrientadorEstagio { get; set; }

        public void RegistrarAvaliacao() { }
        public void ConsultarAvaliacao() { }
        public void AtualizarAvaliacao() { }
    }
}
