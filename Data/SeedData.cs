using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Models.AccommodationManagement;

namespace UnivManagement.Data
{
    public class SeedData
    {
        internal static void Populate(UniversityDbContext? db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            PopulateFuncionarios(db);
            PopulateStudents(db);
            PopulateAccommodation(db);
        }

        private static void PopulateStudents(UniversityDbContext db)
        {
            var students = new[]
            {
                new Student
                {
                    StudentId = 0,
                    Name = "Tiago Mendes",
                    Email = "tiago.mendes@alunos.upg.pt",
                    NumStudent = 1707961m,
                    NIF = 251234567m,
                    Gender = "M",
                    Nationality = "Portuguesa",
                    BirthDate = new DateTime(2004, 5, 12),
                    Address = "Rua Soeiro Viegas 10, 6300-758 Guarda",
                    Telephone = "912345671"
                },
                new Student
                {
                    StudentId = 0,
                    Name = "Mariana Costa",
                    Email = "mariana.costa@alunos.upg.pt",
                    NumStudent = 1707962m,
                    NIF = 252345678m,
                    Gender = "F",
                    Nationality = "Portuguesa",
                    BirthDate = new DateTime(2004, 9, 23),
                    Address = "Avenida Sá Carneiro 24, 6300-559 Guarda",
                    Telephone = "912345672"
                },
                new Student
                {
                    StudentId = 0,
                    Name = "Diogo Almeida",
                    Email = "diogo.almeida@alunos.upg.pt",
                    NumStudent = 1707963m,
                    NIF = 253456789m,
                    Gender = "M",
                    Nationality = "Portuguesa",
                    BirthDate = new DateTime(2003, 11, 4),
                    Address = "Rua Direita 45, 6300-683 Guarda",
                    Telephone = "912345673"
                }
            };

            foreach (var student in students)
            {
                if (!db.Student.Any(s => s.NumStudent == student.NumStudent))
                {
                    db.Student.Add(student);
                }
            }

            db.SaveChanges();
        }

        private static void PopulateAccommodation(UniversityDbContext db)
        {
            PopulateAcademicYears(db);
            PopulateResidences(db);
            PopulatePossibleStatuses(db);
            PopulateRoomTypes(db);
            PopulateRoomStatuses(db);
            PopulateAccommodationApplications(db);
            PopulateApplicationStatusHistories(db);
            PopulateRooms(db);
            PopulateRoomAllocations(db);
            PopulateStayRecords(db);
            PopulateAccommodationContracts(db);
            PopulateAccommodationPayments(db);
        }

        private static void PopulateAcademicYears(UniversityDbContext db)
        {
            var years = new[]
            {
                (Start: 2025, End: 2026),
                (Start: 2026, End: 2027),
                (Start: 2027, End: 2028)
            };

            foreach (var year in years)
            {
                bool exists = db.AcademicYear.Any(a =>
                    a.StartYear == year.Start &&
                    a.EndYear == year.End);

                if (!exists)
                {
                    db.AcademicYear.Add(new AcademicYear
                    {
                        AcademicYearId = 0,
                        StartYear = year.Start,
                        EndYear = year.End
                    });
                }
            }

            db.SaveChanges();
        }

        private static void PopulateResidences(UniversityDbContext db)
        {
            var residences = new[]
            {
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
            };

            foreach (var residence in residences)
            {
                if (!db.Residence.Any(r => r.Name == residence.Name))
                {
                    db.Residence.Add(residence);
                }
            }

            db.SaveChanges();
        }

        private static void PopulatePossibleStatuses(UniversityDbContext db)
        {
            var statuses = new[]
            {
                "Submitted",
                "Approved",
                "Rejected",
                "Cancelled"
            };

            foreach (var name in statuses)
            {
                if (!db.PossibleStatus.Any(s => s.Name == name))
                {
                    db.PossibleStatus.Add(new PossibleStatus
                    {
                        Name = name
                    });
                }
            }

            db.SaveChanges();
        }

