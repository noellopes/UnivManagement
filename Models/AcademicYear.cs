using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models;

/*
AcademicYear:
* IdAcademicYear    int
* StartYear         decimal     (valor de 2000 a 9999)
* EndYear           decimal     (valor de 2000 a 9999)
*/

/// <summary>
/// Academic Year model class from Academic Management domain.
/// </summary>
public class AcademicYear {
    /* ID: */
    [Key]
    public required int IdAcademicYear { get; set; }

    /* START YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(2000, 9999, ErrorMessage = "Ano de início deve ter o máximo de 4 dígitos | Start year must have the maximum of 9 digits.")]
    [Display(Name = "Início")]
    public required decimal StartYear { get; set; }

    /* END YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(2000, 9999, ErrorMessage = "Ano de fim deve ter o máximo de 4 dígitos | End year must have the maximum of 9 digits.")]
    [Display(Name = "Fim")]
    public required decimal EndYear { get; set; }
}