using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    public class ServiceRequestHistoriesController : Controller
    {
        private readonly UniversityDbContext _context;

        public ServiceRequestHistoriesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestHistories
        public async Task<IActionResult> Index()
        {
            var histories = _context.ServiceRequestHistories
                .Include(h => h.ServiceRequest)
                .Include(h => h.OldStatus)
                .Include(h => h.NewStatus);

            return View(await histories.ToListAsync());
        }

        // GET: ServiceRequestHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestHistory = await _context.ServiceRequestHistories
                .Include(h => h.ServiceRequest)
                .Include(h => h.OldStatus)
                .Include(h => h.NewStatus)
                .FirstOrDefaultAsync(m => m.ServiceRequestHistoryId == id);

            if (serviceRequestHistory == null)
            {
                return NotFound();
            }

            return View(serviceRequestHistory);
        }

        // GET: ServiceRequestHistories/Create
        public async Task<IActionResult> Create()
        {
            await LoadListsAsync();
            return View();
        }

        // POST: ServiceRequestHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestId,OldStatusId,NewStatusId,Action,Note")] ServiceRequestHistory serviceRequestHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadListsAsync();
            return View(serviceRequestHistory);
        }

        // GET: ServiceRequestHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestHistory = await _context.ServiceRequestHistories.FindAsync(id);
            if (serviceRequestHistory == null)
            {
                return NotFound();
            }
            await LoadListsAsync();
            return View(serviceRequestHistory);
        }

        // POST: ServiceRequestHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ServiceRequestHistoryId,ServiceRequestId,OldStatusId,NewStatusId,Action,Note")] ServiceRequestHistory serviceRequestHistory)
        {
            if (id != serviceRequestHistory.ServiceRequestHistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequestHistory);
                    _context.Entry(serviceRequestHistory).Property(h => h.ChangedAt).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestHistoryExists(serviceRequestHistory.ServiceRequestHistoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await LoadListsAsync();
            return View(serviceRequestHistory);
        }

        // GET: ServiceRequestHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestHistory = await _context.ServiceRequestHistories
                .Include(h => h.ServiceRequest)
                .Include(h => h.OldStatus)
                .Include(h => h.NewStatus)
                .FirstOrDefaultAsync(m => m.ServiceRequestHistoryId == id);
                
            if (serviceRequestHistory == null)
            {
                return NotFound();
            }

            return View(serviceRequestHistory);
        }

        // POST: ServiceRequestHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequestHistory = await _context.ServiceRequestHistories.FindAsync(id);
            if (serviceRequestHistory != null)
            {
                _context.ServiceRequestHistories.Remove(serviceRequestHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ServiceRequestHistoryExists(int? id)
        {
            return _context.ServiceRequestHistories.Any(e => e.ServiceRequestHistoryId == id);
        }

        private async Task LoadListsAsync()
        {
            ViewData["ServiceRequestId"] = new SelectList(
                await _context.ServiceRequests.OrderBy(r => r.Title).ToListAsync(),
                "ServiceRequestId", "Title");

            var statuses = await _context.ServiceRequestStatuses.OrderBy(s => s.DisplayOrder).ToListAsync();
            ViewData["OldStatusId"] = new SelectList(statuses, "ServiceRequestStatusId", "Name");
            ViewData["NewStatusId"] = new SelectList(statuses, "ServiceRequestStatusId", "Name");
        }
    }
}