        // NOTA: Confirma nos modelos RoomType.cs e RoomStatus.cs se a propriedade se chama "Name".
        private static void PopulateRoomTypes(UniversityDbContext db)
        {
            var types = new[] { "Single", "Double", "Triple" };

            foreach (var name in types)
            {
                if (!db.RoomType.Any(t => t.Name == name))
                {
                    db.RoomType.Add(new RoomType { Name = name });
                }
            }

            db.SaveChanges();
        }

        private static void PopulateRoomStatuses(UniversityDbContext db)
        {
            var statuses = new[] { "Available", "Occupied", "Maintenance" };

            foreach (var name in statuses)
            {
                if (!db.RoomStatus.Any(s => s.Name == name))
                {
                    db.RoomStatus.Add(new RoomStatus { Name = name });
                }
            }

            db.SaveChanges();
        }

        private static void PopulateAccommodationApplications(UniversityDbContext db)
        {
            var students = db.Student
                .OrderBy(s => s.StudentId)
                .Take(3)
                .ToList();

            if (students.Count < 3)
                return;

            var year2026 = db.AcademicYear.FirstOrDefault(a =>
                a.StartYear == 2026 && a.EndYear == 2027);

            var year2027 = db.AcademicYear.FirstOrDefault(a =>
                a.StartYear == 2027 && a.EndYear == 2028);

            if (year2026 == null || year2027 == null)
                return;

            var applications = new[]
            {
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
            };

            foreach (var application in applications)
            {
                bool exists = db.AccommodationApplication.Any(a =>
                    a.StudentId == application.StudentId &&
                    a.AcademicYearId == application.AcademicYearId);

                if (!exists)
                {
                    db.AccommodationApplication.Add(application);
                }
            }

            db.SaveChanges();
        }

        private static void PopulateApplicationStatusHistories(UniversityDbContext db)
        {
            var firstApplication = db.AccommodationApplication
                .OrderBy(a => a.AccommodationApplicationId)
                .FirstOrDefault();

            var submittedStatus = db.PossibleStatus.FirstOrDefault(s => s.Name == "Submitted");
            var approvedStatus = db.PossibleStatus.FirstOrDefault(s => s.Name == "Approved");

            if (firstApplication == null || submittedStatus == null || approvedStatus == null)
                return;

            var histories = new[]
            {
                new ApplicationStatusHistory
                {
                    AccommodationApplicationId = firstApplication.AccommodationApplicationId,
                    PossibleStatusId = submittedStatus.PossibleStatusId,
                    ChangedAt = new DateTime(2026, 9, 2, 10, 15, 30),
                    ChangedBy = "System",
                    Notes = "Application submitted by student."
                },
                new ApplicationStatusHistory
                {
                    AccommodationApplicationId = firstApplication.AccommodationApplicationId,
                    PossibleStatusId = approvedStatus.PossibleStatusId,
                    ChangedAt = new DateTime(2026, 9, 5, 15, 0, 0),
                    ChangedBy = "Gestor_Residencia_1",
                    Notes = "Scholarship priority verified and approved."
                }
            };

            foreach (var history in histories)
            {
                bool exists = db.ApplicationStatusHistory.Any(h =>
                    h.AccommodationApplicationId == history.AccommodationApplicationId &&
                    h.PossibleStatusId == history.PossibleStatusId);

                if (!exists)
                {
                    db.ApplicationStatusHistory.Add(history);
                }
            }

            db.SaveChanges();
        }

