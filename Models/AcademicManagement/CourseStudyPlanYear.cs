using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Course? Course { get; set; }

        // --- Foreign Key StudyPlan ---
        [Required(ErrorMessage = "The {0} is required")]
        [Display(Name = "Study Plan")]
        public int StudyPlanId { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public StudyPlan? StudyPlan { get; set; }

        // --- Foreign Key AcademicYear ---
        [Required(ErrorMessage = "The {0} is required")]
        [Display(Name = "Academic Year")]
        public int AcademicYearId { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public AcademicYear? AcademicYear { get; set; }
    }
}