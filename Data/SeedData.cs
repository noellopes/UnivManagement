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

            foreach( var nome in nomesTipos)
            {
                if(!db.Tipos.Any(t => t.NomeTipo == nome))
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
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateFuncionarios(db);
        }

        private static void PopulateFuncionarios(UniversityDbContext db)
        {
            // 1. Garantir que existem Serviços na BD
            if (!db.ServicoFuncionario.Any())
            {
                var servicos = new List<ServicoFuncionario> {
                    new ServicoFuncionario { Nome = "Recursos Humanos", Ativo = true },
                    new ServicoFuncionario { Nome = "Serviços Académicos", Ativo = true },
                    new ServicoFuncionario { Nome = "Tecnologias de Informação", Ativo = true },
                    new ServicoFuncionario { Nome = "Gestão Financeira", Ativo = true }
                };

                db.ServicoFuncionario.AddRange(servicos);
                db.SaveChanges();
            }

            // 2. Se a tabela Funcionario já tiver registos, não faz nada
            if (db.Funcionario.Any()) return;

            // Obter os serviços da BD (ou o primeiro disponível como fallback seguro)
            var servicoPadrao = db.ServicoFuncionario.First();

            var servicoRH = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Recursos Humanos") ?? servicoPadrao;
            var servicoAcad = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Serviços Académicos") ?? servicoPadrao;
            var servicoTI = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Tecnologias de Informação") ?? servicoPadrao;
            var servicoFin = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Gestão Financeira") ?? servicoPadrao;

            // 3. Adicionar lista de Funcionários iniciais
            var funcionarios = new List<Funcionario> {
                new Funcionario {
                    Nome = "Ana Maria Silva",
                    Nif = "234567890",
                    Email = "ana.silva@upg.pt",
                    Telefone = "912345678",
                    Ativo = true,
                    DataAdmissao = new DateTime(2020, 3, 15),
                    Morada = "Av. são bento, 12, Guarda",
                    ServicoFuncionarioId = servicoRH.ServicoFuncionarioId
                },
                new Funcionario {
                    Nome = "Carlos Alberto Santos",
                    Nif = "245678901",
                    Email = "carlos.santos@upg.pt",
                    Telefone = "934567890",
                    Ativo = true,
                    DataAdmissao = new DateTime(2018, 9, 1),
                    Morada = "Rua Direita, 45, Guarda",
                    ServicoFuncionarioId = servicoTI.ServicoFuncionarioId
                },
                new Funcionario {
                    Nome = "Beatriz Rocha",
                    Nif = "256789012",
                    Email = "beatriz.rocha@upg.pt",
                    Telefone = "965432109",
                    Ativo = true,
                    DataAdmissao = new DateTime(2021, 1, 10),
                    Morada = "Rua Duque de bragança, 8, Guarda",
                    ServicoFuncionarioId = servicoAcad.ServicoFuncionarioId
                },
                new Funcionario {
                    Nome = "João Pedro Ferreira",
                    Nif = "267890123",
                    Email = "joao.ferreira@upg.pt",
                    Telefone = "921122334",
                    Ativo = false,
                    DataAdmissao = new DateTime(2019, 6, 20),
                    Morada = "Paiva couceiro, 102, Guarda",
                    ServicoFuncionarioId = servicoFin.ServicoFuncionarioId
                }
            };

            db.Funcionario.AddRange(funcionarios);
            db.SaveChanges();
        }
    }
}