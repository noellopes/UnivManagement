using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    // Statuses a request goes through (submitted, in progress, resolved...) managed by the administrative services
    public class ServiceRequestStatusesController : Controller {
        private readonly UniversityDbContext _context;

        public ServiceRequestStatusesController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequestStatuses
        public async Task<IActionResult> Index() {
            // Number of requests that currently have each status
            ViewData["RequestCounts"] = await _context.ServiceRequests
                .GroupBy(r => r.ServiceRequestStatusId)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.StatusId, g => g.Count);

            return View(await _context.ServiceRequestStatuses
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.Name)
                .ToListAsync());
        }

        // GET: ServiceRequestStatuses/Create
        public async Task<IActionResult> Create() {
            // Suggest the next position of the list
            int lastOrder = await _context.ServiceRequestStatuses.MaxAsync(s => (int?)s.DisplayOrder) ?? 0;
            return View(new ServiceRequestStatus { Name = string.Empty, DisplayOrder = Math.Min(lastOrder + 1, 99) });
        }

        // POST: ServiceRequestStatuses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,DisplayOrder,IsFinal,RequiresNote,Color")] ServiceRequestStatus status) {
            await ValidateUniqueNameAsync(status);
            if (!ModelState.IsValid) {
                return View(status);
            }

            status.Name = status.Name.Trim();
            _context.Add(status);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"The status \"{status.Name}\" was added.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestStatuses/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var status = await _context.ServiceRequestStatuses.FindAsync(id);
            if (status == null) {
                return NotFound();
            }

            return View(status);
        }

        // POST: ServiceRequestStatuses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ServiceRequestStatusId,Name,Description,DisplayOrder,IsFinal,RequiresNote,Color")] ServiceRequestStatus status) {
            if (id != status.ServiceRequestStatusId) {
                return NotFound();
            }

            await ValidateUniqueNameAsync(status);
            if (!ModelState.IsValid) {
                return View(status);
            }

            bool? wasFinal = await _context.ServiceRequestStatuses
                .Where(s => s.ServiceRequestStatusId == id)
                .Select(s => (bool?)s.IsFinal)
                .FirstOrDefaultAsync();
            if (wasFinal == null) {
                return NotFound();
            }

            status.Name = status.Name.Trim();
            _context.Update(status);
            await _context.SaveChangesAsync();

            // The requests that have this status are closed or reopened with it
            if (wasFinal != status.IsFinal) {
                DateTime? closedAt = status.IsFinal ? DateTime.Now : null;
                await _context.ServiceRequests
                    .Where(r => r.ServiceRequestStatusId == id)
                    .ExecuteUpdateAsync(s => s.SetProperty(r => r.ClosedAt, closedAt));
            }

            TempData["Message"] = $"The status \"{status.Name}\" was updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestStatuses/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var status = await _context.ServiceRequestStatuses.FindAsync(id);
            if (status == null) {
                return NotFound();
            }

            ViewData["RequestCount"] = await _context.ServiceRequests.CountAsync(r => r.ServiceRequestStatusId == id);
            return View(status);
        }

        // POST: ServiceRequestStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var status = await _context.ServiceRequestStatuses.FindAsync(id);
            if (status == null) {
                return RedirectToAction(nameof(Index));
            }

            // A status that some requests have cannot be deleted
            if (await _context.ServiceRequests.AnyAsync(r => r.ServiceRequestStatusId == id)) {
                TempData["Error"] = $"Some requests have the status \"{status.Name}\", so it cannot be deleted.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try {
                _context.ServiceRequestStatuses.Remove(status);
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                // The database does not allow deleting a status that is still used, e.g. in the history of a request
                TempData["Error"] = $"The status \"{status.Name}\" is still in use and cannot be deleted.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Message"] = $"The status \"{status.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }

        // POST: ServiceRequestStatuses/CreateDefaults
        // Adds the usual statuses, only while the list is still empty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDefaults() {
            if (!await _context.ServiceRequestStatuses.AnyAsync()) {
                _context.ServiceRequestStatuses.AddRange(
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
                        Description = "The request was cancelled." });
                await _context.SaveChangesAsync();

                TempData["Message"] = "The default statuses were added.";
            }

            return RedirectToAction(nameof(Index));
        }

        // The database also has a unique index; this check shows a friendly message in the form
        private async Task ValidateUniqueNameAsync(ServiceRequestStatus status) {
            if (string.IsNullOrWhiteSpace(status.Name)) {
                return;
            }

            var name = status.Name.Trim();
            bool exists = await _context.ServiceRequestStatuses.AnyAsync(s =>
                s.Name == name && s.ServiceRequestStatusId != status.ServiceRequestStatusId);
            if (exists) {
                ModelState.AddModelError(nameof(status.Name), "There is already a status with this name.");
            }
        }
    }
}
