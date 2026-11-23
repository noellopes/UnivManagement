using UnivManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateFuncionarios(db);
            PopulateServiceRequestTypes(db);
            PopulateServiceRequestStatuses(db);
            PopulateServiceRequests(db);
            PopulateServiceRequestPriorities(db);
            PopulateServiceRequestCatalogItems(db);
            PopulateServiceRequestAttachments(db);
            PopulateServiceRequestComments(db);
            PopulateServiceRequestAssignments(db);
            PopulateServiceRequestHistories(db);
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

        internal static void PopulateServiceRequestTypes(UniversityDbContext? db) {
            if (db == null || db.ServiceRequestTypes.Any()) return;

            db.ServiceRequestTypes.AddRange(
                new ServiceRequestType { Name = "Certificate", Description = "Request an enrolment or attendance certificate." },
                new ServiceRequestType { Name = "Official document", Description = "Request an official academic document." },
                new ServiceRequestType { Name = "Student support", Description = "Ask for help or guidance from student services." },
                new ServiceRequestType { Name = "Maintenance", Description = "Report a problem in a building, room or equipment." },
                new ServiceRequestType { Name = "Other" }
            );
            db.SaveChanges();
        }

        private static void PopulateServiceRequestStatuses(UniversityDbContext db) {
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

        // A request needs a type and a status, so it is only added when they already exist
        internal static void PopulateServiceRequests(UniversityDbContext? db) {
            if (db == null || db.ServiceRequests.Any()) return;

            var types = db.ServiceRequestTypes.OrderBy(t => t.ServiceRequestTypeId).ToList();
            var statuses = db.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToList();
            var open = statuses.Where(s => !s.IsFinal).ToList();
            var closed = statuses.FirstOrDefault(s => s.IsFinal);
            if (types.Count == 0 || open.Count == 0) return;

            // The type with this name, or the first one if it does not exist
            int TypeId(string name) => (types.FirstOrDefault(t => t.Name == name) ?? types[0]).ServiceRequestTypeId;

            var submitted = open[0];
            var inProgress = open[Math.Min(1, open.Count - 1)];
            var now = DateTime.Now;

            db.ServiceRequests.AddRange(
                new ServiceRequest {
                    Title = "Enrolment certificate",
                    Description = "I need an enrolment certificate to apply for a scholarship.",
                    ServiceRequestTypeId = TypeId("Certificate"),
                    ServiceRequestStatusId = submitted.ServiceRequestStatusId,
                    RequesterName = "Ana Costa",
                    RequesterEmail = "ana.costa@example.com",
                    CreatedAt = now.AddDays(-5)
                },
                new ServiceRequest {
                    Title = "Copy of my transcript of records",
                    ServiceRequestTypeId = TypeId("Official document"),
                    ServiceRequestStatusId = inProgress.ServiceRequestStatusId,
                    RequesterName = "Rui Santos",
                    RequesterEmail = "rui.santos@example.com",
                    AssignedTo = "Academic office",
                    CreatedAt = now.AddDays(-4),
                    UpdatedAt = now.AddDays(-2)
                },
                new ServiceRequest {
                    Title = "Help choosing an elective course",
                    Description = "I would like some guidance before the enrolment period ends.",
                    ServiceRequestTypeId = TypeId("Student support"),
                    ServiceRequestStatusId = (closed ?? inProgress).ServiceRequestStatusId,
                    RequesterName = "Marta Lopes",
                    RequesterEmail = "marta.lopes@example.com",
                    AssignedTo = "Student services",
                    CreatedAt = now.AddDays(-10),
                    UpdatedAt = now.AddDays(-7),
                    ClosedAt = closed == null ? null : now.AddDays(-7)
                },
                new ServiceRequest {
                    Title = "Broken projector in room B2.07",
                    Description = "The projector does not turn on, so the lessons cannot use it.",
                    ServiceRequestTypeId = TypeId("Maintenance"),
                    ServiceRequestStatusId = inProgress.ServiceRequestStatusId,
                    RequesterName = "Pedro Alves",
                    RequesterEmail = "pedro.alves@example.com",
                    AssignedTo = "Maintenance team",
                    CreatedAt = now.AddDays(-2),
                    UpdatedAt = now.AddDays(-1)
                },
                new ServiceRequest {
                    Title = "Change of contact details",
                    ServiceRequestTypeId = TypeId("Other"),
                    ServiceRequestStatusId = submitted.ServiceRequestStatusId,
                    RequesterName = "Inês Ferreira",
                    RequesterEmail = "ines.ferreira@example.com",
                    CreatedAt = now.AddDays(-1)
                }
            );
            db.SaveChanges();
        }

        private static void PopulateServiceRequestPriorities(UniversityDbContext db) {
            // If there are already priorities, do nothing (so the data is not duplicated on every start)
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
            db.SaveChanges();
        }

        private static void PopulateServiceRequestAttachments(UniversityDbContext db) {
            // If there are already attachments, do nothing (so the data is not duplicated on every start)
            if (db.ServiceRequestAttachments.Any()) return;

            // An attachment belongs to a request: if there are none yet, create two sample ones
            if (!db.ServiceRequests.Any()) {
                var type = db.ServiceRequestTypes.OrderBy(t => t.ServiceRequestTypeId).FirstOrDefault();
                if (type == null) {
                    type = new ServiceRequestType { Name = "Certificate" };
                    db.ServiceRequestTypes.Add(type);
                }

                var status = db.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).FirstOrDefault();
                if (status == null) {
                    status = new ServiceRequestStatus { Name = "Pending", DisplayOrder = 1, Color = "warning" };
                    db.ServiceRequestStatuses.Add(status);
                }

                db.ServiceRequests.AddRange(
                    new ServiceRequest { Title = "Enrolment certificate for 2026", Type = type, Status = status, RequesterName = "Ana Silva", RequesterEmail = "ana.silva@example.com" },
                    new ServiceRequest { Title = "Grades transcript", Type = type, Status = status, RequesterName = "Rui Costa", RequesterEmail = "rui.costa@example.com" }
                );
                db.SaveChanges();
            }

            var requests = db.ServiceRequests.OrderBy(r => r.ServiceRequestId).Take(2).ToList();
            var first = requests[0].ServiceRequestId;
            var second = requests[requests.Count - 1].ServiceRequestId;

            var attachments = new List<ServiceRequestAttachment> {
                new ServiceRequestAttachment { ServiceRequestId = first, FileName = "enrolment-form.pdf", SizeInKb = 240, Description = "Signed enrolment form." },
                new ServiceRequestAttachment { ServiceRequestId = first, FileName = "student-card.jpg", SizeInKb = 850, Description = "Front of the student card." },
                new ServiceRequestAttachment { ServiceRequestId = second, FileName = "payment-receipt.pdf", SizeInKb = 120, Description = "Receipt of the transcript fee." },
                new ServiceRequestAttachment { ServiceRequestId = second, FileName = "id-document.png", SizeInKb = 1500 },
                new ServiceRequestAttachment { ServiceRequestId = second, FileName = "justification.docx", SizeInKb = 64, Description = "Why the document is needed." }
            };

            db.ServiceRequestAttachments.AddRange(attachments);
            db.SaveChanges();
        }
        private static void PopulateServiceRequestComments(UniversityDbContext db) {
            // If there are already comments, do nothing (so the data is not duplicated on every start)
            if (db.ServiceRequestComments.Any()) return;

            var requests = db.ServiceRequests.OrderBy(r => r.ServiceRequestId).Take(2).ToList();
            if (requests.Count == 0) return;
            var first = requests[0].ServiceRequestId;
            var second = requests[requests.Count - 1].ServiceRequestId;

            db.ServiceRequestComments.AddRange(
                new ServiceRequestComment { ServiceRequestId = first, Author = "Academic office", Text = "We received your request and we are checking your enrolment.", CreatedAt = DateTime.Now.AddDays(-3) },
                new ServiceRequestComment { ServiceRequestId = first, Author = "Ana Costa", Text = "Thank you. Do you need any other document?", CreatedAt = DateTime.Now.AddDays(-2) },
                new ServiceRequestComment { ServiceRequestId = second, Author = "Academic office", Text = "Please attach the payment receipt of the transcript fee.", CreatedAt = DateTime.Now.AddDays(-1) }
            );
            db.SaveChanges();
        }

        private static void PopulateServiceRequestAssignments(UniversityDbContext db) {
            // If there are already assignments, do nothing (so the data is not duplicated on every start)
            if (db.ServiceRequestAssignments.Any()) return;

            var requests = db.ServiceRequests.OrderBy(r => r.ServiceRequestId).Take(3).ToList();
            if (requests.Count == 0) return;

            // Funcionario and Departamento are not linked yet, so only their numbers are stored
            db.ServiceRequestAssignments.AddRange(
                new ServiceRequestAssignment { ServiceRequestId = requests[0].ServiceRequestId, FuncionarioId = 1, DepartamentoId = 2, AssignedAt = DateTime.Now.AddDays(-4), EndedAt = DateTime.Now.AddDays(-3), Note = "First review of the request." },
                new ServiceRequestAssignment { ServiceRequestId = requests[0].ServiceRequestId, FuncionarioId = 2, DepartamentoId = 2, AssignedAt = DateTime.Now.AddDays(-3), Note = "Now in charge of the request." },
                new ServiceRequestAssignment { ServiceRequestId = requests[requests.Count - 1].ServiceRequestId, DepartamentoId = 3, AssignedAt = DateTime.Now.AddDays(-1) }
            );
            db.SaveChanges();
        }

        private static void PopulateServiceRequestHistories(UniversityDbContext db) {
            // If there is already history, do nothing (so the data is not duplicated on every start)
            if (db.ServiceRequestHistories.Any()) return;

            var request = db.ServiceRequests.OrderBy(r => r.ServiceRequestId).FirstOrDefault();
            var statuses = db.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).Take(3).ToList();
            if (request == null || statuses.Count < 2) return;

            db.ServiceRequestHistories.AddRange(
                new ServiceRequestHistory { ServiceRequestId = request.ServiceRequestId, NewStatusId = statuses[0].ServiceRequestStatusId, Action = "Request created", ChangedAt = DateTime.Now.AddDays(-5) },
                new ServiceRequestHistory { ServiceRequestId = request.ServiceRequestId, OldStatusId = statuses[0].ServiceRequestStatusId, NewStatusId = statuses[1].ServiceRequestStatusId, Action = "Status changed", Note = "The academic office started the review.", ChangedAt = DateTime.Now.AddDays(-3) }
            );
            db.SaveChanges();
        }
    }
}