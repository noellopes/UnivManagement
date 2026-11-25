using UnivManagement.Data;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Models.AccommodationManagement;

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
    var years = new[]
    {
        (Start: 2025, End: 2026),
        (Start: 2026, End: 2027),
        (Start: 2027, End: 2028)
    };

    foreach (var year in years)
    {
        bool exists = db.AcademicYear.Any(a =>
            a.StartYear == year.Start && a.EndYear == year.End);

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

private static void PopulateAccommodationApplications(UniversityDbContext db)
{
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

private static void PopulateRooms(UniversityDbContext db)
{
    var feminina1 = db.Residence.First(r => r.Name == "Feminina 1");
    var masculina1 = db.Residence.First(r => r.Name == "Masculina 1");
    var masculina2 = db.Residence.First(r => r.Name == "Masculina 2");

    var rooms = new[]
    {
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
    var year2026 = db.AcademicYear.First(a =>
        a.StartYear == 2026 && a.EndYear == 2027);

    var year2027 = db.AcademicYear.First(a =>
        a.StartYear == 2027 && a.EndYear == 2028);

    var students = db.Student
        .OrderBy(s => s.StudentId)
        .Take(3)
        .ToList();

    if (students.Count < 3)
    {
        throw new InvalidOperationException(
            "At least three students must exist before seeding room allocations.");
    }

    var application1 = db.AccommodationApplication.First(a =>
        a.StudentId == students[0].StudentId &&
        a.AcademicYearId == year2026.AcademicYearId);

    var application2 = db.AccommodationApplication.First(a =>
        a.StudentId == students[1].StudentId &&
        a.AcademicYearId == year2026.AcademicYearId);

    var application3 = db.AccommodationApplication.First(a =>
        a.StudentId == students[2].StudentId &&
        a.AcademicYearId == year2027.AcademicYearId);

    var room101 = db.Room.First(r => r.Number == "101");
    var room102 = db.Room.First(r => r.Number == "102");
    var room204 = db.Room.First(r => r.Number == "204");

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
    var allocations = db.RoomAllocation.ToList();

    var room101 = db.Room.First(r => r.Number == "101");
    var room204 = db.Room.First(r => r.Number == "204");
    var room102 = db.Room.First(r => r.Number == "102");

    var allocation1 = allocations.First(a =>
        a.RoomId == room101.RoomId);

    var allocation2 = allocations.First(a =>
        a.RoomId == room204.RoomId);

    var allocation3 = allocations.First(a =>
        a.RoomId == room102.RoomId);

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
    var room101 = db.Room.First(r => r.Number == "101");
    var room204 = db.Room.First(r => r.Number == "204");
    var room102 = db.Room.First(r => r.Number == "102");

    var allocation1 = db.RoomAllocation.First(a => a.RoomId == room101.RoomId);
    var allocation2 = db.RoomAllocation.First(a => a.RoomId == room204.RoomId);
    var allocation3 = db.RoomAllocation.First(a => a.RoomId == room102.RoomId);

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
    var room101 = db.Room.First(r => r.Number == "101");
    var room204 = db.Room.First(r => r.Number == "204");

    var allocation1 = db.RoomAllocation.First(a => a.RoomId == room101.RoomId);
    var allocation2 = db.RoomAllocation.First(a => a.RoomId == room204.RoomId);

    var contract1 = db.AccommodationContract.First(c =>
        c.RoomAllocationId == allocation1.RoomAllocationId);

    var contract2 = db.AccommodationContract.First(c =>
        c.RoomAllocationId == allocation2.RoomAllocationId);

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