using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
InscrUC:
 * InscrUCId         int
 * UcId              int         (references UC)
 * InscrCourseId     int         (references InscrCourse)
 * AcademicYearId    int         (references AcademicYear)
 * StateInscr        string      (ONGOING | CANCELED | CONCLUDED)
 * DateInscr         DateOnly
 * DateEnd           DateOnly?   (optional; null while not concluded)
 * Grade             decimal     (value between 0 and 20)
 */

/// <summary>
/// Inscription UC model class from Academic Management domain.
/// Represents a student's inscription in a specific UC for a given academic year,
/// including its state, dates, and final grade.
/// </summary>
public class InscrUC {
    /* ID: */
    [Display(Name = "ID")]
    public required int InscrUCId { get; set; }

    /* REF ID UC: */
    [Display(Name = "ID UC")]
    public required int UcId { get; set; }

    /* REF ID INSCR COURSE: */
    [Display(Name = "ID Inscrição no Curso")]
    public required int InscrCourseId { get; set; }

    /* REF ID ACADEMIC YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "ID Ano Letivo")]
    public required int AcademicYearId { get; set; }

    /* INSCRIPTION STATE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [RegularExpression("^(ONGOING|CANCELED|CONCLUDED)$", ErrorMessage = "O {0} deve ser ONGOING, CANCELED ou CONCLUDED. | The {0} must be ONGOING, CANCELED or CONCLUDED.")]
    [Display(Name = "Estado da Inscrição")]
    public required string StateInscr { get; set; }

    /* INSCRIPTION DATE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Inscrição")]
    public required DateOnly DateInscr { get; set; }

    /* CONCLUSION DATE (OPTIONAL): */
    [DataType(DataType.Date)]
    [Display(Name = "Data de Conclusão")]
    public DateOnly? DateEnd { get; set; }

    /* FINAL GRADE OF THE UC: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(typeof(decimal), "0", "20", ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Classificação")]
    public required decimal Grade { get; set; }
}