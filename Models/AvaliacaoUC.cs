using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class AvaliacaoUC
    {
        public int AvaliacaoUCId { get; set; }

        [Required(ErrorMessage = "Por favor, escolha a {0}.")]
        [Display(Name = "Época")]
        public int TipoAvaliacaoId { get; set; }

        public TipoAvaliacao? TipoAvaliacao { get; set; }

        [Required(ErrorMessage = "Por favor, escolha a {0}.")]
        [Display(Name = "Unidade Curricular")]
        public int UcId { get; set; }

        [Required(ErrorMessage = "Por favor, escolha o {0}.")]
        [Display(Name = "Tipo de Avaliação")]
        public required string Tipo { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [Display(Name = "Avaliação")]
        public required string Designacao { get; set; }

        [Required(ErrorMessage = "Por favor, indique a {0}.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data da Avaliação")]
        public DateTime Data { get; set; }
    }
}
