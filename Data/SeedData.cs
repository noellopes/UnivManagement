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

            string[] tipos = {
                "Avaliação Contínua",
                "Exame Normal",
                "Exame de Recurso",
                "Exame Época Especial"
            };

            foreach (var tipo in tipos)
            {
                if (!db.TiposAvaliacao.Any(t => t.TipoProva == tipo))
                {
                    db.TiposAvaliacao.Add(new TipoAvaliacao
                    {
                        TipoProva = tipo
                    });
                }
            }

            db.SaveChanges();
        }
    }
}
