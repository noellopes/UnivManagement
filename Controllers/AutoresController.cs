
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class AutoresController : Controller
{
    private readonly UniversityDbContext _context;

    public AutoresController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: AUTORS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Autores.ToListAsync());
    }

    // GET: AUTORS/Details/5
    public async Task<IActionResult> Details(int? autorid)
    {
        if (autorid == null)
        {
            return NotFound();
        }

        var autor = await _context.Autores
            .FirstOrDefaultAsync(m => m.AutorId == autorid);
        if (autor == null)
        {
            return NotFound();
        }

        return View(autor);
    }

    // GET: AUTORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AUTORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome")] Autor autor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(autor);
    }

    // GET: AUTORS/Edit/5
    public async Task<IActionResult> Edit(int? autorid)
    {
        if (autorid == null)
        {
            return NotFound();
        }

        var autor = await _context.Autores.FindAsync(autorid);
        if (autor == null)
        {
            return NotFound();
        }
        return View(autor);
    }

    // POST: AUTORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? autorid, [Bind("AutorId,Nome")] Autor autor)
    {
        if (autorid != autor.AutorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var autorExistente = await _context.Autores
                .FindAsync(autor.AutorId);

                if (autorExistente == null)
                {
                    return NotFound();
                }

                autorExistente.Nome = autor.Nome;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(autor.AutorId))
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
        return View(autor);
    }

    // GET: AUTORS/Delete/5
    public async Task<IActionResult> Delete(int? autorid)
    {
        if (autorid == null)
        {
            return NotFound();
        }

        var autor = await _context.Autores
            .FirstOrDefaultAsync(m => m.AutorId == autorid);
        if (autor == null)
        {
            return NotFound();
        }

        return View(autor);
    }

    // POST: AUTORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? autorid)
    {
        var autor = await _context.Autores.FindAsync(autorid);
        if (autor != null)
        {
            var temObras = await _context.ObrasAutores
        .AnyAsync(oa => oa.AutorId == autor.AutorId);

            if (temObras)
            {
                TempData["Erro"] =
                    "Não é possível eliminar este autor porque está associado a uma ou mais obras.";

                return RedirectToAction(nameof(Index));
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AutorExists(int? autorid)
    {
        return _context.Autores.Any(e => e.AutorId == autorid);
    }
}
