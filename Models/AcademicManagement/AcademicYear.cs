using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
AcademicYear:
* AcademicYearId    int
* StartYear         int     (value between 2000 and 9999)
* EndYear           int     (value between 2000 and 9999)
*/

/// <summary>
/// Academic Year model class from Academic Management domain.
/// </summary>
public class AcademicYear {
    /* ID: */
    public int AcademicYearId { get; set; }

    /* START YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(2000, 9999, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Ano de Início")]
    public int StartYear { get; set; }

    /* END YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(2000, 9999, ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Display(Name = "Ano de Fim")]
    public int EndYear { get; set; }
}