using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateServiceRequestStatuses(db);
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
    }
}
