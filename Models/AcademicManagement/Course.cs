using System.ComponentModel.DataAnnotations;

using UnivManagement.Models; // Import necessario ter acesso classe Docente de outro namespace


namespace UnivManagement.Models.AcademicManagement
{
    public class Course
    {
        
        public int CourseId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [RegularExpression(@"^[A-Z0-9]{3,6}$", ErrorMessage = "The {0} must be between 3 and 6 alphanumeric characters (letters and numbers only).")]
        [Display(Name = "Course Code")]
        public required string Code { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(100, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Course Name")]
        public required string Name { get; set; }


        [Required(ErrorMessage = "The {0} is required.")]
        [RegularExpression(@"^(CTeSP|Licenciatura|Mestrado|Doutoramento)$", ErrorMessage = "The {0} must be 'CTeSP', 'Licenciatura', 'Mestrado', or 'Doutoramento'.")]
        [Display(Name = "Academic Degree")]
        public required string AcademicDegree { get; set; }


        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        [Display(Name = "Acronym")]
        public required string Acronym { get; set; }


        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }


        [Required(ErrorMessage = "The {0} is required.")]
        [StringLength(20, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Course State")]
        public required string CourseState { get; set; }


        [StringLength(1000, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }


        // --- Foreign Key para o Calendário Escolar ---
        [Display(Name = "School Calendar")]
        public int SchoolCalendarId { get; set; }

        public SchoolCalendar? SchoolCalendar { get; set; }


        // --- Foreign Key para o Diretor de Curso (tabela do grupo do Simão (Docente)) ---
        [Display(Name = "Course Director")]
        public int DocenteId { get; set; }

        public Docente? Docente { get; set; }

    }
}
