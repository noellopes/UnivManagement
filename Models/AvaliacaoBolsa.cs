using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class AvaliacaoBolsa
    {
        [Key]
        public int AvaliacaoBolsaId { get; set; }

        [Required(ErrorMessage = "O número da candidatura é obrigatório.")]
        [Display(Name = "Candidatura")]
        public int CandidaturaBolsaId { get; set; }
        // Quando a classe CandidaturaBolsa for criada, pode descomentar:
        // [ForeignKey("CandidaturaBolsaId")]
        // public CandidaturaBolsa? CandidaturaBolsa { get; set; }

        [Required(ErrorMessage = "Selecione o técnico responsável.")]
        [Display(Name = "Técnico")]
        public int TecnicoId { get; set; }

        [ForeignKey("TecnicoId")]
        public Tecnico? Tecnico { get; set; }

        [Required(ErrorMessage = "A data da avaliação é obrigatória.")]
        [Display(Name = "Data da avaliação")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime DataAvaliacao { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O resultado da avaliação é obrigatório.")]
        [StringLength(50)]
        [Display(Name = "Resultado")]
        public string Resultado { get; set; } = string.Empty;

        [Required(ErrorMessage = "A fundamentação do parecer é obrigatória.")]
        [StringLength(2000, ErrorMessage = "O parecer não pode exceder 2000 caracteres.")]
        [Display(Name = "Parecer técnico")]
        public string Parecer { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "As observações não podem exceder 1000 caracteres.")]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }
    }
}