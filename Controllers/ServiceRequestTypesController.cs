using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class ServiceRequestTypesController : Controller
    {
        private readonly UniversityDbContext _context;

        public ServiceRequestTypesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestTypes
        public async Task<IActionResult> Index(string? msg = null)
        {
            if (msg != null)
            {
                ViewBag.SuccessMessage = msg;
            }
            return View(await _context.ServiceRequestTypes.ToListAsync());
        }

        // GET: ServiceRequestTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] ServiceRequestType serviceRequestType)
        {
            if (await _context.ServiceRequestTypes.AnyAsync(t => t.Name == serviceRequestType.Name))
            {
                ModelState.AddModelError(nameof(ServiceRequestType.Name), "There is already a request type with this name.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestType);
                await _context.SaveChangesAsync();
                return RedirectToAction(
                    nameof(Details),
                    new { id = serviceRequestType.ServiceRequestTypeId, msg = "Request type created successfully." }
                );
            }
            return View(serviceRequestType);
        }

        // GET: ServiceRequestTypes/Details/5
        public async Task<IActionResult> Details(int? id, string? msg = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestType = await _context.ServiceRequestTypes.FirstOrDefaultAsync(t => t.ServiceRequestTypeId == id);

            if (serviceRequestType == null)
            {
                return View("NotFound");
            }

            if (msg != null)
            {
                ViewBag.SuccessMessage = msg;
            }

            return View(serviceRequestType);
        }

        // GET: ServiceRequestTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestType = await _context.ServiceRequestTypes.FindAsync(id);
            if (serviceRequestType == null)
            {
                return View("NotFound");
            }
            return View(serviceRequestType);
        }

        // POST: ServiceRequestTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceRequestTypeId,Name,Description")] ServiceRequestType serviceRequestType)
        {
            if (id != serviceRequestType.ServiceRequestTypeId)
            {
                return NotFound();
            }

            if (await _context.ServiceRequestTypes.AnyAsync(t => t.Name == serviceRequestType.Name && t.ServiceRequestTypeId != serviceRequestType.ServiceRequestTypeId))
            {
                ModelState.AddModelError(nameof(ServiceRequestType.Name), "There is already a request type with this name.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequestType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestTypeExists(serviceRequestType.ServiceRequestTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(
                    nameof(Details),
                    new { id = serviceRequestType.ServiceRequestTypeId, msg = "Request type edited successfully." }
                );
            }
            return View(serviceRequestType);
        }

        // GET: ServiceRequestTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestType = await _context.ServiceRequestTypes.FindAsync(id);
            if (serviceRequestType == null)
            {
                return RedirectToAction(
                    nameof(Index),
                    new { msg = "The request type was already deleted." }
                );
            }
            return View(serviceRequestType);
        }

        // POST: ServiceRequestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequestType = await _context.ServiceRequestTypes.FindAsync(id);
            if (serviceRequestType != null)
            {
                if (await _context.ServiceRequests.AnyAsync(r => r.ServiceRequestTypeId == id))
                {
                    ModelState.AddModelError(string.Empty, "This type is used by some requests, so it cannot be deleted.");
                    return View("Delete", serviceRequestType);
                }
                _context.ServiceRequestTypes.Remove(serviceRequestType);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(
                nameof(Index),
                new { msg = "Request type deleted successfully." }
            );
        }

        private bool ServiceRequestTypeExists(int id)
        {
            return _context.ServiceRequestTypes.Any(e => e.ServiceRequestTypeId == id);
        }
    }
}