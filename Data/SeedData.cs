using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateCourseStates(db);

        }

        private static void PopulateCourseStates(UniversityDbContext dbContext)
        {
            if (dbContext.CourseState.Any()) return;

            var courseStates = new List<CourseState>
        {
            new CourseState { Name = "DRAFT", Description = "Curso em preparação, ainda não publicado."},
            new CourseState { Name = "ACTIVE", Description = "Curso a decorrer normalmente."},
            new CourseState { Name = "SUSPENDED", Description = "Curso temporariamente suspenso."},
            new CourseState { Name = "CONCLUDED", Description = "Curso terminado, sem novas inscrições."},
            new CourseState { Name = "ARCHIVED", Description = "Curso arquivado historicamente."}
        };

            dbContext.CourseState.AddRange(courseStates);
            dbContext.SaveChanges();
        }
    }
}
