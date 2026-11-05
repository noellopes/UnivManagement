using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/*
Semester:
* SemesterId         int
* PosNum             int          (value between 1 and 100; unique per SchoolCalendarId)
* StartSemester      DateTime
* EndSemester        DateTime
* StartFreqExam      DateTime
* EndFreqExam        DateTime
* StartResitExam     DateTime
* EndResitExam       DateTime
* SchoolCalendarId   int

Nota: existe um índice único composto (SchoolCalendarId, NumSemester),
de modo a impedir dois semestres com o mesmo número dentro do mesmo calendário.
*/

/// <summary>
/// Semester model class from Academic Management domain.
/// Represents a single semester (or term) within a School Calendar,
/// including its teaching period and exam windows.
/// </summary>
public class Semester {
    // SEMESTER ID:
    public int SemesterId { get; set; }

    // POSITION NUMBER OF A SEMESTER:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [Range(1, 100, ErrorMessage = "O {0} deve estar entre {1} e {2}")]
    [Display(Name = "Número do Semestre")]
    public int PosNum { get; set; }

    // START DATE OF THE SEMESTER:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início do Semestre")]
    public DateTime StartSemester { get; set; }

    // END DATE OF THE SEMESTER:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim do Semestre")]
    public DateTime EndSemester { get; set; }

    // START DATE OF THE FREQUENCY EXAMS:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início de Exames de Frequências")]
    public DateTime StartFreqExam { get; set; }

    // END DATE OF THE FREQUENCY EXAMS:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim de Exames de Frequências")]
    public DateTime EndFreqExam { get; set; }

    // START DATE OF THE RESIT EXAMS:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Início de Exames Normais")]
    public DateTime StartResitExam { get; set; }

    // END DATE OF THE RESIT EXAMS:
    [Required(ErrorMessage = "O {0} é obrigatório.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fim de Exames Normais")]
    public DateTime EndResitExam { get; set; }

    // SCHOOL CALENDAR ID:
    [Display(Name = "Calendário Escolar ID")]
    public int SchoolCalendarId { get; set; }
    public SchoolCalendar? SchoolCalendar { get; set; }

}