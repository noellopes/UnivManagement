using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;

namespace UnivManagement.Data {
    public class SeedData {
        internal static void Populate(UniversityDbContext? db) {
            if (db == null) throw new ArgumentNullException(nameof(db));

            PopulateServiceRequestAttachments(db);
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
    }
}
