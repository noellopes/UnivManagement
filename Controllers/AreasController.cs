
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class AreasController : Controller
{
    private readonly UniversityDbContext _context;

    public AreasController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: AREA
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Area.ToListAsync());
    }

    // GET: AREA/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var area = await _context.Area.FirstOrDefaultAsync(m => m.AreaId == id);
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
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var area = await _context.Area.FindAsync(id);
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
    public async Task<IActionResult> Edit(int? id, [Bind("AreaId,Nome,Descricao")] Area area)
    {
        if (id != area.AreaId)
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
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var area = await _context.Area
            .FirstOrDefaultAsync(m => m.AreaId == id);
        if (area == null)
        {
            return NotFound();
        }

        return View(area);
    }

    // POST: AREA/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var area = await _context.Area.FindAsync(id);
        if (area != null)
        {
            _context.Area.Remove(area);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AreaExists(int? id)
    {
        return _context.Area.Any(e => e.AreaId == id);
    }
}
