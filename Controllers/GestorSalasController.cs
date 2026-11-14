
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class GestorSalasController : Controller
{
    private readonly UniversityDbContext _context;

    public GestorSalasController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: GESTORSALASS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.GestorSalas.ToListAsync());
    }

    // GET: GESTORSALASS/Details/5
    [HttpGet("GestorSalas/Details/{utilizadorid}")]
    public async Task<IActionResult> Details(int? utilizadorid)
    {
        if (utilizadorid == null)
        {
            return NotFound();
        }

        var gestorsalas = await _context.GestorSalas
            .FirstOrDefaultAsync(m => m.UtilizadorId == utilizadorid);
        if (gestorsalas == null)
        {
            return NotFound();
        }

        return View(gestorsalas);
    }

    // GET: GESTORSALASS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: GESTORSALASS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UtilizadorId,Gabinete")] GestorSalas gestorsalas)
    {
        if (ModelState.IsValid)
        {
            _context.Add(gestorsalas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(gestorsalas);
    }

    // GET: GESTORSALASS/Edit/5
    [HttpGet("GestorSalas/Edit/{utilizadorid}")]
    public async Task<IActionResult> Edit(int? utilizadorid)
    {
        if (utilizadorid == null)
        {
            return NotFound();
        }

        var gestorsalas = await _context.GestorSalas.FindAsync(utilizadorid);
        if (gestorsalas == null)
        {
            return NotFound();
        }
        return View(gestorsalas);
    }

    // POST: GESTORSALASS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? utilizadorid, [Bind("UtilizadorId,Gabinete")] GestorSalas gestorsalas)
    {
        if (utilizadorid != gestorsalas.UtilizadorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(gestorsalas);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GestorSalasExists(gestorsalas.UtilizadorId))
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
        return View(gestorsalas);
    }

    // GET: GESTORSALASS/Delete/5
    [HttpGet("GestorSalas/Delete/{utilizadorid}")]
    public async Task<IActionResult> Delete(int? utilizadorid)
    {
        if (utilizadorid == null)
        {
            return NotFound();
        }

        var gestorsalas = await _context.GestorSalas
            .FirstOrDefaultAsync(m => m.UtilizadorId == utilizadorid);
        if (gestorsalas == null)
        {
            return NotFound();
        }

        return View(gestorsalas);
    }

    // POST: GESTORSALASS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? utilizadorid)
    {
        var gestorsalas = await _context.GestorSalas.FindAsync(utilizadorid);
        if (gestorsalas != null)
        {
            _context.GestorSalas.Remove(gestorsalas);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GestorSalasExists(int? utilizadorid)
    {
        return _context.GestorSalas.Any(e => e.UtilizadorId == utilizadorid);
    }
}
