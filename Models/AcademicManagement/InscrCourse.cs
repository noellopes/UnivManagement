using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
InscrCourse:
 * InscrCourseId     int
 * CoursePlanYearId  int         (references CoursePlanYear)
 * StudentId         int         (references Student)
 * AcademicYearId    int         (references AcademicYear)
 * StateInscr        string      (UNPAID | ONGOING | CANCELED | CONCLUDED)
 * DateInscr         DateOnly
 * DateGrad          DateOnly?   (optional; null while not graduated)
 * FinalAvgGrade     decimal     (value between 0 and 20)
 * CourseFeePaid     bool
 */

/// <summary>
/// Inscription Course model class from Academic Management domain.
/// Represents a student's inscription in a course for a given academic year,
/// including its state, dates, final grade, and tuition payment status.
/// </summary>
public class InscrCourse {
    /* ID: */
    [Display(Name = "ID")]
    public required int InscrCourseId { get; set; }

    /* REF ID COURSE PLAN YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "ID Curso Plano Ano")]
    public required int CoursePlanYearId { get; set; }

    /* REF ID STUDENT: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "ID Aluno")]
    public required int StudentId { get; set; }

    /* REF ID ACADEMIC YEAR (CURRENT YEAR OF THE INSCRIPTION): */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "ID Ano Letivo")]
    public required int AcademicYearId { get; set; }

    /* INSCRIPTION STATE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [RegularExpression("^(UNPAID|ONGOING|CANCELED|CONCLUDED)$", ErrorMessage = "O {0} deve ser UNPAID, ONGOING, CANCELED ou CONCLUDED. | The {0} must be UNPAID, ONGOING, CANCELED or CONCLUDED.")]
    [Display(Name = "Estado da Inscrição")]
    public required string StateInscr { get; set; }

    /* INSCRIPTION DATE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Inscrição")]
    public required DateOnly DateInscr { get; set; }

    /* GRADUATION DATE (OPTIONAL): */
    [DataType(DataType.Date)]
    [Display(Name = "Data de Graduação")]
    public DateOnly? DateGrad { get; set; }

    /* FINAL COURSE GRADE, also COURSE AVERAGE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(typeof(decimal), "0", "20", ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} should be between {1} and {2}.")]
    [Display(Name = "Nota Média Final")]
    public required decimal FinalAvgGrade { get; set; }

    /* COURSE FEE PAYMENT STATE: */
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Display(Name = "Propina Paga")]
    public required bool CourseFeePaid { get; set; }
}