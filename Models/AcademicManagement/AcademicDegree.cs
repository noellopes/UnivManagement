using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement
{
    public class AcademicDegree
    {

        public int AcademicDegreeId { get; set; }


        /* ACADEMIC DEGREE NAME */
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "O {0} deve ter entre {2} e {1} caracteres.")]
        [Display(Name = "Nome do Grau Académico")]
        public required string Name { get; set; }


        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Range(5, 8, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
        [Display(Name = "Nível")]
        public required int Level { get; set; }


        /* ACADEMIC DEGREE DESCRIPTION */
        [StringLength(100, ErrorMessage = "O {0} não pode exceder {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string? Description { get; set; }

    }
}
