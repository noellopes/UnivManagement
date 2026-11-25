
using UnivManagement.Models;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateFuncionarios(db);
            PopulateAccommodation(db);
        }

        private static void PopulateAccommodation(UniversityDbContext db)
        {
            PopulateAcademicYears(db);
            PopulateResidences(db);
            PopulatePossibleStatuses(db);
            PopulateAccommodationApplications(db);
            PopulateRooms(db);
            PopulateRoomAllocations(db);
            PopulateStayRecords(db);
            PopulateAccommodationContracts(db);
            PopulateAccommodationPayments(db);
        }

        private static void PopulateAcademicYears(UniversityDbContext db)
        {
            if (db.AcademicYear.Any()) return;

            db.AcademicYear.AddRange(
                new AcademicYear { AcademicYearId = 0, StartYear = 2025, EndYear = 2026 },
                new AcademicYear { AcademicYearId = 0, StartYear = 2026, EndYear = 2027 },
                new AcademicYear { AcademicYearId = 0, StartYear = 2027, EndYear = 2028 }
            );

            db.SaveChanges();
        }

        private static void PopulateResidences(UniversityDbContext db)
        {
            if (db.Residence.Any()) return;

            db.Residence.AddRange(
                new Residence
                {
                    Name = "Feminina 1",
                    Address = "Rua Soeiro Viegas 6, 6300-758, Guarda",
                    Capacity = 3,
                    IsActive = true
                },
                new Residence
                {
                    Name = "Masculina 1",
                    Address = "Rua Soeiro Viegas 6, 6300-758, Guarda",
                    Capacity = 2,
                    IsActive = true
                },
                new Residence
                {
                    Name = "Feminina 2",
                    Address = "Rua Soeiro Viegas 6, 6300-758, Guarda",
                    Capacity = 1,
                    IsActive = true
                },
                new Residence
                {
                    Name = "Masculina 2",
                    Address = "Rua Soeiro Viegas 6, 6300-758, Guarda",
                    Capacity = 3,
                    IsActive = true
                }
            );

            db.SaveChanges();
        }

        private static void PopulatePossibleStatuses(UniversityDbContext db)
        {
            if (db.PossibleStatus.Any()) return;

            db.PossibleStatus.AddRange(
                new PossibleStatus { Name = "Submitted" },
                new PossibleStatus { Name = "Approved" },
                new PossibleStatus { Name = "Rejected" },
                new PossibleStatus { Name = "Cancelled" }
            );

            db.SaveChanges();
        }

        private static void PopulateAccommodationApplications(UniversityDbContext db)
        {
            if (db.AccommodationApplication.Any()) return;

            var students = db.Student
                .OrderBy(s => s.StudentId)
                .Take(3)
                .ToList();

            if (students.Count < 3)
            {
                throw new InvalidOperationException(
                    "At least three students must exist before seeding accommodation applications.");
            }

            var year2026 = db.AcademicYear.First(a =>
                a.StartYear == 2026 && a.EndYear == 2027);

            var year2027 = db.AcademicYear.First(a =>
                a.StartYear == 2027 && a.EndYear == 2028);

            db.AccommodationApplication.AddRange(
                new AccommodationApplication
                {
                    StudentId = students[0].StudentId,
                    AcademicYearId = year2026.AcademicYearId,
                    SubmittedAt = new DateTime(2026, 9, 2, 10, 15, 30),
                    HasScholarshipPriority = true,
                    PriorityCheckedAt = new DateTime(2026, 9, 2, 10, 15, 32)
                },
                new AccommodationApplication
                {
                    StudentId = students[1].StudentId,
                    AcademicYearId = year2026.AcademicYearId,
                    SubmittedAt = new DateTime(2026, 9, 3, 14, 42, 10),
                    HasScholarshipPriority = true,
                    PriorityCheckedAt = new DateTime(2026, 9, 3, 14, 42, 11),
                    PriorityOverrideReason = "Agregado familiar deslocado > 150km"
                },
                new AccommodationApplication
                {
                    StudentId = students[2].StudentId,
                    AcademicYearId = year2026.AcademicYearId,
                    SubmittedAt = new DateTime(2026, 9, 10, 18, 5, 0),
                    HasScholarshipPriority = false,
                    PriorityCheckedAt = new DateTime(2026, 9, 10, 18, 5, 2)
                },
                new AccommodationApplication
                {
                    StudentId = students[2].StudentId,
                    AcademicYearId = year2027.AcademicYearId,
                    SubmittedAt = new DateTime(2027, 6, 12, 11, 20, 0),
                    HasScholarshipPriority = false,
                    PriorityCheckedAt = new DateTime(2027, 6, 12, 11, 20, 3)
                }
            );

            db.SaveChanges();
        }

        private static void PopulateRooms(UniversityDbContext db)
        {
            if (db.Room.Any()) return;

            var feminina1 = db.Residence.First(r => r.Name == "Feminina 1");
            var masculina1 = db.Residence.First(r => r.Name == "Masculina 1");
            var masculina2 = db.Residence.First(r => r.Name == "Masculina 2");

            db.Room.AddRange(
                new Room1
                {
                    ResidenceId = feminina1.ResidenceId,
                    Number = "101",
                    Floor = 1,
                    CurrentOccupancy = 0,
                    Capacity = 1,
                    RoomTypeId = 1,
                    RoomStatusId = 1
                },
                new Room1
                {
                    ResidenceId = feminina1.ResidenceId,
                    Number = "102",
                    Floor = 1,
                    CurrentOccupancy = 0,
                    Capacity = 2,
                    RoomTypeId = 2,
                    RoomStatusId = 1
                },
                new Room1
                {
                    ResidenceId = masculina1.ResidenceId,
                    Number = "204",
                    Floor = 2,
                    CurrentOccupancy = 0,
                    Capacity = 2,
                    RoomTypeId = 2,
                    RoomStatusId = 1
                },
                new Room1
                {
                    ResidenceId = masculina2.ResidenceId,
                    Number = "305",
                    Floor = 3,
                    CurrentOccupancy = 0,
                    Capacity = 3,
                    RoomTypeId = 3,
                    RoomStatusId = 2
                }
            );

            db.SaveChanges();
        }

        private static void PopulateRoomAllocations(UniversityDbContext db)
        {
            if (db.RoomAllocation.Any()) return;

            var applications = db.AccommodationApplication
                .OrderBy(a => a.SubmittedAt)
                .ToList();

            if (applications.Count < 4)
            {
                throw new InvalidOperationException(
                    "At least four accommodation applications must exist before seeding room allocations.");
            }

            var room101 = db.Room.First(r => r.Number == "101");
            var room102 = db.Room.First(r => r.Number == "102");
            var room204 = db.Room.First(r => r.Number == "204");

            db.RoomAllocation.AddRange(
                new RoomAllocation
                {
                    AccommodationApplicationId = applications[0].AccommodationApplicationId,
                    StudentId = applications[0].StudentId,
                    RoomId = room101.RoomId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    Status = AllocationStatus.Confirmed
                },
                new RoomAllocation
                {
                    AccommodationApplicationId = applications[1].AccommodationApplicationId,
                    StudentId = applications[1].StudentId,
                    RoomId = room204.RoomId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    Status = AllocationStatus.Confirmed
                },
                new RoomAllocation
                {
                    AccommodationApplicationId = applications[3].AccommodationApplicationId,
                    StudentId = applications[3].StudentId,
                    RoomId = room102.RoomId,
                    StartDate = new DateOnly(2027, 9, 15),
                    EndDate = new DateOnly(2028, 7, 15),
                    Status = AllocationStatus.Confirmed
                }
            );

            db.SaveChanges();
        }

        private static void PopulateStayRecords(UniversityDbContext db)
        {
            if (db.StayRecord.Any()) return;

            var allocations = db.RoomAllocation
                .OrderBy(a => a.StartDate)
                .ToList();

            if (allocations.Count < 3)
            {
                throw new InvalidOperationException(
                    "At least three room allocations must exist before seeding stay records.");
            }

            db.StayRecord.AddRange(
                new StayRecord
                {
                    RoomAllocationId = allocations[0].RoomAllocationId,
                    Type = StayType.CheckIn,
                    OccurredAt = new DateTime(2026, 9, 15, 10, 0, 0),
                    ConditionNotes = "Quarto entregue limpo, chave n.º 101A entregue.",
                    RecordedBy = "Gestor_Residencia_1"
                },
                new StayRecord
                {
                    RoomAllocationId = allocations[1].RoomAllocationId,
                    Type = StayType.CheckIn,
                    OccurredAt = new DateTime(2026, 9, 15, 14, 30, 0),
                    ConditionNotes = "Sem anomalias na cama B e secretária.",
                    RecordedBy = "Gestor_Residencia_2"
                },
                new StayRecord
                {
                    RoomAllocationId = allocations[2].RoomAllocationId,
                    Type = StayType.CheckIn,
                    OccurredAt = new DateTime(2027, 9, 15, 10, 30, 0),
                    ConditionNotes = "Quarto entregue limpo e em boas condições.",
                    RecordedBy = "Gestor_Residencia_1"
                }
            );

            db.SaveChanges();
        }

        private static void PopulateAccommodationContracts(UniversityDbContext db)
        {
            if (db.AccommodationContract.Any()) return;

            var allocations = db.RoomAllocation
                .OrderBy(a => a.StartDate)
                .ToList();

            if (allocations.Count < 3)
            {
                throw new InvalidOperationException(
                    "At least three room allocations must exist before seeding accommodation contracts.");
            }

            db.AccommodationContract.AddRange(
                new AccommodationContract
                {
                    RoomAllocationId = allocations[0].RoomAllocationId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    MonthlyRent = 85.00m,
                    Deposit = 85.00m,
                    ContractStatusId = 2,
                    SignedDocumentId = 9001
                },
                new AccommodationContract
                {
                    RoomAllocationId = allocations[1].RoomAllocationId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    MonthlyRent = 110.00m,
                    Deposit = 110.00m,
                    ContractStatusId = 2,
                    SignedDocumentId = 9002
                },
                new AccommodationContract
                {
                    RoomAllocationId = allocations[2].RoomAllocationId,
                    StartDate = new DateOnly(2027, 9, 15),
                    EndDate = new DateOnly(2028, 7, 15),
                    MonthlyRent = 110.00m,
                    Deposit = 110.00m,
                    ContractStatusId = 2,
                    SignedDocumentId = 9003
                }
            );

            db.SaveChanges();
        }

        private static void PopulateAccommodationPayments(UniversityDbContext db)
        {
            if (db.AccommodationPayment.Any()) return;

            var contracts = db.AccommodationContract
                .OrderBy(c => c.StartDate)
                .ToList();

            if (contracts.Count < 2)
            {
                throw new InvalidOperationException(
                    "At least two accommodation contracts must exist before seeding payments.");
            }

            db.AccommodationPayment.AddRange(
                new AccommodationPayment
                {
                    AccommodationContractId = contracts[0].AccommodationContractId,
                    DueDate = new DateOnly(2026, 10, 8),
                    Amount = 85.00m,
                    PaidAmount = 85.00m,
                    PaidAt = new DateTime(2026, 10, 5, 16, 22, 10),
                    Status = PaymentStatus.Paid
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contracts[0].AccommodationContractId,
                    DueDate = new DateOnly(2026, 11, 8),
                    Amount = 85.00m,
                    PaidAmount = 85.00m,
                    PaidAt = new DateTime(2026, 11, 7, 9, 14, 55),
                    Status = PaymentStatus.Paid
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contracts[1].AccommodationContractId,
                    DueDate = new DateOnly(2026, 10, 8),
                    Amount = 110.00m,
                    PaidAmount = 110.00m,
                    PaidAt = new DateTime(2026, 10, 8, 21, 3, 40),
                    Status = PaymentStatus.Paid
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contracts[1].AccommodationContractId,
                    DueDate = new DateOnly(2026, 11, 8),
                    Amount = 110.00m,
                    PaidAmount = 50.00m,
                    PaidAt = new DateTime(2026, 11, 8, 12, 0, 0),
                    Status = PaymentStatus.Overdue
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contracts[0].AccommodationContractId,
                    DueDate = new DateOnly(2026, 12, 8),
                    Amount = 85.00m,
                    PaidAmount = 0.00m,
                    PaidAt = null,
                    Status = PaymentStatus.Pending
                }
            );

            db.SaveChanges();
        }

        // Método original: mantido sem alterações.
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
