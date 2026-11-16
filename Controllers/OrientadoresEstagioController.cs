using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class OrientadoresEstagioController : Controller
{
    private readonly UniversityDbContext _context;

    public OrientadoresEstagioController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: OrientadoresEstagio
    public async Task<IActionResult> Index()
    {
        return View(await _context.OrientadoresEstagio
            .Include(o => o.Estagios)
            .ToListAsync());
    }

    // GET: OrientadoresEstagio/Details/5
    public async Task<IActionResult> Details(int? orientadorId)
    {
        if (orientadorId == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .Include(o => o.Estagios)
            .FirstOrDefaultAsync(m => m.OrientadorId == orientadorId);

        if (orientadorestagio == null)
        {
            return NotFound();
        }

        return View(orientadorestagio);
    }

    // GET: OrientadoresEstagio/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: OrientadoresEstagio/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nome,Email")] OrientadorEstagio orientadorestagio)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orientadorestagio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(orientadorestagio);
    }

    // GET: OrientadoresEstagio/Edit/5
    public async Task<IActionResult> Edit(int? orientadorId)
    {
        if (orientadorId == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .FindAsync(orientadorId);

        if (orientadorestagio == null)
        {
            return NotFound();
        }

        return View(orientadorestagio);
    }

    // POST: OrientadoresEstagio/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? orientadorId,
        [Bind("OrientadorId,Nome,Email")] OrientadorEstagio orientadorestagio)
    {
        if (orientadorId == null ||
            orientadorId <= 0 ||
            orientadorId != orientadorestagio.OrientadorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(orientadorestagio);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrientadorEstagioExists(orientadorestagio.OrientadorId))
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

        return View(orientadorestagio);
    }

    // GET: OrientadoresEstagio/Delete/5
    public async Task<IActionResult> Delete(int? orientadorId)
    {
        if (orientadorId == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .Include(o => o.Estagios)
            .FirstOrDefaultAsync(m => m.OrientadorId == orientadorId);

        if (orientadorestagio == null)
        {
            return NotFound();
        }

        return View(orientadorestagio);
    }

    // POST: OrientadoresEstagio/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? orientadorId)
    {
        if (orientadorId == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .FindAsync(orientadorId);

        if (orientadorestagio != null)
        {
            if (await _context.Estagios.AnyAsync(
                e => e.OrientadorId == orientadorId))
            {
                return RedirectToAction(nameof(Delete),
                    new { orientadorId });
            }

            _context.OrientadoresEstagio.Remove(orientadorestagio);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool OrientadorEstagioExists(int? orientadorId)
    {
        return _context.OrientadoresEstagio
            .Any(e => e.OrientadorId == orientadorId);
    }
}