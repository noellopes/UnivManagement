using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    // Catalog of services that can be requested (certificates, documents, support...)
    public class ServiceRequestCatalogItemsController : Controller {
        private readonly UniversityDbContext _context;

        public ServiceRequestCatalogItemsController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequestCatalogItems?search=...&type=1&showUnavailable=true
        public async Task<IActionResult> Index(string? search, int? type, bool showUnavailable = false) {
            var items = _context.ServiceRequestCatalogItems
                .Include(i => i.Type)
                .Include(i => i.Requests)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search)) {
                search = search.Trim();
                items = items.Where(i => i.Name.Contains(search) || (i.Description != null && i.Description.Contains(search)));
            }
            if (type.HasValue) {
                items = items.Where(i => i.ServiceRequestTypeId == type.Value);
            }
            if (!showUnavailable) {
                items = items.Where(i => i.IsActive);
            }

            ViewData["Search"] = search;
            ViewData["Type"] = type;
            ViewData["ShowUnavailable"] = showUnavailable;
            await PopulateTypesAsync(type);

            return View(await items.OrderBy(i => i.Type!.Name).ThenBy(i => i.Name).ToListAsync());
        }

        // GET: ServiceRequestCatalogItems/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var item = await _context.ServiceRequestCatalogItems
                .Include(i => i.Type)
                .Include(i => i.Requests.OrderByDescending(r => r.CreatedAt))
                    .ThenInclude(r => r.Status)
                .FirstOrDefaultAsync(i => i.ServiceRequestCatalogItemId == id);
            if (item == null) {
                return NotFound();
            }

            return View(item);
        }

        // GET: ServiceRequestCatalogItems/Create
        public async Task<IActionResult> Create() {
            await PopulateTypesAsync();
            return View();
        }

        // POST: ServiceRequestCatalogItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,ServiceRequestTypeId,ProcessingDays,RequiredDocuments,IsActive")] ServiceRequestCatalogItem item) {
            await ValidateUniqueNameAsync(item);
            await ValidateTypeAsync(item.ServiceRequestTypeId);
            if (!ModelState.IsValid) {
                await PopulateTypesAsync(item.ServiceRequestTypeId);
                return View(item);
            }

            item.Name = item.Name.Trim();
            _context.Add(item);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"The service \"{item.Name}\" was added to the catalog.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestCatalogItems/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var item = await _context.ServiceRequestCatalogItems.FindAsync(id);
            if (item == null) {
                return NotFound();
            }

            await PopulateTypesAsync(item.ServiceRequestTypeId);
            return View(item);
        }

        // POST: ServiceRequestCatalogItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ServiceRequestCatalogItemId,Name,Description,ServiceRequestTypeId,ProcessingDays,RequiredDocuments,IsActive")] ServiceRequestCatalogItem item) {
            if (id != item.ServiceRequestCatalogItemId) {
                return NotFound();
            }

            await ValidateUniqueNameAsync(item);
            await ValidateTypeAsync(item.ServiceRequestTypeId);
            if (!ModelState.IsValid) {
                await PopulateTypesAsync(item.ServiceRequestTypeId);
                return View(item);
            }

            item.Name = item.Name.Trim();
            try {
                _context.Update(item);
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!await _context.ServiceRequestCatalogItems.AnyAsync(i => i.ServiceRequestCatalogItemId == id)) {
                    return NotFound();
                }
                throw;
            }

            TempData["Message"] = $"The service \"{item.Name}\" was updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequestCatalogItems/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var item = await _context.ServiceRequestCatalogItems
                .Include(i => i.Type)
                .Include(i => i.Requests)
                .FirstOrDefaultAsync(i => i.ServiceRequestCatalogItemId == id);
            if (item == null) {
                return NotFound();
            }

            return View(item);
        }

        // POST: ServiceRequestCatalogItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var item = await _context.ServiceRequestCatalogItems.FindAsync(id);
            if (item == null) {
                return RedirectToAction(nameof(Index));
            }

            // A service that was already requested cannot be deleted, only made unavailable
            if (await _context.ServiceRequests.AnyAsync(r => r.ServiceRequestCatalogItemId == id)) {
                TempData["Error"] = $"\"{item.Name}\" has requests and cannot be deleted. Edit it and mark it as unavailable instead.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.ServiceRequestCatalogItems.Remove(item);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"The service \"{item.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }

        // The database also has a unique index; this check shows a friendly message in the form
        private async Task ValidateUniqueNameAsync(ServiceRequestCatalogItem item) {
            if (string.IsNullOrWhiteSpace(item.Name)) {
                return;
            }

            var name = item.Name.Trim();
            bool exists = await _context.ServiceRequestCatalogItems.AnyAsync(i =>
                i.Name == name && i.ServiceRequestCatalogItemId != item.ServiceRequestCatalogItemId);
            if (exists) {
                ModelState.AddModelError(nameof(item.Name), "There is already a service with this name in the catalog.");
            }
        }

        private async Task ValidateTypeAsync(int typeId) {
            if (!await _context.ServiceRequestTypes.AnyAsync(t => t.ServiceRequestTypeId == typeId)) {
                ModelState.AddModelError(nameof(ServiceRequestCatalogItem.ServiceRequestTypeId), "Please choose a request type from the list.");
            }
        }

        // Request types for the select list of the forms and the filter
        private async Task PopulateTypesAsync(int? selectedTypeId = null) {
            var types = await _context.ServiceRequestTypes.OrderBy(t => t.Name).ToListAsync();
            ViewData["Types"] = new SelectList(types,
                nameof(ServiceRequestType.ServiceRequestTypeId), nameof(ServiceRequestType.Name), selectedTypeId);
        }
    }
}
