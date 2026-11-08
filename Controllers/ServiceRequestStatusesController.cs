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
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToListAsync());
        }

        // GET: ServiceRequestStatuses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _context.ServiceRequestStatuses.FindAsync(id));
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
                ModelState.AddModelError("Name", "There is already a status with this name.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequestStatus);
        }

        // GET: ServiceRequestStatuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _context.ServiceRequestStatuses.FindAsync(id));
        }

        // POST: ServiceRequestStatuses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ServiceRequestStatusId,Name,Description,DisplayOrder,IsFinal,RequiresNote,Color")] ServiceRequestStatus serviceRequestStatus)
        {
            // Two statuses cannot have the same name (the one being edited does not count)
            if (_context.ServiceRequestStatuses.Any(s => s.Name == serviceRequestStatus.Name
                && s.ServiceRequestStatusId != serviceRequestStatus.ServiceRequestStatusId))
            {
                ModelState.AddModelError("Name", "There is already a status with this name.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(serviceRequestStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequestStatus);
        }

        // GET: ServiceRequestStatuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _context.ServiceRequestStatuses.FindAsync(id));
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
                    ModelState.AddModelError(string.Empty, "Some requests have this status, so it cannot be deleted.");
                    return View(serviceRequestStatus);
                }

                _context.ServiceRequestStatuses.Remove(serviceRequestStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
