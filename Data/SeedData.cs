using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateObras(db);

        }

        private static void PopulateObras(UniversityDbContext db)
        {
            if (db.Obras.Any()) return;

            var obras = new List<Obra>
            {
                    new Obra
    {
        ISBN = "9780132350884",
        Titulo = "Clean Code",
        Tipo = "Livro Técnico",
        ValorMulta = 1.50
    },

    new Obra
    {
        ISBN = "9780201633610",
        Titulo = "Design Patterns",
        Tipo = "Livro Técnico",
        ValorMulta = 2.00
    },

    new Obra
    {
        ISBN = "9780131103627",
        Titulo = "The C Programming Language",
        Tipo = "Livro Técnico",
        ValorMulta = 1.50
    },

    new Obra
    {
        ISBN = "9780134685991",
        Titulo = "Effective Java",
        Tipo = "Livro Técnico",
        ValorMulta = 2.00
    },

    new Obra
    {
        ISBN = "9780262033848",
        Titulo = "Introduction to Algorithms",
        Tipo = "Manual",
        ValorMulta = 2.50
    }
            };

            db.Obras.AddRange(obras);

            db.SaveChanges();
        }
    }
}
