
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
            PopulateAutores(db);
            PopulateObras(db);
        }

        private static void PopulateTipos(UniversityDbContext db)
        {
            var nomesTipos = new[]
            {
                "Livro Técnico",
                "Manual",
                "Revista",
                "Documento",
                "Relatório"
            };

            foreach (var nome in nomesTipos)
            {
                if (!db.Tipos.Any(t => t.NomeTipo == nome))
                {
                    db.Tipos.Add(new TipoObra
                    {
                        NomeTipo = nome
                    });
                }
            }

            db.SaveChanges();
        }

        private static void PopulateAutores(UniversityDbContext db)
        {
            var nomesAutores = new[]
            {
                // Autores originais
                "Robert C. Martin",
                "Erich Gamma",
                "Richard Helm",
                "Ralph Johnson",
                "John Vlissides",
                "Brian W. Kernighan",
                "Dennis M. Ritchie",
                "Joshua Bloch",
                "Thomas H. Cormen",
                "Charles E. Leiserson",
                "Ronald L. Rivest",
                "Clifford Stein",
                "IEEE Computer Society",
                "Biblioteca Universitária",

                // Novos autores
                "Andrew Hunt",
                "David Thomas",
                "Martin Fowler",
                "Steve McConnell",
                "Eric Evans",
                "Kent Beck",
                "Frederick P. Brooks Jr.",
                "Michael Feathers",
                "Eric Matthes",
                "Jon Skeet",
                "Joseph Albahari",
                "Mark J. Price",
                "Abraham Silberschatz",
                "Henry F. Korth",
                "S. Sudarshan",
                "Andrew S. Tanenbaum",
                "David J. Wetherall",
                "William Stallings",
                "Brett McLaughlin",
                "Gary Pollice",
                "David West",
                "Aditya Bhargava",
                "Martin Kleppmann",
                "Gene Kim",
                "Kevin Behr",
                "George Spafford"
            };

            foreach (var nome in nomesAutores)
            {
                if (!db.Autores.Any(a => a.Nome == nome))
                {
                    db.Autores.Add(new Autor
                    {
                        Nome = nome
                    });
                }
            }

            db.SaveChanges();
        }

        private static void PopulateObras(UniversityDbContext db)
        {
            var tipos = db.Tipos.ToDictionary(
                t => t.NomeTipo,
                t => t.TipoObraId
            );

            var autores = db.Autores.ToDictionary(
                a => a.Nome,
                a => a.AutorId
            );

            var obras = new List<Obra>
            {
                // ==========================
                // OBRAS ORIGINAIS
                // ==========================

                CriarObra(
                    "Clean Code",
                    "9780132350884",
                    "Livro Técnico",
                    2.50,
                    "Robert C. Martin"
                ),

                CriarObra(
                    "Design Patterns",
                    "9780201633610",
                    "Livro Técnico",
                    3.00,
                    "Erich Gamma",
                    "Richard Helm",
                    "Ralph Johnson",
                    "John Vlissides"
                ),

                CriarObra(
                    "The C Programming Language",
                    "9780131103627",
                    "Manual",
                    1.50,
                    "Brian W. Kernighan",
                    "Dennis M. Ritchie"
                ),

                CriarObra(
                    "Effective Java",
                    "9780134685991",
                    "Livro Técnico",
                    2.00,
                    "Joshua Bloch"
                ),

                CriarObra(
                    "Introduction to Algorithms",
                    "9780262033848",
                    "Livro Técnico",
                    3.50,
                    "Thomas H. Cormen",
                    "Charles E. Leiserson",
                    "Ronald L. Rivest",
                    "Clifford Stein"
                ),

                CriarObra(
                    "IEEE Software",
                    "07407459",
                    "Revista",
                    1.00,
                    "IEEE Computer Society"
                ),

                CriarObra(
                    "Regulamento da Biblioteca",
                    null,
                    "Documento",
                    0.50,
                    "Biblioteca Universitária"
                ),

                CriarObra(
                    "Relatório Anual da Biblioteca 2026",
                    null,
                    "Relatório",
                    0.00,
                    "Biblioteca Universitária"
                ),

                // ==========================
                // 20 NOVOS LIVROS
                // ==========================

                // 1
                CriarObra(
                    "The Pragmatic Programmer",
                    "9780135957059",
                    "Livro Técnico",
                    2.50,
                    "Andrew Hunt",
                    "David Thomas"
                ),

                // 2
                CriarObra(
                    "Refactoring",
                    "9780134757599",
                    "Livro Técnico",
                    3.00,
                    "Martin Fowler"
                ),

                // 3
                CriarObra(
                    "Code Complete",
                    "9780735619678",
                    "Livro Técnico",
                    2.50,
                    "Steve McConnell"
                ),

                // 4
                CriarObra(
                    "Clean Architecture",
                    "9780134494166",
                    "Livro Técnico",
                    3.00,
                    "Robert C. Martin"
                ),

                // 5
                CriarObra(
                    "The Clean Coder",
                    "9780137081073",
                    "Livro Técnico",
                    2.00,
                    "Robert C. Martin"
                ),

                // 6
                CriarObra(
                    "Domain-Driven Design",
                    "9780321125217",
                    "Livro Técnico",
                    3.50,
                    "Eric Evans"
                ),

                // 7
                CriarObra(
                    "Test Driven Development: By Example",
                    "9780321146533",
                    "Livro Técnico",
                    2.00,
                    "Kent Beck"
                ),

                // 8
                CriarObra(
                    "The Mythical Man-Month",
                    "9780201835953",
                    "Livro Técnico",
                    2.00,
                    "Frederick P. Brooks Jr."
                ),

                // 9
                CriarObra(
                    "Working Effectively with Legacy Code",
                    "9780131177055",
                    "Livro Técnico",
                    2.50,
                    "Michael Feathers"
                ),

                // 10
                CriarObra(
                    "Python Crash Course",
                    "9781718502703",
                    "Manual",
                    1.50,
                    "Eric Matthes"
                ),

                // 11
                CriarObra(
                    "C# in Depth",
                    "9781617294532",
                    "Livro Técnico",
                    2.50,
                    "Jon Skeet"
                ),

                // 12
                CriarObra(
                    "C# 12 in a Nutshell",
                    "9781098147440",
                    "Manual",
                    2.50,
                    "Joseph Albahari"
                ),

                // 13
                CriarObra(
                    "C# 12 and .NET 8",
                    "9781837635870",
                    "Manual",
                    2.50,
                    "Mark J. Price"
                ),

                // 14
                CriarObra(
                    "Database System Concepts",
                    "9780078022159",
                    "Livro Técnico",
                    3.50,
                    "Abraham Silberschatz",
                    "Henry F. Korth",
                    "S. Sudarshan"
                ),

                // 15
                CriarObra(
                    "Computer Networks",
                    "9780132126953",
                    "Livro Técnico",
                    3.00,
                    "Andrew S. Tanenbaum",
                    "David J. Wetherall"
                ),

                // 16
                CriarObra(
                    "Computer Organization and Architecture",
                    "9780134101613",
                    "Livro Técnico",
                    3.00,
                    "William Stallings"
                ),

                // 17
                CriarObra(
                    "Head First Object-Oriented Analysis and Design",
                    "9780596008673",
                    "Manual",
                    2.00,
                    "Brett McLaughlin",
                    "Gary Pollice",
                    "David West"
                ),

                // 18
                CriarObra(
                    "Grokking Algorithms",
                    "9781617292231",
                    "Manual",
                    1.50,
                    "Aditya Bhargava"
                ),

                // 19
                CriarObra(
                    "Designing Data-Intensive Applications",
                    "9781449373320",
                    "Livro Técnico",
                    3.50,
                    "Martin Kleppmann"
                ),

                // 20
                CriarObra(
                    "The Phoenix Project",
                    "9781942788294",
                    "Livro Técnico",
                    2.00,
                    "Gene Kim",
                    "Kevin Behr",
                    "George Spafford"
                )
            };

            // Adicionar apenas obras que ainda não existem
            foreach (var obra in obras)
            {
                bool existe = db.Obras.Any(o =>
                    o.Titulo == obra.Titulo ||
                    (obra.ISBN_ISSN != null &&
                     o.ISBN_ISSN == obra.ISBN_ISSN)
                );

                if (!existe)
                {
                    db.Obras.Add(obra);
                }
            }

            db.SaveChanges();

            // Método auxiliar para criar obras
            Obra CriarObra(
                string titulo,
                string? isbn,
                string tipo,
                double valorMulta,
                params string[] nomesAutores)
            {
                return new Obra
                {
                    Titulo = titulo,
                    ISBN_ISSN = isbn,
                    TipoObraId = tipos[tipo],
                    ValorMulta = valorMulta,

                    ObrasAutores = nomesAutores
                        .Select(nome => new ObraAutor
                        {
                            AutorId = autores[nome]
                        })
                        .ToList()
                };
            }
        }
    }
}
