using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [Display(Name = "Submissão")]
        public int SubmissaoTrabalhoId { get; set; }

        // public SubmissaoTrabalho? SubmissaoTrabalho { get; set; } // Descomentar quando o Diogo criar a classe

        [Required(ErrorMessage = "Por favor, escolha o {0}.")]
        [Display(Name = "Docente")]
        public int DocenteId { get; set; }

        public Docente? Docente { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data do Feedback")]
        public DateTime DataFeedback { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "Por favor, insira a {0}.")]
        [Range(0, 20, ErrorMessage = "A {0} tem de estar entre {1} e {2}.")]
        [Display(Name = "Nota")]
        public float Nota { get; set; }
    }
}