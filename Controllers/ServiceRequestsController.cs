using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    public class ServiceRequestsController : Controller {
        private const int PageSize = 10;

        private readonly UniversityDbContext _context;

        public ServiceRequestsController(UniversityDbContext context) {
            _context = context;
        }

        // GET: ServiceRequests?search=...&status=2&type=1&priority=3&overdue=true&page=2
        public async Task<IActionResult> Index(string? search, int? status, int? type, int? priority,
            bool overdue = false, int page = 1) {
            var requests = _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .Include(r => r.Priority)
                .Include(r => r.CatalogItem)
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
            if (priority.HasValue) {
                requests = requests.Where(r => r.ServiceRequestPriorityId == priority.Value);
            }
            if (overdue) {
                // Same rule as ServiceRequest.IsOverdue, written so EF can translate it to SQL
                requests = requests.Where(r => r.ClosedAt == null && r.DueDate < DateTime.Today);
            }

            // Keep the filter values so the view can show them again
            ViewData["Search"] = search;
            ViewData["Status"] = status;
            ViewData["Type"] = type;
            ViewData["Priority"] = priority;
            ViewData["Overdue"] = overdue;
            await PopulateListsAsync(type, status, priority);

            var paging = new ServiceRequestPagingInfo {
                PageSize = PageSize,
                TotalItems = await requests.CountAsync()
            };
            // A page out of range shows the closest valid one
            paging.CurrentPage = Math.Clamp(page, 1, paging.TotalPages);
            ViewData["Paging"] = paging;

            var pageItems = await requests
                .OrderByDescending(r => r.CreatedAt)
                .ThenByDescending(r => r.ServiceRequestId)
                .Skip((paging.CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return View(pageItems);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Type)
                .Include(r => r.Status)
                .Include(r => r.Priority)
                .Include(r => r.CatalogItem)
                .Include(r => r.History
                    .OrderByDescending(h => h.ChangedAt)
                    .ThenByDescending(h => h.ServiceRequestHistoryId))
                    .ThenInclude(h => h.OldStatus)
                .Include(r => r.History)
                    .ThenInclude(h => h.NewStatus)
                .Include(r => r.Comments.OrderBy(c => c.CreatedAt))
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return NotFound();
            }

            // The file contents are only needed to download them, not to list them
            serviceRequest.Attachments = await _context.ServiceRequestAttachments
                .Where(a => a.ServiceRequestId == serviceRequest.ServiceRequestId)
                .OrderBy(a => a.UploadedAt)
                .Select(a => new ServiceRequestAttachment {
                    ServiceRequestAttachmentId = a.ServiceRequestAttachmentId,
                    ServiceRequestId = a.ServiceRequestId,
                    FileName = a.FileName,
                    ContentType = a.ContentType,
                    Size = a.Size,
                    Content = Array.Empty<byte>(),
                    UploadedAt = a.UploadedAt
                })
                .ToListAsync();

            // The statuses for the process form of the details page
            await PopulateListsAsync(selectedStatusId: serviceRequest.ServiceRequestStatusId);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create?serviceRequestCatalogItemId=3
        public async Task<IActionResult> Create(int? serviceRequestCatalogItemId) {
            await PopulateListsAsync();
            await PopulateCatalogItemsAsync(serviceRequestCatalogItemId);
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,ServiceRequestTypeId,ServiceRequestPriorityId,ServiceRequestCatalogItemId,RequesterName,RequesterEmail")] ServiceRequest serviceRequest) {
            var catalogItem = await FindCatalogItemAsync(serviceRequest.ServiceRequestCatalogItemId);
            if (serviceRequest.ServiceRequestCatalogItemId != null && catalogItem?.IsActive != true) {
                ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestCatalogItemId), "This service is not available.");
            }
            UseTypeOfService(serviceRequest, catalogItem);

            await ValidateTypeAsync(serviceRequest.ServiceRequestTypeId);
            var priority = await FindPriorityAsync(serviceRequest.ServiceRequestPriorityId);

            var firstStatus = await FindFirstStatusAsync();
            if (firstStatus == null) {
                ModelState.AddModelError(string.Empty, "There are no request statuses yet. Add them before creating requests.");
            }

            if (!ModelState.IsValid) {
                await PopulateListsAsync(serviceRequest.ServiceRequestTypeId, selectedPriorityId: serviceRequest.ServiceRequestPriorityId);
                await PopulateCatalogItemsAsync(serviceRequest.ServiceRequestCatalogItemId);
                return View(serviceRequest);
            }

            // Status and dates are always set by the server, never by the form
            serviceRequest.ServiceRequestStatusId = firstStatus!.ServiceRequestStatusId;
            serviceRequest.CreatedAt = DateTime.Now;
            serviceRequest.DueDate = CalculateDueDate(serviceRequest.CreatedAt, priority!, catalogItem);
            serviceRequest.History.Add(new ServiceRequestHistory {
                Action = "Request submitted",
                NewStatusId = firstStatus.ServiceRequestStatusId
            });

            _context.Add(serviceRequest);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Request #{serviceRequest.ServiceRequestId} was created. Keep this number to track it.";
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

            await PopulateListsAsync(serviceRequest.ServiceRequestTypeId, serviceRequest.ServiceRequestStatusId, serviceRequest.ServiceRequestPriorityId);
            await PopulateCatalogItemsAsync(serviceRequest.ServiceRequestCatalogItemId);
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ServiceRequestId,Title,Description,ServiceRequestTypeId,ServiceRequestStatusId,ServiceRequestPriorityId,ServiceRequestCatalogItemId,RequesterName,RequesterEmail,AssignedTo")] ServiceRequest posted) {
            if (id != posted.ServiceRequestId) {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(r => r.Priority)
                .FirstOrDefaultAsync(r => r.ServiceRequestId == id);
            if (serviceRequest == null) {
                return NotFound();
            }

            // A request can keep a service that is no longer available, but cannot change to one
            bool serviceChanged = serviceRequest.ServiceRequestCatalogItemId != posted.ServiceRequestCatalogItemId;
            var catalogItem = await FindCatalogItemAsync(posted.ServiceRequestCatalogItemId);
            if (posted.ServiceRequestCatalogItemId != null && (catalogItem == null || (serviceChanged && !catalogItem.IsActive))) {
                ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestCatalogItemId), "This service is not available.");
            }
            UseTypeOfService(posted, catalogItem);

            await ValidateTypeAsync(posted.ServiceRequestTypeId);
            var status = await _context.ServiceRequestStatuses.FindAsync(posted.ServiceRequestStatusId);
            if (status == null) {
                ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestStatusId), "Please choose a status from the list.");
            }
            var priority = await FindPriorityAsync(posted.ServiceRequestPriorityId);

            if (!ModelState.IsValid) {
                // Values that are not in the form, shown again as information
                posted.CreatedAt = serviceRequest.CreatedAt;
                posted.DueDate = serviceRequest.DueDate;
                await PopulateListsAsync(posted.ServiceRequestTypeId, posted.ServiceRequestStatusId, posted.ServiceRequestPriorityId);
                await PopulateCatalogItemsAsync(posted.ServiceRequestCatalogItemId);
                return View(posted);
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

            bool priorityChanged = serviceRequest.ServiceRequestPriorityId != priority!.ServiceRequestPriorityId;
            if (priorityChanged) {
                serviceRequest.History.Add(new ServiceRequestHistory {
                    Action = $"Priority changed from {serviceRequest.Priority?.Name} to {priority.Name}"
                });
                serviceRequest.Priority = priority;
            }
            if (serviceChanged) {
                serviceRequest.History.Add(new ServiceRequestHistory {
                    Action = catalogItem == null ? "Service removed" : $"Service changed to {catalogItem.Name}"
                });
                serviceRequest.ServiceRequestCatalogItemId = posted.ServiceRequestCatalogItemId;
            }

            // A new priority or service means a new deadline, counted from the day the request was created
            if (priorityChanged || serviceChanged) {
                serviceRequest.DueDate = CalculateDueDate(serviceRequest.CreatedAt, priority, catalogItem);
            }

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

        // Services of the catalog have their own processing time; other requests use the response time of the priority
        private static DateTime CalculateDueDate(DateTime createdAt, ServiceRequestPriority priority, ServiceRequestCatalogItem? catalogItem) {
            int days = catalogItem?.ProcessingDays ?? priority.ResponseDays;
            return createdAt.Date.AddDays(days);
        }

        // A request for a service of the catalog always has the type of that service, whatever the form sent
        private void UseTypeOfService(ServiceRequest request, ServiceRequestCatalogItem? catalogItem) {
            if (catalogItem != null) {
                request.ServiceRequestTypeId = catalogItem.ServiceRequestTypeId;
                ModelState.Remove(nameof(ServiceRequest.ServiceRequestTypeId));
            }
        }

        private async Task<ServiceRequestCatalogItem?> FindCatalogItemAsync(int? id) {
            return id == null ? null : await _context.ServiceRequestCatalogItems.FindAsync(id);
        }

        // Only available services can be chosen; the current one is kept even if it is no longer available
        private async Task PopulateCatalogItemsAsync(int? selectedId) {
            var items = await _context.ServiceRequestCatalogItems
                .Where(i => i.IsActive || i.ServiceRequestCatalogItemId == selectedId)
                .OrderBy(i => i.Name)
                .ToListAsync();

            ViewData["CatalogItems"] = new SelectList(items,
                nameof(ServiceRequestCatalogItem.ServiceRequestCatalogItemId),
                nameof(ServiceRequestCatalogItem.Name),
                selectedId);
        }

        private async Task<ServiceRequestPriority?> FindPriorityAsync(int priorityId) {
            var priority = await _context.ServiceRequestPriorities.FindAsync(priorityId);
            if (priority == null) {
                ModelState.AddModelError(nameof(ServiceRequest.ServiceRequestPriorityId), "Please choose a priority from the list.");
            }
            return priority;
        }

        // Types, statuses and priorities for the select lists of the forms and the filters
        private async Task PopulateListsAsync(int? selectedTypeId = null, int? selectedStatusId = null, int? selectedPriorityId = null) {
            var types = await _context.ServiceRequestTypes.OrderBy(t => t.Name).ToListAsync();
            ViewData["Types"] = new SelectList(types,
                nameof(ServiceRequestType.ServiceRequestTypeId), nameof(ServiceRequestType.Name), selectedTypeId);

            var statuses = await _context.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToListAsync();
            ViewData["Statuses"] = new SelectList(statuses,
                nameof(ServiceRequestStatus.ServiceRequestStatusId), nameof(ServiceRequestStatus.Name), selectedStatusId);

            // The most urgent priorities first
            var priorities = await _context.ServiceRequestPriorities.OrderByDescending(p => p.Level).ToListAsync();
            ViewData["Priorities"] = new SelectList(priorities,
                nameof(ServiceRequestPriority.ServiceRequestPriorityId), nameof(ServiceRequestPriority.Name), selectedPriorityId);
        }
    }
}
