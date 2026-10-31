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
                .Include(r => r.History
                    .OrderByDescending(h => h.ChangedAt)
                    .ThenByDescending(h => h.ServiceRequestHistoryId))
                    .ThenInclude(h => h.OldStatus)
                .Include(r => r.History)
                    .ThenInclude(h => h.NewStatus)
                .Include(r => r.Comments.OrderBy(c => c.CreatedAt))
                .AsSplitQuery()
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return NotFound();
            }

            // The statuses for the process form of the details page
            await PopulateListsAsync(selectedStatusId: serviceRequest.ServiceRequestStatusId);
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
            serviceRequest.History.Add(new ServiceRequestHistory {
                Action = "Request submitted",
                NewStatusId = firstStatus.ServiceRequestStatusId
            });

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

            var assignedTo = string.IsNullOrWhiteSpace(posted.AssignedTo) ? null : posted.AssignedTo.Trim();

            bool detailsChanged = serviceRequest.Title != posted.Title
                || serviceRequest.Description != posted.Description
                || serviceRequest.ServiceRequestTypeId != posted.ServiceRequestTypeId
                || serviceRequest.RequesterName != posted.RequesterName
                || serviceRequest.RequesterEmail != posted.RequesterEmail;
            if (detailsChanged) {
                serviceRequest.History.Add(new ServiceRequestHistory { Action = "Request details updated" });
            }
            RecordChanges(serviceRequest, status!, assignedTo, null);

            // Copy only the editable fields; CreatedAt is kept from the database
            serviceRequest.Title = posted.Title;
            serviceRequest.Description = posted.Description;
            serviceRequest.ServiceRequestTypeId = posted.ServiceRequestTypeId;
            serviceRequest.ServiceRequestStatusId = posted.ServiceRequestStatusId;
            serviceRequest.RequesterName = posted.RequesterName;
            serviceRequest.RequesterEmail = posted.RequesterEmail;
            serviceRequest.AssignedTo = assignedTo;
            UpdateDates(serviceRequest, status!);

            await _context.SaveChangesAsync();

            TempData["Message"] = "The request was updated.";
            return RedirectToAction(nameof(Index));
        }

        // POST: ServiceRequests/Process/5
        // Quick update from the details page: change the status, the assignment and/or add a note
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(int id, int serviceRequestStatusId, string? assignedTo, string? note) {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null) {
                return NotFound();
            }

            var status = await _context.ServiceRequestStatuses.FindAsync(serviceRequestStatusId);
            assignedTo = string.IsNullOrWhiteSpace(assignedTo) ? null : assignedTo.Trim();
            note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
            bool statusChanged = serviceRequestStatusId != serviceRequest.ServiceRequestStatusId;

            string? error = null;
            if (status == null) {
                error = "Please choose a status from the list.";
            } else if (note?.Length > 500) {
                error = "The note cannot exceed 500 characters.";
            } else if (assignedTo?.Length > 200) {
                error = "The assigned to field cannot exceed 200 characters.";
            } else if (note == null && statusChanged && status.RequiresNote) {
                // The requester must know why the request was rejected or what is missing
                error = $"Please add a note explaining the reason for the status \"{status.Name}\".";
            } else if (!statusChanged && assignedTo == serviceRequest.AssignedTo && note == null) {
                error = "There is nothing to update.";
            }

            if (error != null) {
                TempData["Error"] = error;
                return RedirectToAction(nameof(Details), new { id });
            }

            RecordChanges(serviceRequest, status!, assignedTo, note);
            serviceRequest.ServiceRequestStatusId = status!.ServiceRequestStatusId;
            serviceRequest.AssignedTo = assignedTo;
            UpdateDates(serviceRequest, status);

            await _context.SaveChangesAsync();

            TempData["Message"] = "The request was updated.";
            return RedirectToAction(nameof(Details), new { id });
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

        // Adds one history entry for each change of status or assignment.
        // If there is a note but nothing else changed, the note is saved on its own.
        private static void RecordChanges(ServiceRequest request, ServiceRequestStatus newStatus, string? newAssignedTo, string? note) {
            if (request.ServiceRequestStatusId != newStatus.ServiceRequestStatusId) {
                request.History.Add(new ServiceRequestHistory {
                    Action = "Status changed",
                    OldStatusId = request.ServiceRequestStatusId,
                    NewStatusId = newStatus.ServiceRequestStatusId,
                    Note = note
                });
                note = null;
            }

            if (request.AssignedTo != newAssignedTo) {
                request.History.Add(new ServiceRequestHistory {
                    Action = newAssignedTo == null ? "Assignment removed" : $"Assigned to {newAssignedTo}",
                    Note = note
                });
                note = null;
            }

            if (note != null) {
                request.History.Add(new ServiceRequestHistory { Action = "Note added", Note = note });
            }
        }

        // A status that closes the request sets the closing date; any other status clears it
        private static void UpdateDates(ServiceRequest request, ServiceRequestStatus status) {
            request.UpdatedAt = DateTime.Now;
            request.ClosedAt = status.IsFinal ? request.ClosedAt ?? DateTime.Now : null;
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
