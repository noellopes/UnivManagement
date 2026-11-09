
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
        return View(await _context.OrientadoresEstagio.ToListAsync());
    }

    // GET: OrientadoresEstagio/Details
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

    // GET: OrientadoresEstagio/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: OrientadoresEstagio/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nome,Email,Password")] OrientadorEstagio orientadorestagio)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orientadorestagio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(orientadorestagio);
    }

    // GET: OrientadoresEstagio/Edit
    public async Task<IActionResult> Edit(int? idorientador)
    {
        if (idorientador == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .FindAsync(idorientador);

        if (orientadorestagio == null)
        {
            return NotFound();
        }

        return View(orientadorestagio);
    }

    // POST: OrientadoresEstagio/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    int? idorientador, string Nome, string Email, string? Password)
    {
        if (idorientador == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .FindAsync(idorientador);

        if (orientadorestagio == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            orientadorestagio.Nome = Nome;
            orientadorestagio.Email = Email;

            if (!string.IsNullOrEmpty(Password))
            {
                orientadorestagio.Password = Password;
            }

            try
            {
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

    // GET: OrientadoresEstagio/Delete
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

    // POST: OrientadoresEstagio/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idorientador)
    {
        if (idorientador == null)
        {
            return NotFound();
        }

        var orientadorestagio = await _context.OrientadoresEstagio
            .FindAsync(idorientador);

        if (orientadorestagio != null)
        {
            _context.OrientadoresEstagio.Remove(orientadorestagio);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool OrientadorEstagioExists(int? idorientador)
    {
        return _context.OrientadoresEstagio
            .Any(e => e.IdOrientador == idorientador);
    }
}