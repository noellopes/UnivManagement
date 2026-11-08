using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class TipoDocumento
    {
        public int TipoDocumentoId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} não pode ter mais de {1} caracteres.")]
        [Display(Name = "Nome do tipo")]
        public required string NomeTipo { get; set; }

        [Display(Name = "Obrigatório")]
        public bool Obrigatorio { get; set; }
    }
}
