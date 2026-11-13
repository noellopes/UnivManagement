using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models
{
    public class Tecnico
    {
        
        public int TecnicoId { get; set; }

        // Technician's name with required validation and length limits
        [Required(ErrorMessage = "The name is required.")]
        [StringLength(200, ErrorMessage = "The name cannot exceed {1} characters.")]
        public required string Nome { get; set; }

        // Technician's email with email format validation
        [Required(ErrorMessage = "The email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(150, ErrorMessage = "The email cannot exceed {1} characters.")]
        public required string Email { get; set; }

        // Foreign key for the Department
        public int DepartamentoId { get; set; }

        // Navigation property linking to the Departamento model
        [ForeignKey("DepartamentoId")]
        public Departamento? Departamento { get; set; }
    }
}