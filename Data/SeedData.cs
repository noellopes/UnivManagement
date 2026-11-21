using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateFuncoesDocentes(db);

        }

        private static void PopulateFuncoesDocentes(UniversityDbContext db)
        {
            if (db.FuncaoDocente.Any()) return;

            var funcoes = new List<FuncaoDocente>
            {
                new FuncaoDocente { Nome = "Professor Catedrático" },
                new FuncaoDocente { Nome = "ProfessorCoordenador " },
                new FuncaoDocente { Nome = "Professor Auxiliar" },
                new FuncaoDocente { Nome = "Assistente" },
                new FuncaoDocente { Nome = "Monitor" }
            };

            db.FuncaoDocente.AddRange(funcoes);
            db.SaveChanges();
        }
    }
}
