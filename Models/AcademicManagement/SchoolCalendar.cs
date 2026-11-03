using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
SchoolCalendar:
* SchoolCalendarId   int
* NumSemesters       int          (valor de 1 a 100; número de semestres do calendário)
* StartSpecialExam   DateTime     (início dos exames especiais)
* EndSpecialExam     DateTime     (fim dos exames especiais)
* AcademicYearId     int          (FK → AcademicYear)

Nota: o calendário pode conter dois ou mais semestres (NumSemesters ≥ 1).
Cada semestre associado é controlado pelo modelo Semester, que garante
a unicidade do par (SchoolCalendarId, NumSemester).
*/

/// <summary>
/// SchoolCalendar model class from Academic Management domain.
/// Defines the exam period configuration for a given Academic Year,
/// which is later subdivided into Semesters.
/// </summary>
public class SchoolCalendar {
    // SCHOOL CALENDAR ID:
    public int SchoolCalendarId { get; set; }

    // TOTAL NUMBER OF SEMESTERS:
    [Required(ErrorMessage = "O {0} é obrigatório. | The {0} is required.")]
    [Range(1, 100, ErrorMessage = "O {0} deve estar entre {1} e {2} | The {0} must be between {1} and {2}.")]
    [Display(Name = "Quantidade de Semestres")]
    public int TotalSemesters { get; set; }

    // --- Special Exams ---

    // START DATE OF SPECIAL EXAMS:
    [Required(ErrorMessage = "The {0} is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Semester 2 special exams start date")]
    public DateTime StartSpecialExam { get; set; }

    // END DATE OF SPECIAL EXAMS:
    [Required(ErrorMessage = "The {0} is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Semester 2 special exams end date")]
    public DateTime EndSpecialExam { get; set; }

    // --- Foreign Key & Navigation Property ---
    // ACADEMIC YEAR ID:
    [Display(Name = "Academic year")]
    public int AcademicYearId { get; set; }

    public AcademicYear? AcademicYear { get; set; }
}