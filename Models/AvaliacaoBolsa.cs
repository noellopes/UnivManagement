using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class AvaliacaoBolsa
    {
        [Key]
        public int Id_Avaliacao { get; set; }

        [Required]
        public int Id_CandidaturaBolsa { get; set; }

        // Navigation property for Entity Framework
        [ForeignKey("Id_CandidaturaBolsa")]
        public CandidaturaBolsa? CandidaturaBolsa { get; set; }

        public int Id_Tecnico { get; set; }

        public DateTime Data_Avaliacao { get; set; } = DateTime.Now;

        public string? Parecer { get; set; }

        public string? Resultado { get; set; }

        public string? Observacoes { get; set; }

        public void Registar() { }
        public void EmitirParecer() { }
        public void Atualizar() { }
        public byte[] GerarRelatorio()
        {
            return Array.Empty<byte>();
        }
    }
}