using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateCourseStates(db);
            PopulateAcademicDegrees(db);
            
        }

        private static void PopulateCourseStates(UniversityDbContext dbContext)
        {
            if (dbContext.CourseState.Any()) return;

            var courseStates = new List<CourseState>
            {
                new CourseState { Name = "DRAFT",     Description = "Curso em preparação, ainda não publicado." },
                new CourseState { Name = "ACTIVE",    Description = "Curso a decorrer normalmente." },
                new CourseState { Name = "SUSPENDED", Description = "Curso temporariamente suspenso." },
                new CourseState { Name = "CONCLUDED", Description = "Curso terminado, sem novas inscrições." },
                new CourseState { Name = "ARCHIVED",  Description = "Curso arquivado historicamente." }
            };

            dbContext.CourseState.AddRange(courseStates);
            dbContext.SaveChanges(); 
        }

        private static void PopulateAcademicDegrees(UniversityDbContext dbContext)
        {
            if (dbContext.AcademicDegree.Any()) return;

            var academicDegrees = new List<AcademicDegree>
            {
                new AcademicDegree { Name = "Licenciatura",  Description = "Grau de licenciado (3–4 anos)." },
                new AcademicDegree { Name = "Mestrado",      Description = "Grau de mestre (2 anos)." },
                new AcademicDegree { Name = "Doutoramento",  Description = "Grau de doutor." },
                new AcademicDegree { Name = "Pós-Graduação", Description = "Especialização pós-licenciatura." },
                new AcademicDegree { Name = "CTeSP",         Description = "Curso Técnico Superior Profissional." }
            };

            dbContext.AcademicDegree.AddRange(academicDegrees);
            dbContext.SaveChanges();
        }
    }
}
