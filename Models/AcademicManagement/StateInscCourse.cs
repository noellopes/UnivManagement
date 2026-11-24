using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement
{
    public class StateInscCourse
    {
        /* PRIMARY KEY */
        [Display(Name = "ID")]
        public int StateInscCourseId { get; set; }


        /* STATE */
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [RegularExpression(@"^[A-Z ]+$", ErrorMessage = "O {0} só pode conter letras maiúsculas.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres.")]
        [Display(Name = "Estado da Inscrição")]
        public required string Name { get; set; }


        /* STATE DESCRIPTION */
        [StringLength(200, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
    }
}
