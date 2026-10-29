using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models
{
    public class Teacher : ApplicationUser
    {
        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [Display(Name = "Número Mecanográfico")]
        public int NumeroMecanografico { get; set; }

        [Required(ErrorMessage = "Por favor, indique o {0}.")]
        [StringLength(100, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Departamento")]
        public required string Departamento { get; set; }
    }
}