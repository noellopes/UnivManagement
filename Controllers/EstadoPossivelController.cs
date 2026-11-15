
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class EstadoPossivelController : Controller
{
    private readonly UniversityDbContext _context;

    public EstadoPossivelController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ESTADOPOSSIVELS
    public async Task<IActionResult> Index()
    {
        return View(await _context.EstadoPossivel.ToListAsync());
    }

    // GET: ESTADOPOSSIVELS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var estadopossivel = await _context.EstadoPossivel
            .FirstOrDefaultAsync(m => m.EstadoPossivelId == id);
        if (estadopossivel == null)
        {
            return NotFound();
        }

        return View(estadopossivel);
    }

    // GET: ESTADOPOSSIVELS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ESTADOPOSSIVELS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EstadoPossivelId,NomeEstado,Descricao")] EstadoPossivel estadopossivel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(estadopossivel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(estadopossivel);
    }

    // GET: ESTADOPOSSIVELS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var estadopossivel = await _context.EstadoPossivel.FindAsync(id);
        if (estadopossivel == null)
        {
            return NotFound();
        }
        return View(estadopossivel);
    }

    // POST: ESTADOPOSSIVELS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("EstadoPossivelId,NomeEstado,Descricao")] EstadoPossivel estadopossivel)
    {
        if (id != estadopossivel.EstadoPossivelId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(estadopossivel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoPossivelExists(estadopossivel.EstadoPossivelId))
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
        return View(estadopossivel);
    }

    // GET: ESTADOPOSSIVELS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var estadopossivel = await _context.EstadoPossivel
            .FirstOrDefaultAsync(m => m.EstadoPossivelId == id);
        if (estadopossivel == null)
        {
            return NotFound();
        }

        return View(estadopossivel);
    }

    // POST: ESTADOPOSSIVELS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var estadopossivel = await _context.EstadoPossivel.FindAsync(id);
        if (estadopossivel != null)
        {
            _context.EstadoPossivel.Remove(estadopossivel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EstadoPossivelExists(int id)
    {
        return _context.EstadoPossivel.Any(e => e.EstadoPossivelId == id);
    }
}