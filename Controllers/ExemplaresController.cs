
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class ExemplaresController : Controller
{
    private readonly UniversityDbContext _context;

    public ExemplaresController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: EXEMPLARS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Exemplares.ToListAsync());
    }

    // GET: EXEMPLARS/Details/5
    public async Task<IActionResult> Details(int? exemplarid)
    {
        if (exemplarid == null)
        {
            return NotFound();
        }

        var exemplar = await _context.Exemplares
            .FirstOrDefaultAsync(m => m.ExemplarId == exemplarid);
        if (exemplar == null)
        {
            return NotFound();
        }

        return View(exemplar);
    }

    // GET: EXEMPLARS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: EXEMPLARS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ExemplarId,Codigo,Estado,IdCatalogoFK,Catalogo")] Exemplar exemplar)
    {
        if (ModelState.IsValid)
        {
            _context.Add(exemplar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(exemplar);
    }

    // GET: EXEMPLARS/Edit/5
    public async Task<IActionResult> Edit(int? exemplarid)
    {
        if (exemplarid == null)
        {
            return NotFound();
        }

        var exemplar = await _context.Exemplares.FindAsync(exemplarid);
        if (exemplar == null)
        {
            return NotFound();
        }
        return View(exemplar);
    }

    // POST: EXEMPLARS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? exemplarid, [Bind("ExemplarId,Codigo,Estado,IdCatalogoFK,Catalogo")] Exemplar exemplar)
    {
        if (exemplarid != exemplar.ExemplarId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exemplar);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExemplarExists(exemplar.ExemplarId))
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
        return View(exemplar);
    }

    // GET: EXEMPLARS/Delete/5
    public async Task<IActionResult> Delete(int? exemplarid)
    {
        if (exemplarid == null)
        {
            return NotFound();
        }

        var exemplar = await _context.Exemplares
            .FirstOrDefaultAsync(m => m.ExemplarId == exemplarid);
        if (exemplar == null)
        {
            return NotFound();
        }

        return View(exemplar);
    }

    // POST: EXEMPLARS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? exemplarid)
    {
        var exemplar = await _context.Exemplares.FindAsync(exemplarid);
        if (exemplar != null)
        {
            _context.Exemplares.Remove(exemplar);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ExemplarExists(int? exemplarid)
    {
        return _context.Exemplares.Any(e => e.ExemplarId == exemplarid);
    }
}
