
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class AreaController : Controller
{
    private readonly UniversityDbContext _context;

    public AreaController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: AREA
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Area.ToListAsync());
    }

    // GET: AREA/Details/5
    public async Task<IActionResult> Details(int? areaid)
    {
        if (areaid == null)
        {
            return NotFound();
        }

        var area = await _context.Area
            .FirstOrDefaultAsync(m => m.AreaId == areaid);
        if (area == null)
        {
            return NotFound();
        }

        return View(area);
    }

    // GET: AREA/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AREA/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AreaId,Nome,Descricao")] Area area)
    {
        if (ModelState.IsValid)
        {
            _context.Add(area);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(area);
    }

    // GET: AREA/Edit/5
    public async Task<IActionResult> Edit(int? areaid)
    {
        if (areaid == null)
        {
            return NotFound();
        }

        var area = await _context.Area.FindAsync(areaid);
        if (area == null)
        {
            return NotFound();
        }
        return View(area);
    }

    // POST: AREA/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? areaid, [Bind("AreaId,Nome,Descricao")] Area area)
    {
        if (areaid != area.AreaId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(area);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaExists(area.AreaId))
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
        return View(area);
    }

    // GET: AREA/Delete/5
    public async Task<IActionResult> Delete(int? areaid)
    {
        if (areaid == null)
        {
            return NotFound();
        }

        var area = await _context.Area
            .FirstOrDefaultAsync(m => m.AreaId == areaid);
        if (area == null)
        {
            return NotFound();
        }

        return View(area);
    }

    // POST: AREA/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? areaid)
    {
        var area = await _context.Area.FindAsync(areaid);
        if (area != null)
        {
            _context.Area.Remove(area);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AreaExists(int? areaid)
    {
        return _context.Area.Any(e => e.AreaId == areaid);
    }
}
