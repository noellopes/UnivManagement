using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement
{
    public class CourseState
    {
        public int CourseStateId { get; set; }


        /* NAME — nome legível (mostrado na UI) */
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres.")]
        [RegularExpression(@"^[A-Z]+(?:[ _][A-Z]+)*$", ErrorMessage = "O {0} só pode conter letras maiúsculas, espaços e underscores.")]
        [Display(Name = "Nome")]
        public required string Name { get; set; }

        /* DESCRIPTION — opcional */
        [StringLength(200, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
    }
}
