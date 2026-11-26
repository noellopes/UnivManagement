using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateEstadoPossivel(db);
            PopulateCourseStates(db);
            PopulateStudents(db);
            PopulateServicosFuncionarios(db);
            PopulateServiceRequestTypes(db);
            PopulateServiceRequestCatalogItems(db);
            PopulateServiceRequestStatuses(db);
            PopulateServiceRequestPriorities(db);
            PopulateFuncionarios(db);
            PopulateBiblioteca(db);
        }

        private static void PopulateEstadoPossivel(UniversityDbContext db)
        {
            if (db.EstadoPossivel.Any()) return;

            db.EstadoPossivel.AddRange(new List<EstadoPossivel> {
                new EstadoPossivel {
                    NomeEstado = "Submetida",
                    Descricao = "A candidatura foi submetida pelo estudante e aguarda análise."
                },
                new EstadoPossivel {
                    NomeEstado = "Em análise",
                    Descricao = "A candidatura está a ser analisada por um técnico dos serviços de ação social."
                },
                new EstadoPossivel {
                    NomeEstado = "A aguardar documentos",
                    Descricao = "Foram pedidos documentos em falta ou correções ao estudante."
                },
                new EstadoPossivel {
                    NomeEstado = "Aprovada",
                    Descricao = "A candidatura cumpre os critérios de elegibilidade e a bolsa foi atribuída."
                },
                new EstadoPossivel {
                    NomeEstado = "Rejeitada",
                    Descricao = "A candidatura não cumpre os critérios de elegibilidade."
                },
                new EstadoPossivel {
                    NomeEstado = "Cancelada",
                    Descricao = "A candidatura foi cancelada pelo estudante ou anulada pelos serviços."
                }
            });

            db.SaveChanges();
        }

        private static void PopulateServicosFuncionarios(UniversityDbContext db)
        {
            if (db.ServicoFuncionario.Any()) return;

            var servicos = new List<ServicoFuncionario>
            {
                new ServicoFuncionario { Nome = "Recursos Humanos", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Serviços Académicos", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Tecnologias de Informação", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Contabilidade e Finanças", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Manutenção e Instalações", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Biblioteca e Documentação", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Ação Social e Apoio ao Estudante", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Relações Internacionais e Mobilidade", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Comunicação e Imagem", Ativo = true, DataCriacao = DateTime.Now },
                new ServicoFuncionario { Nome = "Gabinete Jurídico", Ativo = true, DataCriacao = DateTime.Now }
            };

            db.ServicoFuncionario.AddRange(servicos);

            db.SaveChanges();
        }

        internal static void PopulateServiceRequestTypes(UniversityDbContext? db)
        {
            if (db == null) return;

            var defaultTypes = new List<ServiceRequestType>
            {
                new ServiceRequestType { Name = "Certificate", Description = "Request an enrolment or attendance certificate." },
                new ServiceRequestType { Name = "Official document", Description = "Request an official academic document." },
                new ServiceRequestType { Name = "Student support", Description = "Ask for help or guidance from student services." },
                new ServiceRequestType { Name = "Maintenance", Description = "Report a problem in a building, room or equipment." },
                new ServiceRequestType { Name = "Other", Description = "Other requests." }
            };

            bool alterou = false;
            foreach (var type in defaultTypes)
            {
                if (!db.ServiceRequestTypes.Any(t => t.Name.ToLower() == type.Name.ToLower()))
                {
                    db.ServiceRequestTypes.Add(type);
                    alterou = true;
                }
            }

            if (alterou)
            {
                db.SaveChanges();
            }
        }

        private static void PopulateServiceRequestCatalogItems(UniversityDbContext db)
        {
            if (db.ServiceRequestCatalogItems.Any()) return;

            PopulateServiceRequestTypes(db);

            var types = db.ServiceRequestTypes.ToDictionary(t => t.Name, t => t.ServiceRequestTypeId, StringComparer.OrdinalIgnoreCase);

            var services = new List<ServiceRequestCatalogItem> {
                new ServiceRequestCatalogItem { Name = "Enrolment certificate", Description = "Official proof that the student is enrolled this year.", ServiceRequestTypeId = types["Certificate"], ProcessingDays = 3, RequiredDocuments = "Student card", IsActive = true },
                new ServiceRequestCatalogItem { Name = "Attendance certificate", Description = "Proof of attendance to the classes of a course.", ServiceRequestTypeId = types["Certificate"], ProcessingDays = 3, RequiredDocuments = "Student card", IsActive = true },
                new ServiceRequestCatalogItem { Name = "Grades transcript", Description = "Official list of the grades obtained so far.", ServiceRequestTypeId = types["Official document"], ProcessingDays = 5, RequiredDocuments = "Student card and payment receipt", IsActive = true },
                new ServiceRequestCatalogItem { Name = "Diploma copy", Description = "A certified copy of the diploma.", ServiceRequestTypeId = types["Official document"], ProcessingDays = 15, RequiredDocuments = "Identification document and payment receipt", IsActive = true },
                new ServiceRequestCatalogItem { Name = "Academic counselling", Description = "A meeting with the student support service.", ServiceRequestTypeId = types["Student support"], ProcessingDays = 7, IsActive = true },
                new ServiceRequestCatalogItem { Name = "Classroom equipment repair", Description = "Report a broken projector, computer or chair.", ServiceRequestTypeId = types["Maintenance"], ProcessingDays = 2, RequiredDocuments = "Room number and a short description", IsActive = true },
                new ServiceRequestCatalogItem { Name = "Paper enrolment form", Description = "Replaced by the online enrolment.", ServiceRequestTypeId = types["Official document"], ProcessingDays = 20, IsActive = false }
            };

            db.ServiceRequestCatalogItems.AddRange(services);
            db.SaveChanges();
        }

        private static void PopulateServiceRequestStatuses(UniversityDbContext db)
        {
            if (db.ServiceRequestStatuses.Any()) return;

            var statuses = new List<ServiceRequestStatus> {
                new ServiceRequestStatus { Name = "Submitted", DisplayOrder = 1, Color = "secondary",
                    Description = "The request was received and is waiting to be reviewed." },
                new ServiceRequestStatus { Name = "In progress", DisplayOrder = 2, Color = "primary",
                    Description = "The administrative services are working on the request." },
                new ServiceRequestStatus { Name = "Waiting for information", DisplayOrder = 3, Color = "warning", RequiresNote = true,
                    Description = "More information or documents are needed from the requester." },
                new ServiceRequestStatus { Name = "Resolved", DisplayOrder = 4, Color = "success", IsFinal = true,
                    Description = "The request was completed." },
                new ServiceRequestStatus { Name = "Rejected", DisplayOrder = 5, Color = "danger", IsFinal = true, RequiresNote = true,
                    Description = "The request cannot be accepted; the note explains why." },
                new ServiceRequestStatus { Name = "Cancelled", DisplayOrder = 6, Color = "dark", IsFinal = true,
                    Description = "The request was cancelled." }
            };

            db.ServiceRequestStatuses.AddRange(statuses);
            db.SaveChanges();
        }

        private static void PopulateServiceRequestPriorities(UniversityDbContext db)
        {
            if (db.ServiceRequestPriorities.Any()) return;

            var priorities = new List<ServiceRequestPriority> {
                new ServiceRequestPriority { Name = "Low", Level = 1, ResponseDays = 14, Color = "success" },
                new ServiceRequestPriority { Name = "Medium", Level = 2, ResponseDays = 7, Color = "info" },
                new ServiceRequestPriority { Name = "High", Level = 3, ResponseDays = 3, Color = "warning" },
                new ServiceRequestPriority { Name = "Urgent", Level = 4, ResponseDays = 1, Color = "danger" }
            };

            db.ServiceRequestPriorities.AddRange(priorities);
            db.SaveChanges();
        }

        private static void PopulateFuncionarios(UniversityDbContext db)
        {
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

            if (db.Funcionario.Any()) return;

            var servicoPadrao = db.ServicoFuncionario.First();

            var servicoRH = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Recursos Humanos") ?? servicoPadrao;
            var servicoAcad = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Serviços Académicos") ?? servicoPadrao;
            var servicoTI = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Tecnologias de Informação") ?? servicoPadrao;
            var servicoFin = db.ServicoFuncionario.FirstOrDefault(s => s.Nome == "Gestão Financeira") ?? servicoPadrao;

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

        private static void PopulateStudents(UniversityDbContext dbContext)
        {
            if (dbContext.Student.Any()) return;

            var students = new List<Student>
            {
                new Student { NumStudent = "1000001", Name = "Ana Silva", Gender = "F", Nationality = "Portuguesa", BirthDate = new DateOnly(2000, 3, 15), Address = "Rua das Flores, 12, Lisboa", NIF = "123456789", Email = "ana.silva@uni.pt", CountryPhoneCode = "+351", Telephone = "912345678" },
                new Student { NumStudent = "1000002", Name = "Bruno Costa", Gender = "M", Nationality = "Portuguesa", BirthDate = new DateOnly(1999, 7, 22), Address = "Avenida da Liberdade, 45, Porto", NIF = "987654322", Email = "bruno.costa@uni.pt", CountryPhoneCode = "+351", Telephone = "913456789" },
                new Student { NumStudent = "1000003", Name = "Carla Mendes", Gender = "F", Nationality = "Portuguesa", BirthDate = new DateOnly(2012, 1, 10), Address = "Praça do Comércio, 3, Coimbra", NIF = "111222338", Email = "carla.mendes@uni.pt", CountryPhoneCode = "+351", Telephone = "914567890" },
                new Student { NumStudent = "1000004", Name = "David Johnson", Gender = "M", Nationality = "Britânica", BirthDate = new DateOnly(2001, 11, 5), Address = "Rua de São Bento, 78, Braga", NIF = "222333448", Email = "david.johnson@uni.pt", CountryPhoneCode = "+44", Telephone = "7911123456" },
                new Student { NumStudent = "1000005", Name = "Eva Rodrigues", Gender = "F", Nationality = "Brasileira", BirthDate = new DateOnly(1998, 9, 30), Address = "Rua Augusta, 100, Lisboa", NIF = "333444558", Email = "eva.rodrigues@uni.pt", CountryPhoneCode = "+55", Telephone = "11987654321" },
                new Student { NumStudent = "1000006", Name = "Pedro Silva", Gender = "M", Nationality = "Portuguesa", BirthDate = new DateOnly(2002, 5, 18), Address = "Rua das Flores, 12, Lisboa", NIF = "555666492", Email = "pedro.silva@uni.pt", CountryPhoneCode = "+351", Telephone = "912345698" }
            };

            dbContext.Student.AddRange(students);
            dbContext.SaveChanges();
        }

        private static void PopulateCourseStates(UniversityDbContext dbContext)
        {
            if (dbContext.CourseState.Any()) return;

            var courseStates = new List<CourseState>
            {
                new CourseState { Name = "DRAFT", Description = "Curso em preparação, ainda não publicado."},
                new CourseState { Name = "ACTIVE", Description = "Curso a decorrer normalmente."},
                new CourseState { Name = "SUSPENDED", Description = "Curso temporariamente suspenso."},
                new CourseState { Name = "CONCLUDED", Description = "Curso terminado, sem novas inscrições."},
                new CourseState { Name = "ARCHIVED", Description = "Curso arquivado historicamente."}
            };

            dbContext.CourseState.AddRange(courseStates);
            dbContext.SaveChanges();
        }

        private static void PopulateBiblioteca(UniversityDbContext db)
        {
            PopulateTipoObra(db);
            PopulateObras(db);
            PopulateExemplares(db);
            PopulateEmprestimos(db);
        }

        private static void PopulateTipoObra(UniversityDbContext db)
        {
            if (db.Tipos.Any()) return;

            var tipos = new List<TipoObra>
    {
        new TipoObra { NomeTipo = "Livro" },
        new TipoObra { NomeTipo = "Revista" }
    };

            db.Tipos.AddRange(tipos);
            db.SaveChanges();
        }

        private static void PopulateObras(UniversityDbContext db)
        {
            if (!db.Tipos.Any()) PopulateTipoObra(db);
            if (db.Obras.Any()) return;

            var tipoLivro = db.Tipos.FirstOrDefault(t => t.NomeTipo == "Livro") ?? db.Tipos.First();

            var obras = new List<Obra>
    {
        new Obra
        {
            ISBN = "978-972-722-800-0",
            Titulo = "Engenharia de Software",
            TipoObraId = tipoLivro.TipoObraId,
            ValorMulta = 1.50
        },
        new Obra
        {
            ISBN = "978-013-235-088-4",
            Titulo = "Clean Code",
            TipoObraId = tipoLivro.TipoObraId,
            ValorMulta = 2.00
        },
        new Obra
        {
            ISBN = "978-972-0-01234-5",
            Titulo = "Introdução à Programação com C#",
            TipoObraId = tipoLivro.TipoObraId,
            ValorMulta = 1.00
        },
        new Obra
        {
            ISBN = "978-020-163-361-0",
            Titulo = "Design Patterns",
            TipoObraId = tipoLivro.TipoObraId,
            ValorMulta = 2.50
        }
    };

            db.Obras.AddRange(obras);
            db.SaveChanges();
        }

        private static void PopulateExemplares(UniversityDbContext db)
        {
            if (!db.Obras.Any()) PopulateObras(db);
            if (db.Exemplares.Any()) return;

            var engenhariaSoftware = db.Obras.FirstOrDefault(o => o.Titulo.Contains("Engenharia"));
            var cleanCode = db.Obras.FirstOrDefault(o => o.Titulo.Contains("Clean Code"));
            var introCSharp = db.Obras.FirstOrDefault(o => o.Titulo.Contains("C#"));
            var designPatterns = db.Obras.FirstOrDefault(o => o.Titulo.Contains("Design Patterns"));

            if (engenhariaSoftware == null || cleanCode == null || introCSharp == null || designPatterns == null)
                return;

            var exemplares = new List<Exemplar>
    {
        new Exemplar { Codigo = "EX-ES-001", Estado = "Emprestado", ObraId = engenhariaSoftware.ObraId },
        new Exemplar { Codigo = "EX-ES-002", Estado = "Disponível", ObraId = engenhariaSoftware.ObraId },
        new Exemplar { Codigo = "EX-CC-001", Estado = "Emprestado", ObraId = cleanCode.ObraId },
        new Exemplar { Codigo = "EX-CC-002", Estado = "Disponível", ObraId = cleanCode.ObraId },
        new Exemplar { Codigo = "EX-CS-001", Estado = "Disponível", ObraId = introCSharp.ObraId },
        new Exemplar { Codigo = "EX-DP-001", Estado = "Disponível", ObraId = designPatterns.ObraId }
    };

            db.Exemplares.AddRange(exemplares);
            db.SaveChanges();
        }

        private static void PopulateEmprestimos(UniversityDbContext db)
        {
            if (!db.Exemplares.Any()) PopulateExemplares(db);
            if (db.Emprestimo.Any()) return;

            var exEngenharia = db.Exemplares.FirstOrDefault(e => e.Codigo == "EX-ES-001");
            var exCleanCode = db.Exemplares.FirstOrDefault(e => e.Codigo == "EX-CC-001");
            var exCSharp = db.Exemplares.FirstOrDefault(e => e.Codigo == "EX-CS-001");

            var emprestimos = new List<Emprestimo>();

            if (exEngenharia != null)
            {
                emprestimos.Add(new Emprestimo
                {
                    dataEmprestimo = DateTime.Today.AddDays(-3),
                    dataDevolucao = DateTime.Today.AddDays(4),
                    estado = "Ativo",
                    ExemplarId = exEngenharia.ExemplarId
                });
            }

            if (exCleanCode != null)
            {
                emprestimos.Add(new Emprestimo
                {
                    dataEmprestimo = DateTime.Today.AddDays(-5),
                    dataDevolucao = DateTime.Today.AddDays(2),
                    estado = "Ativo",
                    ExemplarId = exCleanCode.ExemplarId
                });
            }

            if (exCSharp != null)
            {
                emprestimos.Add(new Emprestimo
                {
                    dataEmprestimo = DateTime.Today.AddDays(-15),
                    dataDevolucao = DateTime.Today.AddDays(-8),
                    estado = "Devolvido",
                    ExemplarId = exCSharp.ExemplarId
                });
            }

            db.Emprestimo.AddRange(emprestimos);
            db.SaveChanges();
        }
    }
}