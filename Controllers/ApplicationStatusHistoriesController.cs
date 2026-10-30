using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

namespace UnivManagement.Controllers; // Confirma se tens o namespace aqui!

public class ApplicationStatusHistoriesController : Controller
{
    private readonly UniversityDbContext _context;

    public ApplicationStatusHistoriesController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ApplicationStatusHistories
    public async Task<IActionResult> Index()
    {
        return View(await _context.ApplicationStatusHistory.ToListAsync());
    }

    // GET: ApplicationStatusHistories/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var applicationstatushistory = await _context.ApplicationStatusHistory
            .FirstOrDefaultAsync(m => m.ApplicationStatusHistoryId == id);
        if (applicationstatushistory == null)
        {
            return NotFound();
        }

        return View(applicationstatushistory);
    }

    // GET: ApplicationStatusHistories/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ApplicationStatusHistories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ApplicationStatusHistoryId,AccommodationApplicationId,ChangedAt,ChangedBy")] ApplicationStatusHistory applicationstatushistory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(applicationstatushistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(applicationstatushistory);
    }

    // GET: ApplicationStatusHistories/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var applicationstatushistory = await _context.ApplicationStatusHistory.FindAsync(id);
        if (applicationstatushistory == null)
        {
            return NotFound();
        }
        return View(applicationstatushistory);
    }

    // POST: ApplicationStatusHistories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ApplicationStatusHistoryId,AccommodationApplicationId,ChangedAt,ChangedBy")] ApplicationStatusHistory applicationstatushistory)
    {
        if (id != applicationstatushistory.ApplicationStatusHistoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(applicationstatushistory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationStatusHistoryExists(applicationstatushistory.ApplicationStatusHistoryId))
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
        return View(applicationstatushistory);
    }

    // GET: ApplicationStatusHistories/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var applicationstatushistory = await _context.ApplicationStatusHistory
            .FirstOrDefaultAsync(m => m.ApplicationStatusHistoryId == id);
        if (applicationstatushistory == null)
        {
            return NotFound();
        }

        return View(applicationstatushistory);
    }

    // POST: ApplicationStatusHistories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var applicationstatushistory = await _context.ApplicationStatusHistory.FindAsync(id);
        if (applicationstatushistory != null)
        {
            _context.ApplicationStatusHistory.Remove(applicationstatushistory);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ApplicationStatusHistoryExists(int id)
    {
        return _context.ApplicationStatusHistory.Any(e => e.ApplicationStatusHistoryId == id);
    }
}
