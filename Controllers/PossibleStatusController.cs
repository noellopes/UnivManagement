using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

public class PossibleStatusController : Controller
{
    private readonly UniversityDbContext _context;

    public PossibleStatusController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: PossibleStatus
    public async Task<IActionResult> Index()
    {
        return View(await _context.PossibleStatus.ToListAsync());
    }

    // GET: PossibleStatus/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var possibleStatus = await _context.PossibleStatus
            .FirstOrDefaultAsync(m => m.PossibleStatusId == id);
        if (possibleStatus == null)
        {
            return NotFound();
        }

        return View(possibleStatus);
    }

    // GET: PossibleStatus/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PossibleStatus/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PossibleStatusId,Name")] PossibleStatus possibleStatus)
    {
        if (ModelState.IsValid)
        {
            _context.Add(possibleStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(possibleStatus);
    }

    // GET: PossibleStatus/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var possibleStatus = await _context.PossibleStatus.FindAsync(id);
        if (possibleStatus == null)
        {
            return NotFound();
        }
        return View(possibleStatus);
    }

    // POST: PossibleStatus/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PossibleStatusId,Name")] PossibleStatus possibleStatus)
    {
        if (id != possibleStatus.PossibleStatusId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(possibleStatus);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PossibleStatusExists(possibleStatus.PossibleStatusId))
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
        return View(possibleStatus);
    }

    // GET: PossibleStatus/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var possibleStatus = await _context.PossibleStatus
            .FirstOrDefaultAsync(m => m.PossibleStatusId == id);
        if (possibleStatus == null)
        {
            return NotFound();
        }

        return View(possibleStatus);
    }

    // POST: PossibleStatus/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var possibleStatus = await _context.PossibleStatus.FindAsync(id);
        if (possibleStatus != null)
        {
            _context.PossibleStatus.Remove(possibleStatus);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PossibleStatusExists(int id)
    {
        return _context.PossibleStatus.Any(e => e.PossibleStatusId == id);
    }
}