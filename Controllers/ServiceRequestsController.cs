using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    public class ServiceRequestsController : Controller {
        private readonly UniversityDbContext _context;

        public ServiceRequestsController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequests?search=...&status=2&type=1
        public async Task<IActionResult> Index(string? search, int? status, int? type) {
            var requests = _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search)) {
                search = search.Trim();
                requests = requests.Where(r => r.Title.Contains(search)
                    || r.RequesterName.Contains(search)
                    || r.RequesterEmail.Contains(search));
            }
            if (status.HasValue) {
                requests = requests.Where(r => r.ServiceRequestStatusId == status.Value);
            }
            if (type.HasValue) {
                requests = requests.Where(r => r.ServiceRequestTypeId == type.Value);
            }

            // Keep the filter values so the view can show them again
            ViewData["Search"] = search;
            ViewData["Status"] = status;
            ViewData["Type"] = type;
            await PopulateListsAsync(type, status);

            return View(await requests.OrderByDescending(r => r.CreatedAt).ToListAsync());
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create() {
            await PopulateListsAsync();
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,ServiceRequestTypeId,RequesterName,RequesterEmail")] ServiceRequest serviceRequest) {
            await ValidateTypeAsync(serviceRequest.ServiceRequestTypeId);

            var firstStatus = await FindFirstStatusAsync();
            if (firstStatus == null) {
                ModelState.AddModelError(string.Empty, "There are no request statuses yet. Add them before creating requests.");
            }

            if (!ModelState.IsValid) {
                await PopulateListsAsync(serviceRequest.ServiceRequestTypeId);
                return View(serviceRequest);
            }

            // Status and dates are always set by the server, never by the form
            serviceRequest.ServiceRequestStatusId = firstStatus!.ServiceRequestStatusId;
            serviceRequest.CreatedAt = DateTime.Now;

            _context.Add(serviceRequest);
            await _context.SaveChangesAsync();

            TempData["Message"] = "The request was created.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null) {
                return NotFound();
            }

            await PopulateListsAsync(serviceRequest.ServiceRequestTypeId, serviceRequest.ServiceRequestStatusId);
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ServiceRequestId,Title,Description,ServiceRequestTypeId,ServiceRequestStatusId,RequesterName,RequesterEmail,AssignedTo")] ServiceRequest posted) {
            if (id != posted.ServiceRequestId) {
                return NotFound();
            }

            await ValidateTypeAsync(posted.ServiceRequestTypeId);
            var status = await _context.ServiceRequestStatuses.FindAsync(posted.ServiceRequestStatusId);
            if (status == null) {
                ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestStatusId), "Please choose a status from the list.");
            }

            if (!ModelState.IsValid) {
                await PopulateListsAsync(posted.ServiceRequestTypeId, posted.ServiceRequestStatusId);
                return View(posted);
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null) {
                return NotFound();
            }

            // Copy only the editable fields; CreatedAt is kept from the database
            serviceRequest.Title = posted.Title;
            serviceRequest.Description = posted.Description;
            serviceRequest.ServiceRequestTypeId = posted.ServiceRequestTypeId;
            serviceRequest.ServiceRequestStatusId = posted.ServiceRequestStatusId;
            serviceRequest.RequesterName = posted.RequesterName;
            serviceRequest.RequesterEmail = posted.RequesterEmail;
            serviceRequest.AssignedTo = string.IsNullOrWhiteSpace(posted.AssignedTo) ? null : posted.AssignedTo.Trim();
            serviceRequest.UpdatedAt = DateTime.Now;

            // A status that closes the request sets the closing date; any other status clears it
            serviceRequest.ClosedAt = status!.IsFinal ? serviceRequest.ClosedAt ?? DateTime.Now : null;

            await _context.SaveChangesAsync();

            TempData["Message"] = "The request was updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest != null) {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }

            TempData["Message"] = "The request was deleted.";
            return RedirectToAction(nameof(Index));
        }

        // New requests start with the first status of the list that does not close the request
        private async Task<ServiceRequestStatus?> FindFirstStatusAsync() {
            return await _context.ServiceRequestStatuses
                .Where(s => !s.IsFinal)
                .OrderBy(s => s.DisplayOrder)
                .FirstOrDefaultAsync();
        }

        private async Task ValidateTypeAsync(int typeId) {
            if (!await _context.ServiceRequestTypes.AnyAsync(t => t.ServiceRequestTypeId == typeId)) {
                ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestTypeId), "Please choose a request type from the list.");
            }
        }

        // Types and statuses for the select lists of the forms and the filters
        private async Task PopulateListsAsync(int? selectedTypeId = null, int? selectedStatusId = null) {
            var types = await _context.ServiceRequestTypes.OrderBy(t => t.Name).ToListAsync();
            ViewData["Types"] = new SelectList(types,
                nameof(ServiceRequestType.ServiceRequestTypeId), nameof(ServiceRequestType.Name), selectedTypeId);

            var statuses = await _context.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToListAsync();
            ViewData["Statuses"] = new SelectList(statuses,
                nameof(ServiceRequestStatus.ServiceRequestStatusId), nameof(ServiceRequestStatus.Name), selectedStatusId);
        }
    }
}
