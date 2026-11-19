using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Models
{
    public class InscAval
    {
        public int InscAvalId { get; set; }

        [Required(ErrorMessage = "Por favor, escolha a {0}.")]
        [Display(Name = "Avaliação")]
        public int AvaliacaoUCId { get; set; }

        [ForeignKey("AvaliacaoUCId")]
        public AvaliacaoUC? AvaliacaoUC { get; set; }

        [Required(ErrorMessage = "Por favor, escolha a {0}.")]
        [Display(Name = "Inscrição na UC")]
        public int InscrUcId { get; set; }

        [ForeignKey("InscrUcId")]
        public InscrUC? InscrUC { get; set; }

        [Display(Name = "Pago")]
        public bool Pago { get; set; }
    }
}