        private static void PopulateRooms(UniversityDbContext db)
        {
            var feminina1 = db.Residence.FirstOrDefault(r => r.Name == "Feminina 1");
            var masculina1 = db.Residence.FirstOrDefault(r => r.Name == "Masculina 1");
            var masculina2 = db.Residence.FirstOrDefault(r => r.Name == "Masculina 2");

            var singleType = db.RoomType.FirstOrDefault(t => t.Name == "Single") ?? db.RoomType.FirstOrDefault();
            var doubleType = db.RoomType.FirstOrDefault(t => t.Name == "Double") ?? singleType;
            var tripleType = db.RoomType.FirstOrDefault(t => t.Name == "Triple") ?? singleType;

            var availableStatus = db.RoomStatus.FirstOrDefault(s => s.Name == "Available") ?? db.RoomStatus.FirstOrDefault();
            var occupiedStatus = db.RoomStatus.FirstOrDefault(s => s.Name == "Occupied") ?? availableStatus;

            if (feminina1 == null || masculina1 == null || masculina2 == null ||
                singleType == null || doubleType == null || tripleType == null ||
                availableStatus == null || occupiedStatus == null)
                return;

            var rooms = new[]
            {
                new Room1
                {
                    ResidenceId = feminina1.ResidenceId,
                    Number = "101",
                    Floor = 1,
                    CurrentOccupancy = 0,
                    Capacity = 1,
                    RoomTypeId = singleType.RoomTypeId,
                    RoomStatusId = availableStatus.RoomStatusId
                },
                new Room1
                {
                    ResidenceId = feminina1.ResidenceId,
                    Number = "102",
                    Floor = 1,
                    CurrentOccupancy = 0,
                    Capacity = 2,
                    RoomTypeId = doubleType.RoomTypeId,
                    RoomStatusId = availableStatus.RoomStatusId
                },
                new Room1
                {
                    ResidenceId = masculina1.ResidenceId,
                    Number = "204",
                    Floor = 2,
                    CurrentOccupancy = 0,
                    Capacity = 2,
                    RoomTypeId = doubleType.RoomTypeId,
                    RoomStatusId = availableStatus.RoomStatusId
                },
                new Room1
                {
                    ResidenceId = masculina2.ResidenceId,
                    Number = "305",
                    Floor = 3,
                    CurrentOccupancy = 0,
                    Capacity = 3,
                    RoomTypeId = tripleType.RoomTypeId,
                    RoomStatusId = occupiedStatus.RoomStatusId
                }
            };

            foreach (var room in rooms)
            {
                if (!db.Room.Any(r => r.Number == room.Number))
                {
                    db.Room.Add(room);
                }
            }

            db.SaveChanges();
        }

        private static void PopulateRoomAllocations(UniversityDbContext db)
        {
            var year2026 = db.AcademicYear.FirstOrDefault(a =>
                a.StartYear == 2026 && a.EndYear == 2027);

            var year2027 = db.AcademicYear.FirstOrDefault(a =>
                a.StartYear == 2027 && a.EndYear == 2028);

            var students = db.Student
                .OrderBy(s => s.StudentId)
                .Take(3)
                .ToList();

            if (students.Count < 3 || year2026 == null || year2027 == null)
                return;

            var application1 = db.AccommodationApplication.FirstOrDefault(a =>
                a.StudentId == students[0].StudentId &&
                a.AcademicYearId == year2026.AcademicYearId);

            var application2 = db.AccommodationApplication.FirstOrDefault(a =>
                a.StudentId == students[1].StudentId &&
                a.AcademicYearId == year2026.AcademicYearId);

            var application3 = db.AccommodationApplication.FirstOrDefault(a =>
                a.StudentId == students[2].StudentId &&
                a.AcademicYearId == year2027.AcademicYearId);

            var room101 = db.Room.FirstOrDefault(r => r.Number == "101");
            var room102 = db.Room.FirstOrDefault(r => r.Number == "102");
            var room204 = db.Room.FirstOrDefault(r => r.Number == "204");

            if (application1 == null || application2 == null || application3 == null ||
                room101 == null || room102 == null || room204 == null)
                return;

            var allocations = new[]
            {
                new RoomAllocation
                {
                    AccommodationApplicationId = application1.AccommodationApplicationId,
                    StudentId = application1.StudentId,
                    RoomId = room101.RoomId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    Status = AllocationStatus.Confirmed
                },
                new RoomAllocation
                {
                    AccommodationApplicationId = application2.AccommodationApplicationId,
                    StudentId = application2.StudentId,
                    RoomId = room204.RoomId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    Status = AllocationStatus.Confirmed
                },
                new RoomAllocation
                {
                    AccommodationApplicationId = application3.AccommodationApplicationId,
                    StudentId = application3.StudentId,
                    RoomId = room102.RoomId,
                    StartDate = new DateOnly(2027, 9, 15),
                    EndDate = new DateOnly(2028, 7, 15),
                    Status = AllocationStatus.Confirmed
                }
            };

            foreach (var allocation in allocations)
            {
                bool exists = db.RoomAllocation.Any(a =>
                    a.AccommodationApplicationId == allocation.AccommodationApplicationId &&
                    a.RoomId == allocation.RoomId);

                if (!exists)
                {
                    db.RoomAllocation.Add(allocation);
                }
            }

            db.SaveChanges();
        }

