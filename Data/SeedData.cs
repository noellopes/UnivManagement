using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateServiceRequests(db);
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
    }
}
