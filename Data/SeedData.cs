
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            PopulateTipos(db);
            PopulateObras(db);
            PopulateAutores(db);
        }

        private static void PopulateTipos(UniversityDbContext db)
        {
            if (db.Tipos.Any())
                return;

            var tipos = new List<TipoObra>
            {
                new TipoObra { NomeTipo = "Livro Técnico" },
                new TipoObra { NomeTipo = "Manual" }
            };

            db.Tipos.AddRange(tipos);
            db.SaveChanges();
        }

        private static void PopulateObras(UniversityDbContext db)
        {
            if (db.Obras.Any())
                return;

            var livroTecnico = db.Tipos
                .First(t => t.NomeTipo == "Livro Técnico");

            var manual = db.Tipos
                .First(t => t.NomeTipo == "Manual");

            var obras = new List<Obra>
            {
                new Obra
                {
                    ISBN = "9780132350884",
                    Titulo = "Clean Code",
                    TipoObraId = livroTecnico.TipoObraId,
                    ValorMulta = 1.50
                },
                new Obra
                {
                    ISBN = "9780201633610",
                    Titulo = "Design Patterns",
                    TipoObraId = livroTecnico.TipoObraId,
                    ValorMulta = 2.00
                },
                new Obra
                {
                    ISBN = "9780131103627",
                    Titulo = "The C Programming Language",
                    TipoObraId = livroTecnico.TipoObraId,
                    ValorMulta = 1.50
                },
                new Obra
                {
                    ISBN = "9780134685991",
                    Titulo = "Effective Java",
                    TipoObraId = livroTecnico.TipoObraId,
                    ValorMulta = 2.00
                },
                new Obra
                {
                    ISBN = "9780262033848",
                    Titulo = "Introduction to Algorithms",
                    TipoObraId = manual.TipoObraId,
                    ValorMulta = 2.50
                }
            };

            db.Obras.AddRange(obras);
            db.SaveChanges();
        }

        private static void PopulateAutores(UniversityDbContext db)
        {
            if (db.Autores.Any())
                return;

            var autores = new List<Autor>
    {
        new Autor { Nome = "Robert C. Martin" },
        new Autor { Nome = "Erich Gamma" },
        new Autor { Nome = "Brian W. Kernighan" },
        new Autor { Nome = "Joshua Bloch" },
        new Autor { Nome = "Thomas H. Cormen" }
    };

            db.Autores.AddRange(autores);
            db.SaveChanges();
        }
    }
}
