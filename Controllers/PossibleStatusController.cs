
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

    // GET: POSSIBLESTATUSS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.PossibleStatus.ToListAsync());
    }

    // GET: POSSIBLESTATUSS/Details/5
    public async Task<IActionResult> Details(int? possiblestatusid)
    {
        if (possiblestatusid == null)
        {
            return NotFound();
        }

        var possiblestatus = await _context.PossibleStatus
            .FirstOrDefaultAsync(m => m.PossibleStatusId == possiblestatusid);
        if (possiblestatus == null)
        {
            return NotFound();
        }

        return View(possiblestatus);
    }

    // GET: POSSIBLESTATUSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: POSSIBLESTATUSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PossibleStatusId,Name")] PossibleStatus possiblestatus)
    {
        if (ModelState.IsValid)
        {
            _context.Add(possiblestatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(possiblestatus);
    }

    // GET: POSSIBLESTATUSS/Edit/5
    public async Task<IActionResult> Edit(int? possiblestatusid)
    {
        if (possiblestatusid == null)
        {
            return NotFound();
        }

        var possiblestatus = await _context.PossibleStatus.FindAsync(possiblestatusid);
        if (possiblestatus == null)
        {
            return NotFound();
        }
        return View(possiblestatus);
    }

    // POST: POSSIBLESTATUSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? possiblestatusid, [Bind("PossibleStatusId,Name")] PossibleStatus possiblestatus)
    {
        if (possiblestatusid != possiblestatus.PossibleStatusId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(possiblestatus);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PossibleStatusExists(possiblestatus.PossibleStatusId))
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
        return View(possiblestatus);
    }

    // GET: POSSIBLESTATUSS/Delete/5
    public async Task<IActionResult> Delete(int? possiblestatusid)
    {
        if (possiblestatusid == null)
        {
            return NotFound();
        }

        var possiblestatus = await _context.PossibleStatus
            .FirstOrDefaultAsync(m => m.PossibleStatusId == possiblestatusid);
        if (possiblestatus == null)
        {
            return NotFound();
        }

        return View(possiblestatus);
    }

    // POST: POSSIBLESTATUSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? possiblestatusid)
    {
        var possiblestatus = await _context.PossibleStatus.FindAsync(possiblestatusid);
        if (possiblestatus != null)
        {
            _context.PossibleStatus.Remove(possiblestatus);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PossibleStatusExists(int? possiblestatusid)
    {
        return _context.PossibleStatus.Any(e => e.PossibleStatusId == possiblestatusid);
    }
}
