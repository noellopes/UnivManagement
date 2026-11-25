using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    public class ServiceRequestsController : Controller {
        private const string CancelledStatusName = "Cancelled";

        private readonly UniversityDbContext _context;

        public ServiceRequestsController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequests?search=...&status=2&type=1
        public async Task<IActionResult> Index(string? search, int? status, int? type, string? msg = null) {
            if (msg != null) {
                ViewBag.SuccessMessage = msg;
            }

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
        public async Task<IActionResult> Details(int? id, string? msg = null) {
            if (id == null) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return View("NotFound");
            }

            if (msg != null) {
                ViewBag.SuccessMessage = msg;
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

            return RedirectToAction(
                nameof(Details),
                new { id = serviceRequest.ServiceRequestId, msg = "Request created successfully." });
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return View("NotFound");
            }

            ViewBag.IsClosed = serviceRequest.IsClosed();
            ViewBag.StatusName = serviceRequest.Status?.Name;
            await PopulateListsAsync(serviceRequest.ServiceRequestTypeId, serviceRequest.ServiceRequestStatusId, forEdit: true);
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

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return View("NotFound");
            }

            await ValidateTypeAsync(posted.ServiceRequestTypeId);

            // A closed request keeps its status. For an open one the status must be in the list,
            // and cancelling has its own button
            var closed = serviceRequest.IsClosed();
            ServiceRequestStatus? status = null;
            if (!closed) {
                status = await _context.ServiceRequestStatuses.FindAsync(posted.ServiceRequestStatusId);
                if (status == null) {
                    ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestStatusId), "Please choose a status from the list.");
                } else if (status.Name == CancelledStatusName) {
                    ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestStatusId), "To cancel a request, use the Cancel request button in its details.");
                }
            }

            if (!ModelState.IsValid) {
                ViewBag.IsClosed = closed;
                ViewBag.StatusName = serviceRequest.Status?.Name;
                await PopulateListsAsync(posted.ServiceRequestTypeId, posted.ServiceRequestStatusId, forEdit: true);
                return View(posted);
            }

            // Copy only the editable fields; CreatedAt is kept from the database
            serviceRequest.Title = posted.Title;
            serviceRequest.Description = posted.Description;
            serviceRequest.ServiceRequestTypeId = posted.ServiceRequestTypeId;
            serviceRequest.RequesterName = posted.RequesterName;
            serviceRequest.RequesterEmail = posted.RequesterEmail;
            serviceRequest.AssignedTo = string.IsNullOrWhiteSpace(posted.AssignedTo) ? null : posted.AssignedTo.Trim();
            serviceRequest.UpdatedAt = DateTime.Now;

            // Only an open request can change status; a status that closes it sets the closing date
            if (!closed) {
                serviceRequest.Status = status;
                if (status!.IsFinal) {
                    serviceRequest.ClosedAt = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = serviceRequest.ServiceRequestId, msg = "Request edited successfully." });
        }

        // GET: ServiceRequests/Cancel/5
        public async Task<IActionResult> Cancel(int? id) {
            if (id == null) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return View("NotFound");
            }

            if (serviceRequest.IsClosed()) {
                ViewBag.ErrorMessage = "This request is closed, so it cannot be cancelled.";
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id) {
            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return View("NotFound");
            }

            if (serviceRequest.IsClosed()) {
                ViewBag.ErrorMessage = "This request is closed, so it cannot be cancelled.";
                return View("Cancel", serviceRequest);
            }

            // The statuses are configurable, so the cancelled one is found by its name
            var cancelled = await _context.ServiceRequestStatuses
                .FirstOrDefaultAsync(s => s.Name == CancelledStatusName && s.IsFinal);
            if (cancelled == null) {
                ViewBag.ErrorMessage = "There is no closing status called Cancelled. Add it in the statuses first.";
                return View("Cancel", serviceRequest);
            }

            serviceRequest.Status = cancelled;
            serviceRequest.UpdatedAt = DateTime.Now;
            serviceRequest.ClosedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = serviceRequest.ServiceRequestId, msg = "Request cancelled successfully." });
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
                return RedirectToAction(nameof(Index), new { msg = "The request was already deleted." });
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

            return RedirectToAction(nameof(Index), new { msg = "Request deleted successfully." });
        }

        // New requests start with the first status of the list that does not close the request
        private async Task<ServiceRequestStatus?> FindFirstStatusAsync() {
            return await _context.ServiceRequestStatuses
                .Where(s => !s.IsFinal)
                .OrderBy(s => s.DisplayOrder)
                .FirstOrDefaultAsync();
        }

        private async Task ValidateTypeAsync(int typeId) {
            // Leaving the select empty fails the model binding with a generic message, so ours replaces it
            ModelState.Remove(nameof(ServiceRequest.ServiceRequestTypeId));

            if (!await _context.ServiceRequestTypes.AnyAsync(t => t.ServiceRequestTypeId == typeId)) {
                ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestTypeId), "Please choose a request type from the list.");
            }
        }

        // Types and statuses for the select lists of the forms and the filters.
        // The edit form leaves out Cancelled, because cancelling has its own button
        private async Task PopulateListsAsync(int? selectedTypeId = null, int? selectedStatusId = null, bool forEdit = false) {
            var types = await _context.ServiceRequestTypes.OrderBy(t => t.Name).ToListAsync();
            ViewData["Types"] = new SelectList(types,
                nameof(ServiceRequestType.ServiceRequestTypeId), nameof(ServiceRequestType.Name), selectedTypeId);

            var statuses = await _context.ServiceRequestStatuses
                .Where(s => !forEdit || s.Name != CancelledStatusName)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
            ViewData["Statuses"] = new SelectList(statuses,
                nameof(ServiceRequestStatus.ServiceRequestStatusId), nameof(ServiceRequestStatus.Name), selectedStatusId);
        }
    }
}
