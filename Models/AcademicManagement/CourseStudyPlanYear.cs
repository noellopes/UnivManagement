using System.ComponentModel.DataAnnotations;

namespace UnivManagement.Models.AcademicManagement
{
    public class CourseStudyPlanYear
    {
        // Chave Primária
        public int CourseStudyPlanYearId { get; set; }

        // --- Foreign Key Course ---
        [Required(ErrorMessage = "The {0} is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }


        // --- Foreign Key StudyPlan ---
        [Required(ErrorMessage = "The {0} is required")]
        [Display(Name = "Study Plan")]
        public int StudyPlanId { get; set; }
        public StudyPlan? StudyPlan { get; set; }


        // --- Foreign Key AcademicYear ---
        [Required(ErrorMessage = "The {0} is required")]
        [Display(Name = "Academic Year")]
        public int AcademicYearId { get; set; }
        public AcademicYear? AcademicYear { get; set; }

    }
}
