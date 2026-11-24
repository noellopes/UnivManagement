using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Models.AcademicManagement;

/*
InscrCourse:
 * InscrCourseId     int
 * DateInscr         DateOnly
 * DateGrad          DateOnly?   (optional; null while not graduated)
 * FinalAvgGrade     decimal     (value between 0 and 20)
 * CourseFeePaid     bool
 * CourseStudyPlanYearId (FK)  int         (references CourseStudyPlanYear)
 * StudentId (FK)         int         (references Student)
 * AcademicYearId (FK)    int         (references AcademicYear)
 * StateInscCourseId (FK)  int         (references StateInscrCourse)
 */

/// <summary>
/// Inscription Course model class from Academic Management domain.
/// Represents a student's inscription in a course for a given academic year,
/// including its state, dates, final grade, and tuition payment status.
/// </summary>
public class InscrCourse
{
    /* ID: */
    [Display(Name = "ID")]
    public int InscrCourseId { get; set; }


    /* INSCRIPTION DATE: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Inscrição")]
    public required DateOnly DateInscr { get; set; }

    /* GRADUATION DATE (OPTIONAL): */
    [DataType(DataType.Date)]
    [Display(Name = "Data de Graduação")]
    public DateOnly? DateGrad { get; set; }

    /* FINAL COURSE GRADE, also COURSE AVERAGE: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(typeof(decimal), "0", "20", ErrorMessage = "{0} deve estar entre {1} e {2}.")]
    [Precision(4, 2)]
    [Display(Name = "Média Final de Curso")]
    public required decimal FinalAvgGrade { get; set; }

    /* COURSE FEE PAYMENT STATE: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Propina Paga")]
    public required bool CourseFeePaid { get; set; }

    /* Foreign Key Relationships / Navigation Properties: */

    /* REF FK_ID COURSE PLAN YEAR: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "ID Curso Plano Ano")]
    public int CourseStudyPlanYearId { get; set; }
    //public CourseStudyPlanYear? CourseStudyPlanYear { get; set; }

    /* REF FK_ID STUDENT: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "ID Aluno")]
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    /* REF FK_ID ACADEMIC YEAR (CURRENT YEAR OF THE INSCRIPTION): */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "ID Ano Letivo")]
    public int AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }


    /* REF FK_ID of State Inscriptin Course */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "ID Estado da Inscrição")]
    public int StateInscCourseId { get; set; }
    public StateInscCourse? StateInscCourse { get; set; }
}