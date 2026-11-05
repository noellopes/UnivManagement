using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using System.Linq;
namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));
            PopulateCriterios(db);

        }
        private static void PopulateCriterios(UniversityDbContext db)
        {
            if (db.CriterioElegibilidade.Any()) return;

            var criterios = new List<CriterioElegibilidade>
            {
                new CriterioElegibilidade
                {
                    Descricao = "Students with low annual family income",
                    RendimentoMaximo = 12000m,
                    EctsMinimos = 30
                },

                new CriterioElegibilidade
                {
                    Descricao = "Students with excellent academic performance",
                    RendimentoMaximo = 25000m,
                    EctsMinimos = 45
                },

                new CriterioElegibilidade
                {
                    Descricao = "Students with disabilities",
                    RendimentoMaximo = 20000m,
                    EctsMinimos = 30
                }
            };

            db.CriterioElegibilidade.AddRange(criterios);
            db.SaveChanges();
        }
    }

}

