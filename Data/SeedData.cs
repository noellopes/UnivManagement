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
                "Biblioteca Universitária"
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
            if (db.Obras.Any())
                return;

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
                new Obra
                {
                    Titulo = "Clean Code",
                    ISBN_ISSN = "9780132350884",
                    TipoObraId = tipos["Livro Técnico"],
                    ValorMulta = 2.50,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["Robert C. Martin"]
                        }
                    }
                },

                new Obra
                {
                    Titulo = "Design Patterns",
                    ISBN_ISSN = "9780201633610",
                    TipoObraId = tipos["Livro Técnico"],
                    ValorMulta = 3.00,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["Erich Gamma"]
                        },
                        new ObraAutor
                        {
                            AutorId = autores["Richard Helm"]
                        },
                        new ObraAutor
                        {
                            AutorId = autores["Ralph Johnson"]
                        },
                        new ObraAutor
                        {
                            AutorId = autores["John Vlissides"]
                        }
                    }
                },

                new Obra
                {
                    Titulo = "The C Programming Language",
                    ISBN_ISSN = "9780131103627",
                    TipoObraId = tipos["Manual"],
                    ValorMulta = 1.50,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["Brian W. Kernighan"]
                        },
                        new ObraAutor
                        {
                            AutorId = autores["Dennis M. Ritchie"]
                        }
                    }
                },

                new Obra
                {
                    Titulo = "Effective Java",
                    ISBN_ISSN = "9780134685991",
                    TipoObraId = tipos["Livro Técnico"],
                    ValorMulta = 2.00,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["Joshua Bloch"]
                        }
                    }
                },

                new Obra
                {
                    Titulo = "Introduction to Algorithms",
                    ISBN_ISSN = "9780262033848",
                    TipoObraId = tipos["Livro Técnico"],
                    ValorMulta = 3.50,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["Thomas H. Cormen"]
                        },
                        new ObraAutor
                        {
                            AutorId = autores["Charles E. Leiserson"]
                        },
                        new ObraAutor
                        {
                            AutorId = autores["Ronald L. Rivest"]
                        },
                        new ObraAutor
                        {
                            AutorId = autores["Clifford Stein"]
                        }
                    }
                },

                new Obra
                {
                    Titulo = "IEEE Software",
                    ISBN_ISSN = "07407459",
                    TipoObraId = tipos["Revista"],
                    ValorMulta = 1.00,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["IEEE Computer Society"]
                        }
                    }
                },

                new Obra
                {
                    Titulo = "Regulamento da Biblioteca",
                    ISBN_ISSN = null,
                    TipoObraId = tipos["Documento"],
                    ValorMulta = 0.50,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["Biblioteca Universitária"]
                        }
                    }
                },

                new Obra
                {
                    Titulo = "Relatório Anual da Biblioteca 2026",
                    ISBN_ISSN = null,
                    TipoObraId = tipos["Relatório"],
                    ValorMulta = 0.00,
                    ObrasAutores = new List<ObraAutor>
                    {
                        new ObraAutor
                        {
                            AutorId = autores["Biblioteca Universitária"]
                        }
                    }
                }
            };

            db.Obras.AddRange(obras);
            db.SaveChanges();
        }
    }
};

        