using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class TipoAvaliacao
    {
        public int TipoAvaliacaoId { get; set; }

        [Required(ErrorMessage = "Por favor, insira o {0}.")]
        [StringLength(50, ErrorMessage = "O campo {0} não pode exceder {1} carateres.")]
        [Display(Name = "Tipo de Prova")]
        public required string TipoProva { get; set; }
    }
}
