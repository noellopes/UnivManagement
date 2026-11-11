using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class RelatorioEstagio
    {
        [Key]
        public int IdRelatorio { get; set; }

        public DateTime DataEntrega { get; set; }
        public string Descricao { get; set; }
        public string Ficheiro { get; set; }

        [ForeignKey("Estagio")]
        public int IdEstagio { get; set; }
        public Estagio Estagio { get; set; }

        public void SubmeterRelatorio() { }
        public void ConsultarRelatorio() { }
    }
}
