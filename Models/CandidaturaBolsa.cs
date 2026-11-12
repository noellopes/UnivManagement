using System;
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class CandidaturaBolsa
    {
        [Key]
        public int Id_CandidaturaBolsa { get; set; }

        public string NomeEstudante { get; set; }

        public double Classificacao { get; set; }

        public DateTime DataSubmissao { get; set; }

        public DateTime? DataDecisao { get; set; }

        public string Estado { get; set; } = "Pendente";
    }
}