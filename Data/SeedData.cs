using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateServiceRequestPriorities(db);
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
    }
}
