using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;

namespace UnivManagement.Controllers {
    // Lets a requester follow a request with its number and the email used to submit it
    public class ServiceRequestTrackingController : Controller {
        private readonly UniversityDbContext _context;

        public ServiceRequestTrackingController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequestTracking?number=5&email=name@example.com
        public async Task<IActionResult> Index(int? number, string? email) {
            ViewData["Number"] = number;
            ViewData["Email"] = email;

            if (number == null && string.IsNullOrWhiteSpace(email)) {
                return View(null);
            }

            if (number == null || string.IsNullOrWhiteSpace(email)) {
                ViewData["Error"] = "Enter both the request number and your email.";
                return View(null);
            }

            string trimmedEmail = email.Trim();
            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .Include(r => r.CatalogItem)
                .Include(r => r.History
                    .OrderByDescending(h => h.ChangedAt)
                    .ThenByDescending(h => h.ServiceRequestHistoryId))
                    .ThenInclude(h => h.NewStatus)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ServiceRequestId == number && r.RequesterEmail == trimmedEmail);

            // The same message is shown whether the number or the email is wrong
            if (serviceRequest == null) {
                ViewData["Error"] = "No request was found with that number and email.";
                return View(null);
            }

            // The progress bar shows "Submitted" while the request still has the first status of the list
            ViewData["FirstStatusId"] = await _context.ServiceRequestStatuses
                .Where(s => !s.IsFinal)
                .OrderBy(s => s.DisplayOrder)
                .Select(s => (int?)s.ServiceRequestStatusId)
                .FirstOrDefaultAsync();

            return View(serviceRequest);
        }
    }
}
