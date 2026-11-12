using System.ComponentModel.DataAnnotations;

using UnivManagement.Models; // Import necessario ter acesso classe Docente de outro namespace


namespace UnivManagement.Models.AcademicManagement
{
    public class Course
    {
        [Display(Name = "Course ID")]
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
        [StringLength(5, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        [RegularExpression(@"^[A-Z0-9]+(?: [A-Z0-9]+)*$", ErrorMessage = "The {0} can only contain uppercase letters, numbers and single spaces.")]
        [Display(Name = "Acronym")]
        public required string Acronym { get; set; }


        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public DateOnly? CreationDate { get; set; }


        [StringLength(1000, ErrorMessage = "The {0} cannot be longer than {1} characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }



        // FOREIGN KEY and Navigation Property - ACADEMIC DEGREE 
        [Display(Name = "Grau Académico")]
        public int AcademicDegreeId { get; set; }
        public AcademicDegree? AcademicDegree { get; set; }


        // FOREIGN KEY and Navigation Property - COURSE STATE
        [Display(Name = "Course State")]
        public int CourseStateId { get; set; }
        public CourseState? CourseState { get; set; }


        // --- Foreign Key para o Diretor de Curso (tabela do grupo do Simão (Docente)) ---
        [Display(Name = "Course Director")]
        public int DocenteId { get; set; }

        public Docente? Docente { get; set; }

    }
}
