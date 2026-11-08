
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        public static void Populate(UniversityDbContext? dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            PopulateTipoBolsas(dbContext);
            PopulateCriterios(dbContext);
        }

        private static void PopulateTipoBolsas(UniversityDbContext dbContext)
        {
            if (dbContext.TipoBolsa.Any()) return;

            var tiposBolsa = new List<TipoBolsa>
            {
                new TipoBolsa { Nome = "Merit Scholarship", Descricao = "Financial support for students with excellent academic performance." },
                new TipoBolsa { Nome = "Social Scholarship", Descricao = "Financial support for students from low-income families." },
                new TipoBolsa { Nome = "Erasmus Scholarship", Descricao = "Financial support for students participating in an Erasmus+ exchange programme." }
            };

            dbContext.TipoBolsa.AddRange(tiposBolsa);
            dbContext.SaveChanges();
        }

        private static void PopulateCriterios(UniversityDbContext dbContext)
        {
            if (dbContext.CriterioElegibilidade.Any()) return;

            PopulateTipoBolsas(dbContext);

            var tiposBolsa = dbContext.TipoBolsa.ToDictionary(t => t.Nome, t => t.TipoBolsaId);

            if (tiposBolsa.Count == 0) return;

            var criterios = new List<CriterioElegibilidade>
            {
                // Merit Scholarship
                new CriterioElegibilidade { Descricao = "Students must have a minimum academic average of 16 out of 20.", RendimentoMaximo = 30000m, EctsMinimos = 45, TipoBolsaId = tiposBolsa["Merit Scholarship"] },

                // Social Scholarship
                new CriterioElegibilidade { Descricao = "Students must belong to a low-income family and demonstrate financial need.", RendimentoMaximo = 12000m, EctsMinimos = 30, TipoBolsaId = tiposBolsa["Social Scholarship"] },

                // Erasmus Scholarship
                new CriterioElegibilidade { Descricao = "Students must be officially selected for an Erasmus+ exchange programme.", RendimentoMaximo = 30000m, EctsMinimos = 30, TipoBolsaId = tiposBolsa["Erasmus Scholarship"] }
            };

            dbContext.CriterioElegibilidade.AddRange(criterios);
            dbContext.SaveChanges();
        }
    }
}
