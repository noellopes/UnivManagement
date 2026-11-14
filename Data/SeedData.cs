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
    }
}