        private static void PopulateStayRecords(UniversityDbContext db)
        {
            var room101 = db.Room.FirstOrDefault(r => r.Number == "101");
            var room204 = db.Room.FirstOrDefault(r => r.Number == "204");
            var room102 = db.Room.FirstOrDefault(r => r.Number == "102");

            if (room101 == null || room204 == null || room102 == null)
                return;

            var allocation1 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room101.RoomId);

            var allocation2 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room204.RoomId);

            var allocation3 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room102.RoomId);

            if (allocation1 == null || allocation2 == null || allocation3 == null)
                return;

            var records = new[]
            {
                new StayRecord
                {
                    RoomAllocationId = allocation1.RoomAllocationId,
                    Type = StayType.CheckIn,
                    OccurredAt = new DateTime(2026, 9, 15, 10, 0, 0),
                    ConditionNotes = "Quarto entregue limpo, chave n.º 101A entregue.",
                    RecordedBy = "Gestor_Residencia_1"
                },
                new StayRecord
                {
                    RoomAllocationId = allocation2.RoomAllocationId,
                    Type = StayType.CheckIn,
                    OccurredAt = new DateTime(2026, 9, 15, 14, 30, 0),
                    ConditionNotes = "Sem anomalias na cama B e secretária.",
                    RecordedBy = "Gestor_Residencia_2"
                },
                new StayRecord
                {
                    RoomAllocationId = allocation3.RoomAllocationId,
                    Type = StayType.CheckIn,
                    OccurredAt = new DateTime(2027, 9, 15, 10, 30, 0),
                    ConditionNotes = "Quarto entregue limpo e em boas condições.",
                    RecordedBy = "Gestor_Residencia_1"
                }
            };

            foreach (var record in records)
            {
                bool exists = db.StayRecord.Any(s =>
                    s.RoomAllocationId == record.RoomAllocationId &&
                    s.Type == record.Type &&
                    s.OccurredAt == record.OccurredAt);

                if (!exists)
                {
                    db.StayRecord.Add(record);
                }
            }

            db.SaveChanges();
        }

        private static void PopulateAccommodationContracts(UniversityDbContext db)
        {
            var room101 = db.Room.FirstOrDefault(r => r.Number == "101");
            var room204 = db.Room.FirstOrDefault(r => r.Number == "204");
            var room102 = db.Room.FirstOrDefault(r => r.Number == "102");

            if (room101 == null || room204 == null || room102 == null)
                return;

            var allocation1 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room101.RoomId);

            var allocation2 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room204.RoomId);

            var allocation3 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room102.RoomId);

            if (allocation1 == null || allocation2 == null || allocation3 == null)
                return;

            var contracts = new[]
            {
                new AccommodationContract
                {
                    RoomAllocationId = allocation1.RoomAllocationId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    MonthlyRent = 85.00m,
                    Deposit = 85.00m,
                    ContractStatusId = 2,
                    SignedDocumentId = 9001
                },
                new AccommodationContract
                {
                    RoomAllocationId = allocation2.RoomAllocationId,
                    StartDate = new DateOnly(2026, 9, 15),
                    EndDate = new DateOnly(2027, 7, 15),
                    MonthlyRent = 110.00m,
                    Deposit = 110.00m,
                    ContractStatusId = 2,
                    SignedDocumentId = 9002
                },
                new AccommodationContract
                {
                    RoomAllocationId = allocation3.RoomAllocationId,
                    StartDate = new DateOnly(2027, 9, 15),
                    EndDate = new DateOnly(2028, 7, 15),
                    MonthlyRent = 110.00m,
                    Deposit = 110.00m,
                    ContractStatusId = 2,
                    SignedDocumentId = 9003
                }
            };

            foreach (var contract in contracts)
            {
                if (!db.AccommodationContract.Any(c =>
                    c.RoomAllocationId == contract.RoomAllocationId))
                {
                    db.AccommodationContract.Add(contract);
                }
            }

            db.SaveChanges();
        }

        private static void PopulateAccommodationPayments(UniversityDbContext db)
        {
            var room101 = db.Room.FirstOrDefault(r => r.Number == "101");
            var room204 = db.Room.FirstOrDefault(r => r.Number == "204");

            if (room101 == null || room204 == null)
                return;

            var allocation1 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room101.RoomId);

            var allocation2 = db.RoomAllocation.FirstOrDefault(a =>
                a.RoomId == room204.RoomId);

            if (allocation1 == null || allocation2 == null)
                return;

            var contract1 = db.AccommodationContract.FirstOrDefault(c =>
                c.RoomAllocationId == allocation1.RoomAllocationId);

            var contract2 = db.AccommodationContract.FirstOrDefault(c =>
                c.RoomAllocationId == allocation2.RoomAllocationId);

            if (contract1 == null || contract2 == null)
                return;

            var payments = new[]
            {
                new AccommodationPayment
                {
                    AccommodationContractId = contract1.AccommodationContractId,
                    DueDate = new DateOnly(2026, 10, 8),
                    Amount = 85.00m,
                    PaidAmount = 85.00m,
                    PaidAt = new DateTime(2026, 10, 5, 16, 22, 10),
                    Status = PaymentStatus.Paid
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contract1.AccommodationContractId,
                    DueDate = new DateOnly(2026, 11, 8),
                    Amount = 85.00m,
                    PaidAmount = 85.00m,
                    PaidAt = new DateTime(2026, 11, 7, 9, 14, 55),
                    Status = PaymentStatus.Paid
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contract2.AccommodationContractId,
                    DueDate = new DateOnly(2026, 10, 8),
                    Amount = 110.00m,
                    PaidAmount = 110.00m,
                    PaidAt = new DateTime(2026, 10, 8, 21, 3, 40),
                    Status = PaymentStatus.Paid
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contract2.AccommodationContractId,
                    DueDate = new DateOnly(2026, 11, 8),
                    Amount = 110.00m,
                    PaidAmount = 50.00m,
                    PaidAt = new DateTime(2026, 11, 8, 12, 0, 0),
                    Status = PaymentStatus.Overdue
                },
                new AccommodationPayment
                {
                    AccommodationContractId = contract1.AccommodationContractId,
                    DueDate = new DateOnly(2026, 12, 8),
                    Amount = 85.00m,
                    PaidAmount = 0.00m,
                    PaidAt = null,
                    Status = PaymentStatus.Pending
                }
            };

            foreach (var payment in payments)
            {
                bool exists = db.AccommodationPayment.Any(p =>
                    p.AccommodationContractId == payment.AccommodationContractId &&
                    p.DueDate == payment.DueDate);

                if (!exists)
                {
                    db.AccommodationPayment.Add(payment);
                }
            }

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