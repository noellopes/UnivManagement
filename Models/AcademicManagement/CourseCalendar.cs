using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement;

/// <summary>
/// Historical association between a Course and a SchoolCalendar.
/// Records must be preserved when the course is associated with a different calendar later.
/// </summary>
public class CourseCalendar
{
    public int CourseCalendarId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Curso")]
    public int CourseId { get; set; }

    public Course? Course { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O {0} é obrigatório.")]
    [Display(Name = "Calendário Escolar")]
    public int SchoolCalendarId { get; set; }

    public SchoolCalendar? SchoolCalendar { get; set; }
}
