using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateLivros(db);

        }

        private static void PopulateLivros(UniversityDbContext db)
        {
            if (db.Livros.Any()) return;

            var livros = new List<Livro>
            {
                    new Livro
    {
        ISBN = "9780132350884",
        Titulo = "Clean Code",
        Tipo = "Livro Técnico",
        ValorMulta = 1.50
    },

    new Livro
    {
        ISBN = "9780201633610",
        Titulo = "Design Patterns",
        Tipo = "Livro Técnico",
        ValorMulta = 2.00
    },

    new Livro
    {
        ISBN = "9780131103627",
        Titulo = "The C Programming Language",
        Tipo = "Livro Técnico",
        ValorMulta = 1.50
    },

    new Livro
    {
        ISBN = "9780134685991",
        Titulo = "Effective Java",
        Tipo = "Livro Técnico",
        ValorMulta = 2.00
    },

    new Livro
    {
        ISBN = "9780262033848",
        Titulo = "Introduction to Algorithms",
        Tipo = "Manual",
        ValorMulta = 2.50
    }
            };

            db.Livros.AddRange(livros);

            db.SaveChanges();
        }
    }
}
