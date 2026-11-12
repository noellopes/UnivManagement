using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateServiceRequestTypes(db);
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
    }
}
