using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement
{

    /// <summary>
    /// Academic degree reference entity from Academic Management domain.
    /// Records are predefined and must be seeded at database initialization.
    /// </summary>
    public class AcademicDegree
    {

        public int AcademicDegreeId { get; set; }


        /* ACADEMIC DEGREE NAME */
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O {0} não pode ter mais de {1} caracteres.")]
        [RegularExpression(@"^[\p{L} _]+$", ErrorMessage = "O {0} deve conter apenas letras, espaços e underscores.")]
        [Display(Name = "Grau Académico")]
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
