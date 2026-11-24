using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    // Statuses a request goes through (submitted, in progress, resolved...) managed by the administrative services
    public class ServiceRequestStatusesController : Controller
    {
        private readonly UniversityDbContext _context;

        public ServiceRequestStatusesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestStatuses
        public async Task<IActionResult> Index(string? successMessage = null)
        {
            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            return View(await _context.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToListAsync());
        }

        // GET: ServiceRequestStatuses/Details/5
        public async Task<IActionResult> Details(int? id, string? successMessage = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestStatus = await _context.ServiceRequestStatuses.FindAsync(id);
            if (serviceRequestStatus == null)
            {
                return View("NotFound");
            }

            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            return View(serviceRequestStatus);
        }

        // GET: ServiceRequestStatuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestStatuses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestStatusId,Name,Description,DisplayOrder,IsFinal,RequiresNote,Color")] ServiceRequestStatus serviceRequestStatus)
        {
            // Two statuses cannot have the same name
            if (_context.ServiceRequestStatuses.Any(s => s.Name == serviceRequestStatus.Name))
            {
                ViewBag.ErrorMessage = "There is already a status with this name.";
            }
            else if (ModelState.IsValid)
            {
                _context.Add(serviceRequestStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details),
                    new { id = serviceRequestStatus.ServiceRequestStatusId, successMessage = "Status created successfully." });
            }
            return View(serviceRequestStatus);
        }

        // GET: ServiceRequestStatuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestStatus = await _context.ServiceRequestStatuses.FindAsync(id);
            if (serviceRequestStatus == null)
            {
                return View("NotFound");
            }

            return View(serviceRequestStatus);
        }

        // POST: ServiceRequestStatuses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ServiceRequestStatusId,Name,Description,DisplayOrder,IsFinal,RequiresNote,Color")] ServiceRequestStatus serviceRequestStatus)
        {
            if (id != serviceRequestStatus.ServiceRequestStatusId)
            {
                return NotFound();
            }

            // Two statuses cannot have the same name (the one being edited does not count)
            if (_context.ServiceRequestStatuses.Any(s => s.Name == serviceRequestStatus.Name
                && s.ServiceRequestStatusId != serviceRequestStatus.ServiceRequestStatusId))
            {
                ViewBag.ErrorMessage = "There is already a status with this name.";
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequestStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestStatusExists(serviceRequestStatus.ServiceRequestStatusId))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details),
                    new { id = serviceRequestStatus.ServiceRequestStatusId, successMessage = "Status updated successfully." });
            }
            return View(serviceRequestStatus);
        }

        // GET: ServiceRequestStatuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestStatus = await _context.ServiceRequestStatuses.FindAsync(id);
            if (serviceRequestStatus == null)
            {
                return RedirectToAction(nameof(Index),
                    new { successMessage = "The status was already deleted by another user." });
            }

            return View(serviceRequestStatus);
        }

        // POST: ServiceRequestStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequestStatus = await _context.ServiceRequestStatuses.FindAsync(id);
            if (serviceRequestStatus != null)
            {
                // A status that some requests have cannot be deleted
                if (_context.ServiceRequests.Any(r => r.ServiceRequestStatusId == id))
                {
                    ViewBag.ErrorMessage = "Some requests have this status, so it cannot be deleted.";
                    return View(serviceRequestStatus);
                }

                _context.ServiceRequestStatuses.Remove(serviceRequestStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { successMessage = "Status deleted successfully." });
        }

        private bool ServiceRequestStatusExists(int id)
        {
            return _context.ServiceRequestStatuses.Any(e => e.ServiceRequestStatusId == id);
        }
    }
}
