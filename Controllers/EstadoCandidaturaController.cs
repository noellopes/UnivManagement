
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class EstadoCandidaturaController : Controller
{
    private readonly UniversityDbContext _context;

    public EstadoCandidaturaController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ESTADOCANDIDATURAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.EstadoCandidatura.ToListAsync());
    }

    // GET: ESTADOCANDIDATURAS/Details/5
    public async Task<IActionResult> Details(int? estadocandidaturaid)
    {
        if (estadocandidaturaid == null)
        {
            return NotFound();
        }

        var estadocandidatura = await _context.EstadoCandidatura
            .FirstOrDefaultAsync(m => m.EstadoCandidaturaId == estadocandidaturaid);
        if (estadocandidatura == null)
        {
            return NotFound();
        }

        return View(estadocandidatura);
    }

    // GET: ESTADOCANDIDATURAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ESTADOCANDIDATURAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EstadoCandidaturaId,nome_estado,descricao")] EstadoCandidatura estadocandidatura)
    {
        if (ModelState.IsValid)
        {
            _context.Add(estadocandidatura);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(estadocandidatura);
    }

    // GET: ESTADOCANDIDATURAS/Edit/5
    public async Task<IActionResult> Edit(int? estadocandidaturaid)
    {
        if (estadocandidaturaid == null)
        {
            return NotFound();
        }

        var estadocandidatura = await _context.EstadoCandidatura.FindAsync(estadocandidaturaid);
        if (estadocandidatura == null)
        {
            return NotFound();
        }
        return View(estadocandidatura);
    }

    // POST: ESTADOCANDIDATURAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? estadocandidaturaid, [Bind("EstadoCandidaturaId,nome_estado,descricao")] EstadoCandidatura estadocandidatura)
    {
        if (estadocandidaturaid != estadocandidatura.EstadoCandidaturaId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(estadocandidatura);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoCandidaturaExists(estadocandidatura.EstadoCandidaturaId))
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
        return View(estadocandidatura);
    }

    // GET: ESTADOCANDIDATURAS/Delete/5
    public async Task<IActionResult> Delete(int? estadocandidaturaid)
    {
        if (estadocandidaturaid == null)
        {
            return NotFound();
        }

        var estadocandidatura = await _context.EstadoCandidatura
            .FirstOrDefaultAsync(m => m.EstadoCandidaturaId == estadocandidaturaid);
        if (estadocandidatura == null)
        {
            return NotFound();
        }

        return View(estadocandidatura);
    }

    // POST: ESTADOCANDIDATURAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? estadocandidaturaid)
    {
        var estadocandidatura = await _context.EstadoCandidatura.FindAsync(estadocandidaturaid);
        if (estadocandidatura != null)
        {
            _context.EstadoCandidatura.Remove(estadocandidatura);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EstadoCandidaturaExists(int? estadocandidaturaid)
    {
        return _context.EstadoCandidatura.Any(e => e.EstadoCandidaturaId == estadocandidaturaid);
    }
}
