
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class OrientadoresEstagioController : Controller
{
    private readonly UniversityDbContext _context;

    public OrientadoresEstagioController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ORIENTADORESTAGIOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.OrientadoresEstagio.ToListAsync());
    }

    // GET: ORIENTADORESTAGIOS/Details/5
    public async Task<IActionResult> Details(int? idorientador)
    {
        if (idorientador == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .FirstOrDefaultAsync(m => m.IdOrientador == idorientador);
        if (orientadorestagio == null)
        {
            return NotFound();
        }

        return View(orientadorestagio);
    }

    // GET: ORIENTADORESTAGIOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ORIENTADORESTAGIOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdOrientador,Nome,Email,Password,Estagios,Avaliacoes")] OrientadorEstagio orientadorestagio)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orientadorestagio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(orientadorestagio);
    }

    // GET: ORIENTADORESTAGIOS/Edit/5
    public async Task<IActionResult> Edit(int? idorientador)
    {
        if (idorientador == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio.FindAsync(idorientador);
        if (orientadorestagio == null)
        {
            return NotFound();
        }
        return View(orientadorestagio);
    }

    // POST: ORIENTADORESTAGIOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idorientador, [Bind("IdOrientador,Nome,Email,Password,Estagios,Avaliacoes")] OrientadorEstagio orientadorestagio)
    {
        if (idorientador != orientadorestagio.IdOrientador)
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
                if (!OrientadorEstagioExists(orientadorestagio.IdOrientador))
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

    // GET: ORIENTADORESTAGIOS/Delete/5
    public async Task<IActionResult> Delete(int? idorientador)
    {
        if (idorientador == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .FirstOrDefaultAsync(m => m.IdOrientador == idorientador);
        if (orientadorestagio == null)
        {
            return NotFound();
        }

        return View(orientadorestagio);
    }

    // POST: ORIENTADORESTAGIOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idorientador)
    {
        var orientadorestagio = await _context.OrientadoresEstagio.FindAsync(idorientador);
        if (orientadorestagio != null)
        {
            _context.OrientadoresEstagio.Remove(orientadorestagio);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrientadorEstagioExists(int? idorientador)
    {
        return _context.OrientadoresEstagio.Any(e => e.IdOrientador == idorientador);
    }
}
