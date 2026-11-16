using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateStateEnrollments(db);
        }

        private static void PopulateStateEnrollments(UniversityDbContext dbContext)
        { 
            if (dbContext.StateEnrollment.Any()) return;

            var stateEnrollments = new List<StateEnrollment>
        {
            new StateEnrollment {Name = "PENDING", Description = "Inscrição submetida, a aguardar validação."},
            new StateEnrollment {Name = "ACTIVE", Description = "Inscrição ativa, aluno a frequentar o curso."},
            new StateEnrollment {Name = "SUSPENDED", Description = "Inscrição temporariamente suspensa."},
            new StateEnrollment {Name = "WITHDRAWN", Description = "Aluno desistiu do curso."},
            new StateEnrollment {Name = "COMPLETED", Description = "Curso concluído com sucesso."},
            new StateEnrollment {Name = "CANCELLED", Description = "Inscrição cancelada antes do início."}
        };

            dbContext.StateEnrollment.AddRange(stateEnrollments);
            dbContext.SaveChanges();
        }

    }
}
