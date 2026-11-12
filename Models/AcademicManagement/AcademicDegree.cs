using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement
{
    public class AcademicDegree
    {

        [Display(Name = "ID")]
        public int AcademicDegreeId { get; set; }


        /* ACADEMIC DEGREE NAME */
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ]+(?:[ -][A-Za-zÀ-ÿ]+)*$", ErrorMessage = "O {0} só pode conter letras, espaços e hífenes.")]
        [Display(Name = "Grau Académico")]
        public required string Name { get; set; }


        /* ACADEMIC DEGREE DESCRIPTION */
        [StringLength(100, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }

    }
}
