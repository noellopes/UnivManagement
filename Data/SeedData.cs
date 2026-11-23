using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            // Inserir os tipos de avaliação
            if (!db.TiposAvaliacao.Any())
            {
                db.TiposAvaliacao.AddRange(
                    new TipoAvaliacao
                    {
                        TipoProva = "Avaliação Contínua"
                    },
                    new TipoAvaliacao
                    {
                        TipoProva = "Exame Época Normal"
                    },
                    new TipoAvaliacao
                    {
                        TipoProva = "Exame Época de Recurso"
                    }
                );

                db.SaveChanges();
            }
        }
    }
}
