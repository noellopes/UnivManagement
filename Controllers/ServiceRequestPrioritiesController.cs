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
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceRequestPriorities.ToListAsync());
        }

        // GET: ServiceRequestPriorities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _context.ServiceRequestPriorities.FindAsync(id));
        }

        // GET: ServiceRequestPriorities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestPriorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestPriorityId,Name,Level,ResponseDays,Color")] ServiceRequestPriority serviceRequestPriority)
        {
            // Two priorities cannot have the same name
            if (_context.ServiceRequestPriorities.Any(p => p.Name == serviceRequestPriority.Name))
            {
                ModelState.AddModelError("Name", "There is already a priority with this name.");
            }

            if (_context.ServiceRequestPriorities.Any(p => p.Level == serviceRequestPriority.Level))
            {
                ModelState.AddModelError("Level", "There is already a priority with this level.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestPriority);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequestPriority);
        }

        // GET: ServiceRequestPriorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _context.ServiceRequestPriorities.FindAsync(id));
        }

        // POST: ServiceRequestPriorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ServiceRequestPriorityId,Name,Level,ResponseDays,Color")] ServiceRequestPriority serviceRequestPriority)
        {
            // Two priorities cannot have the same name (the one being edited does not count)
            if (_context.ServiceRequestPriorities.Any(p => p.Name == serviceRequestPriority.Name
                && p.ServiceRequestPriorityId != serviceRequestPriority.ServiceRequestPriorityId))
            {
                ModelState.AddModelError("Name", "There is already a priority with this name.");
            }

            if(_context.ServiceRequestPriorities.Any(p => p.Level == serviceRequestPriority.Level
                && p.ServiceRequestPriorityId != serviceRequestPriority.ServiceRequestPriorityId))
            {
                ModelState.AddModelError("Level", "There is already a priority with this level.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(serviceRequestPriority);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequestPriority);
        }

        // GET: ServiceRequestPriorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _context.ServiceRequestPriorities.FindAsync(id));
        }

        // POST: ServiceRequestPriorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequestPriority = await _context.ServiceRequestPriorities.FindAsync(id);
            if (serviceRequestPriority != null)
            {
                _context.ServiceRequestPriorities.Remove(serviceRequestPriority);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
