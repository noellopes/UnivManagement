using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateServiceRequestCatalogItems(db);
            PopulateServiceRequestStatuses(db);
            PopulateServiceRequestPriorities(db);
            PopulateFuncionarios(db);
        }

        private static void PopulateServiceRequestCatalogItems(UniversityDbContext db)
        {
            // If there are already services, do nothing (so the data is not duplicated on every start)
            if (db.ServiceRequestCatalogItems.Any()) return;

            // Request types: reuse the ones that exist (by name) and create only the missing ones
            var types = db.ServiceRequestTypes.ToDictionary(t => t.Name, t => t.ServiceRequestTypeId, StringComparer.OrdinalIgnoreCase);
            var typeNames = new[] { "Certificate", "Official document", "Student support", "Maintenance" };

            foreach (var name in typeNames)
            {
                if (!types.ContainsKey(name))
                {
                    db.ServiceRequestTypes.Add(new ServiceRequestType { Name = name });
                }
            }
            db.SaveChanges();

            // Read them again so every type has its id
            types = db.ServiceRequestTypes.ToDictionary(t => t.Name, t => t.ServiceRequestTypeId, StringComparer.OrdinalIgnoreCase);

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
        }

        private static void PopulateServiceRequestPriorities(UniversityDbContext db)
        {
            // If there are already priorities, do nothing (so the data is not duplicated on every start)
            if (db.ServiceRequestPriorities.Any()) return;

            var priorities = new List<ServiceRequestPriority> {
                new ServiceRequestPriority { Name = "Low", Level = 1, ResponseDays = 14, Color = "success" },
                new ServiceRequestPriority { Name = "Medium", Level = 2, ResponseDays = 7, Color = "info" },
                new ServiceRequestPriority { Name = "High", Level = 3, ResponseDays = 3, Color = "warning" },
                new ServiceRequestPriority { Name = "Urgent", Level = 4, ResponseDays = 1, Color = "danger" }
            };

            db.ServiceRequestPriorities.AddRange(priorities);
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