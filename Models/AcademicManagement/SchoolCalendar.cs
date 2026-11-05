using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnivManagement.Models.AcademicManagement
{
    public class SchoolCalendar
    {

        public int SchoolCalendarId { get; set; }

        // --- Academic Year Dates ---
        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Academic year start date")]
        public DateTime StartYear { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Academic year end date")]
        public DateTime EndYear { get; set; }

        // --- Semester 1 ---
        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 start date")]
        public DateTime StartSemesterS1 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 end date")]
        public DateTime EndSemesterS1 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 frequency exams start date")]
        public DateTime StartFreqExamS1 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 frequency exams end date")]
        public DateTime EndFreqExamS1 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 resit exams start date")]
        public DateTime StartResitExamS1 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 resit exams end date")]
        public DateTime EndResitExamS1 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 special exams start date")]
        public DateTime StartSpecialExamS1 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 1 special exams end date")]
        public DateTime EndSpecialExamS1 { get; set; }

        // --- Semester 2 ---
        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 start date")]
        public DateTime StartSemesterS2 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 end date")]
        public DateTime EndSemesterS2 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 frequency exams start date")]
        public DateTime StartFreqExamS2 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 frequency exams end date")]
        public DateTime EndFreqExamS2 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 resit exams start date")]
        public DateTime StartResitExamS2 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 resit exams end date")]
        public DateTime EndResitExamS2 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 special exams start date")]
        public DateTime StartSpecialExamS2 { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Semester 2 special exams end date")]
        public DateTime EndSpecialExamS2 { get; set; }

        // --- Foreign Key & Navigation Property ---
        [Display(Name = "Academic year")]
        public int AcademicYearId { get; set; }

        public AcademicYear? AcademicYear { get; set; }
    }
}
