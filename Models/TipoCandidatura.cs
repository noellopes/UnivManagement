
using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class TipoCandidatura
    {
        public int TipoCandidaturaId { get; set; }

        // The application type name is required.
        [Required(ErrorMessage = "Please enter the application type {0}")]
        [StringLength(200, ErrorMessage = "The application type's {0} cannot exceed {1} characters.")]
        public required string Designacao { get; set; }

        // The application type description is optional.
        public string? Descricao { get; set; }
    }
}
