using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    // Request types (certificate, official document, student support...) managed by the administrative services
    public class ServiceRequestTypesController : Controller {
        private readonly UniversityDbContext _context;

        public ServiceRequestTypesController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequestTypes
        public async Task<IActionResult> Index() {
            // Number of requests of each type, shown in the list
            ViewData["RequestCounts"] = await _context.ServiceRequests
                .GroupBy(r => r.ServiceRequestTypeId)
                .Select(g => new { TypeId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.TypeId, g => g.Count);

            return View(await _context.ServiceRequestTypes.OrderBy(t => t.Name).ToListAsync());
        }

        // GET: ServiceRequestTypes/Create
        public IActionResult Create() {
            return View();
        }

        // POST: ServiceRequestTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] ServiceRequestType type) {
            await ValidateUniqueNameAsync(type);
            if (!ModelState.IsValid) {
                return View(type);
            }

            type.Name = type.Name.Trim();
            _context.Add(type);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"The request type \"{type.Name}\" was added.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestTypes/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var type = await _context.ServiceRequestTypes.FindAsync(id);
            if (type == null) {
                return NotFound();
            }

            return View(type);
        }

        // POST: ServiceRequestTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceRequestTypeId,Name,Description")] ServiceRequestType type) {
            if (id != type.ServiceRequestTypeId) {
                return NotFound();
            }

            await ValidateUniqueNameAsync(type);
            if (!ModelState.IsValid) {
                return View(type);
            }

            type.Name = type.Name.Trim();
            try {
                _context.Update(type);
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!await _context.ServiceRequestTypes.AnyAsync(t => t.ServiceRequestTypeId == id)) {
                    return NotFound();
                }
                throw;
            }

            TempData["Message"] = $"The request type \"{type.Name}\" was updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestTypes/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var type = await _context.ServiceRequestTypes.FindAsync(id);
            if (type == null) {
                return NotFound();
            }

            ViewData["RequestCount"] = await _context.ServiceRequests.CountAsync(r => r.ServiceRequestTypeId == id);
            return View(type);
        }

        // POST: ServiceRequestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var type = await _context.ServiceRequestTypes.FindAsync(id);
            if (type == null) {
                return RedirectToAction(nameof(Index));
            }

            // A type that is used by requests cannot be deleted
            if (await _context.ServiceRequests.AnyAsync(r => r.ServiceRequestTypeId == id)) {
                TempData["Error"] = $"\"{type.Name}\" is used by some requests and cannot be deleted.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try {
                _context.ServiceRequestTypes.Remove(type);
                await _context.SaveChangesAsync();
            } catch (DbUpdateException) {
                // The database does not allow deleting a type that is still used somewhere else
                TempData["Error"] = $"\"{type.Name}\" is still in use and cannot be deleted.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["Message"] = $"The request type \"{type.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }

        // POST: ServiceRequestTypes/CreateDefaults
        // Adds the usual request types, only while the list is still empty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDefaults() {
            if (!await _context.ServiceRequestTypes.AnyAsync()) {
                _context.ServiceRequestTypes.AddRange(
                    new ServiceRequestType { Name = "Certificate", Description = "Certificates of enrolment, grades or course completion." },
                    new ServiceRequestType { Name = "Official document", Description = "Official documents such as diplomas or academic transcripts." },
                    new ServiceRequestType { Name = "Student support", Description = "Help with academic or administrative problems." },
                    new ServiceRequestType { Name = "Maintenance", Description = "Problems in campus buildings, rooms or equipment." },
                    new ServiceRequestType { Name = "Other", Description = "Requests that do not fit in any other type." });
                await _context.SaveChangesAsync();

                TempData["Message"] = "The default request types were added.";
            }

            return RedirectToAction(nameof(Index));
        }

        // The database also has a unique index; this check shows a friendly message in the form
        private async Task ValidateUniqueNameAsync(ServiceRequestType type) {
            if (string.IsNullOrWhiteSpace(type.Name)) {
                return;
            }

            var name = type.Name.Trim();
            bool exists = await _context.ServiceRequestTypes.AnyAsync(t =>
                t.Name == name && t.ServiceRequestTypeId != type.ServiceRequestTypeId);
            if (exists) {
                ModelState.AddModelError(nameof(type.Name), "There is already a request type with this name.");
            }
        }
    }
}
