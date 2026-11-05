using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
SchoolCalendar:
* SchoolCalendarId   int
* TotalSemesters     int          (valor de 1 a 100; número de semestres do calendário)
* StartSpecialExam   DateTime     (início dos exames especiais)
* EndSpecialExam     DateTime     (fim dos exames especiais)
* AcademicYearId     int          (FK → AcademicYear)
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
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(1, 100, ErrorMessage = "O {0} deve estar entre {1} e {2}")]
    [Display(Name = "Quantidade de Semestres")]
    public int TotalSemesters { get; set; }

    // --- Special Exams ---

    // START DATE OF SPECIAL EXAMS:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início de Exames da Época Especial")]
    public DateTime StartSpecialExam { get; set; }

    // END DATE OF SPECIAL EXAMS:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim de Exames da Época Especial")]
    public DateTime EndSpecialExam { get; set; }

    // --- Foreign Key & Navigation Property ---

    // ACADEMIC YEAR ID:
    [Display(Name = "Ano Letivo ID")]
    public int AcademicYearId { get; set; }

    public AcademicYear? AcademicYear { get; set; }
}