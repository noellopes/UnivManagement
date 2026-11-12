using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class ServiceRequestPrioritiesController : Controller
    {
        private readonly UniversityDbContext _context;

        public ServiceRequestPrioritiesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestPriorities
        public async Task<IActionResult> Index(string? successMessage = null)
        {
            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            return View(await _context.ServiceRequestPriorities.ToListAsync());
        }

        // GET: ServiceRequestPriorities/Details/5
        public async Task<IActionResult> Details(int? id, string? successMessage = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestPriority = await _context.ServiceRequestPriorities
                .FirstOrDefaultAsync(m => m.ServiceRequestPriorityId == id);

            if (serviceRequestPriority == null)
            {
                return View("NotFound");
            }

            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            return View(serviceRequestPriority);
        }

        // GET: ServiceRequestPriorities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestPriorities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestPriorityId,Name,Level,ResponseDays,Color")] ServiceRequestPriority serviceRequestPriority)
        {
            // Two priorities cannot have the same name
            if (_context.ServiceRequestPriorities.Any(p => p.Name == serviceRequestPriority.Name))
            {
                ModelState.AddModelError("Name", "There is already a priority with this name.");
            }

            // Two priorities cannot have the same level
            if (_context.ServiceRequestPriorities.Any(p => p.Level == serviceRequestPriority.Level))
            {
                ModelState.AddModelError("Level", "There is already a priority with this level.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestPriority);
                await _context.SaveChangesAsync();
                return RedirectToAction(
                    nameof(Details),
                    new { id = serviceRequestPriority.ServiceRequestPriorityId, successMessage = "Priority created successfully." });
            }
            return View(serviceRequestPriority);
        }

        // GET: ServiceRequestPriorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestPriority = await _context.ServiceRequestPriorities.FindAsync(id);

            if (serviceRequestPriority == null)
            {
                return View("NotFound");
            }

            return View(serviceRequestPriority);
        }

        // POST: ServiceRequestPriorities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ServiceRequestPriorityId,Name,Level,ResponseDays,Color")] ServiceRequestPriority serviceRequestPriority)
        {
            if (id != serviceRequestPriority.ServiceRequestPriorityId)
            {
                return NotFound();
            }

            // Two priorities cannot have the same name (the one being edited does not count)
            if (_context.ServiceRequestPriorities.Any(p => p.Name == serviceRequestPriority.Name
                && p.ServiceRequestPriorityId != serviceRequestPriority.ServiceRequestPriorityId))
            {
                ModelState.AddModelError("Name", "There is already a priority with this name.");
            }

            // Two priorities cannot have the same level (the one being edited does not count)
            if (_context.ServiceRequestPriorities.Any(p => p.Level == serviceRequestPriority.Level
                && p.ServiceRequestPriorityId != serviceRequestPriority.ServiceRequestPriorityId))
            {
                ModelState.AddModelError("Level", "There is already a priority with this level.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequestPriority);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestPriorityExists(serviceRequestPriority.ServiceRequestPriorityId))
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
                    new { id = serviceRequestPriority.ServiceRequestPriorityId, successMessage = "Priority updated successfully." });
            }
            return View(serviceRequestPriority);
        }

        // GET: ServiceRequestPriorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestPriority = await _context.ServiceRequestPriorities
                .FirstOrDefaultAsync(m => m.ServiceRequestPriorityId == id);

            if (serviceRequestPriority == null)
            {
                return RedirectToAction(nameof(Index),
                    new { successMessage = "Success: The priority was already deleted by another user." });
            }

            return View(serviceRequestPriority);
        }

        // POST: ServiceRequestPriorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var serviceRequestPriority = await _context.ServiceRequestPriorities.FindAsync(id);
            if (serviceRequestPriority != null)
            {
                _context.ServiceRequestPriorities.Remove(serviceRequestPriority);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { successMessage = "Priority deleted successfully." });
        }

        private bool ServiceRequestPriorityExists(int id)
        {
            return _context.ServiceRequestPriorities.Any(e => e.ServiceRequestPriorityId == id);
        }
    }
}