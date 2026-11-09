using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Número Mecanográfico")]
        public int NumeroMecanografico { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Departamento")]
        [StringLength(50, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        public required string Departamento { get; set; }
    }
}