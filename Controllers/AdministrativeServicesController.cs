using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    public class AdministrativeServicesController : Controller {
        private readonly UniversityDbContext _context;

        public AdministrativeServicesController(UniversityDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var requests = _context.ServiceRequests;
            var openRequests = requests.Where(r => r.ClosedAt == null);
            var overdueRequests = openRequests.Where(r => r.DueDate < DateTime.Today);

            var closingTimes = await requests
                .Where(r => r.ClosedAt != null)
                .Select(r => new { r.CreatedAt, r.ClosedAt })
                .ToListAsync();

            var countsByStatus = await requests
                .GroupBy(r => r.ServiceRequestStatusId)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.StatusId, g => g.Count);
            var countsByType = await requests
                .GroupBy(r => r.ServiceRequestTypeId)
                .Select(g => new { TypeId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.TypeId, g => g.Count);
            var openCountsByPriority = await openRequests
                .GroupBy(r => r.ServiceRequestPriorityId)
                .Select(g => new { PriorityId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.PriorityId, g => g.Count);

            var statuses = await _context.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToListAsync();
            var types = await _context.ServiceRequestTypes.OrderBy(t => t.Name).ToListAsync();
            var priorities = await _context.ServiceRequestPriorities.OrderByDescending(p => p.Level).ToListAsync();

            var model = new ServiceRequestDashboardViewModel {
                TotalRequests = await requests.CountAsync(),
                OpenRequests = await openRequests.CountAsync(),
                ClosedRequests = closingTimes.Count,
                UnassignedRequests = await openRequests.CountAsync(r => r.AssignedTo == null),
                OverdueRequests = await overdueRequests.CountAsync(),

                AverageDaysToClose = closingTimes.Count == 0
                    ? null
                    : closingTimes.Average(r => (r.ClosedAt!.Value - r.CreatedAt).TotalDays),

                // Statuses and types without requests are also listed, with 0
                RequestsByStatus = statuses
                    .Select(s => new KeyValuePair<ServiceRequestStatus, int>(s, countsByStatus.GetValueOrDefault(s.ServiceRequestStatusId)))
                    .ToList(),

                RequestsByType = types
                    .Select(t => new KeyValuePair<ServiceRequestType, int>(t, countsByType.GetValueOrDefault(t.ServiceRequestTypeId)))
                    .ToList(),

                OpenRequestsByPriority = priorities
                    .Select(p => new KeyValuePair<ServiceRequestPriority, int>(p, openCountsByPriority.GetValueOrDefault(p.ServiceRequestPriorityId)))
                    .ToList(),

                LatestRequests = await requests
                    .Include(r => r.Status)
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(5)
                    .ToListAsync(),

                OverdueList = await overdueRequests
                    .Include(r => r.Priority)
                    .OrderBy(r => r.DueDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
