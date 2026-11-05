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

            var countsByStatus = await requests
                .GroupBy(r => r.ServiceRequestStatusId)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.StatusId, g => g.Count);
            var countsByType = await requests
                .GroupBy(r => r.ServiceRequestTypeId)
                .Select(g => new { TypeId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.TypeId, g => g.Count);

            var statuses = await _context.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToListAsync();
            var types = await _context.ServiceRequestTypes.OrderBy(t => t.Name).ToListAsync();

            var model = new ServiceRequestDashboardViewModel {
                TotalRequests = await requests.CountAsync(),
                OpenRequests = await requests.CountAsync(r => r.ClosedAt == null),
                ClosedRequests = await requests.CountAsync(r => r.ClosedAt != null),
                UnassignedRequests = await requests.CountAsync(r => r.ClosedAt == null && r.AssignedTo == null),

                // Statuses and types without requests are also listed, with 0
                RequestsByStatus = statuses
                    .Select(s => new KeyValuePair<ServiceRequestStatus, int>(s, countsByStatus.GetValueOrDefault(s.ServiceRequestStatusId)))
                    .ToList(),

                RequestsByType = types
                    .Select(t => new KeyValuePair<ServiceRequestType, int>(t, countsByType.GetValueOrDefault(t.ServiceRequestTypeId)))
                    .ToList(),

                LatestRequests = await requests
                    .Include(r => r.Status)
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}
