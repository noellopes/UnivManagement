using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    // Priorities of the requests (low, normal, high...) and the days the administrative services have to resolve them
    public class ServiceRequestPrioritiesController : Controller {
        private readonly UniversityDbContext _context;

        public ServiceRequestPrioritiesController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequestPriorities
        public async Task<IActionResult> Index() {
            // Number of requests that have each priority
            ViewData["RequestCounts"] = await _context.ServiceRequests
                .GroupBy(r => r.ServiceRequestPriorityId)
                .Select(g => new { PriorityId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.PriorityId, g => g.Count);

            return View(await _context.ServiceRequestPriorities
                .OrderByDescending(p => p.Level)
                .ThenBy(p => p.Name)
                .ToListAsync());
        }

        // GET: ServiceRequestPriorities/Create
        public IActionResult Create() {
            return View(new ServiceRequestPriority { Name = string.Empty });
        }

        // POST: ServiceRequestPriorities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,ResponseDays,Level,Color")] ServiceRequestPriority priority) {
            await ValidateUniqueNameAsync(priority);
            if (!ModelState.IsValid) {
                return View(priority);
            }

            priority.Name = priority.Name.Trim();
            _context.Add(priority);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"The priority \"{priority.Name}\" was added.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestPriorities/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var priority = await _context.ServiceRequestPriorities.FindAsync(id);
            if (priority == null) {
                return NotFound();
            }

            return View(priority);
        }

        // POST: ServiceRequestPriorities/Edit/5
        // A new response time is used for the requests created from now on; existing due dates are kept
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ServiceRequestPriorityId,Name,ResponseDays,Level,Color")] ServiceRequestPriority priority) {
            if (id != priority.ServiceRequestPriorityId) {
                return NotFound();
            }

            await ValidateUniqueNameAsync(priority);
            if (!ModelState.IsValid) {
                return View(priority);
            }

            try {
                priority.Name = priority.Name.Trim();
                _context.Update(priority);
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!await _context.ServiceRequestPriorities.AnyAsync(p => p.ServiceRequestPriorityId == id)) {
                    return NotFound();
                }
                throw;
            }

            TempData["Message"] = $"The priority \"{priority.Name}\" was updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestPriorities/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var priority = await _context.ServiceRequestPriorities.FindAsync(id);
            if (priority == null) {
                return NotFound();
            }

            ViewData["RequestCount"] = await _context.ServiceRequests.CountAsync(r => r.ServiceRequestPriorityId == id);
            return View(priority);
        }

        // POST: ServiceRequestPriorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var priority = await _context.ServiceRequestPriorities.FindAsync(id);
            if (priority == null) {
                return RedirectToAction(nameof(Index));
            }

            // A priority that some requests have cannot be deleted
            if (await _context.ServiceRequests.AnyAsync(r => r.ServiceRequestPriorityId == id)) {
                TempData["Error"] = $"Some requests have the priority \"{priority.Name}\", so it cannot be deleted.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try {
                _context.ServiceRequestPriorities.Remove(priority);
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                // A request may have been given this priority in the meantime
                TempData["Error"] = $"The priority \"{priority.Name}\" is still in use and cannot be deleted.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Message"] = $"The priority \"{priority.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }

        // POST: ServiceRequestPriorities/CreateDefaults
        // Adds the usual priorities, only while the list is still empty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDefaults() {
            if (!await _context.ServiceRequestPriorities.AnyAsync()) {
                _context.ServiceRequestPriorities.AddRange(
                    new ServiceRequestPriority { Name = "Low", Level = 1, ResponseDays = 15, Color = "light" },
                    new ServiceRequestPriority { Name = "Normal", Level = 2, ResponseDays = 7, Color = "info" },
                    new ServiceRequestPriority { Name = "High", Level = 3, ResponseDays = 3, Color = "warning" },
                    new ServiceRequestPriority { Name = "Urgent", Level = 4, ResponseDays = 1, Color = "danger" });
                await _context.SaveChangesAsync();

                TempData["Message"] = "The default priorities were added.";
            }

            return RedirectToAction(nameof(Index));
        }

        // The database also has a unique index; this check shows a friendly message in the form
        private async Task ValidateUniqueNameAsync(ServiceRequestPriority priority) {
            if (string.IsNullOrWhiteSpace(priority.Name)) {
                return;
            }

            var name = priority.Name.Trim();
            bool exists = await _context.ServiceRequestPriorities.AnyAsync(p =>
                p.Name == name && p.ServiceRequestPriorityId != priority.ServiceRequestPriorityId);
            if (exists) {
                ModelState.AddModelError(nameof(priority.Name), "There is already a priority with this name.");
            }
        }
    }
}
