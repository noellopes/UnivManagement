using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Models.AcademicManagement;

/*
InscrUC:
 * InscrUCId         int
 * DateInscr         DateOnly
 * DateEnd           DateOnly?   (optional; null while not concluded)
 * Grade             decimal     (value between 0 and 20)
 * UcId (FK)              int         (references UC)
 * InscrCourseId (FK)     int         (references InscrCourse)
 * AcademicYearId (FK)    int         (references AcademicYear)
 * StateInscrUcId (FK)  int         (references StateInscrUc)
 */

/// <summary>
/// Inscription UC model class from Academic Management domain.
/// Represents a student's inscription in a specific UC for a given academic year,
/// including its state, dates, and final grade.
/// </summary>
public class InscrUC {
    /* ID: */
    [Display(Name = "ID")]
    public int InscrUCId { get; set; }

    /* INSCRIPTION DATE: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Inscrição")]
    public required DateOnly DateInscr { get; set; }

    /* CONCLUSION DATE (OPTIONAL): */
    [DataType(DataType.Date)]
    [Display(Name = "Data de Conclusão")]
    public DateOnly? DateEnd { get; set; }

    /* FINAL GRADE OF THE UC: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(typeof(decimal), "0", "20", ErrorMessage = "O {0} deve estar entre {1} e {2}.")]
    [Precision(4, 2)]
    [Display(Name = "Classificação")]
    public required decimal Grade { get; set; }

    // ===== FK / Navigation property =====

    /* REF ID UC: */
    [Display(Name = "ID UC")]
    public int UcId { get; set; }
    public UC? Uc { get; set; }

    /* REF ID INSCR COURSE: */
    [Display(Name = "ID Inscrição no Curso")]
    public int InscrCourseId { get; set; }
    public InscrCourse? InscrCourse { get; set; }

    /*
     * estava a gerar redundancia e dava erro ao criar a bd pois inscrCourse já tem AcademicYearId, 
     * e InscrUC também tem AcademicYearId, e não é necessário ter ambos.
     * 
    // REF ID ACADEMIC YEAR: 
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "ID Ano Letivo")]
    public int AcademicYearId { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    */

    /* INSCRIPTION STATE: */
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "ID Estado da Inscrição")]
    public int StateInscrUcId { get; set; }
    public StateInscrUc? StateInscrUc { get; set; } 
}