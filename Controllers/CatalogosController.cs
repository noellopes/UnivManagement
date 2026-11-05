
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class CatalogosController : Controller
{
    private readonly UniversityDbContext _context;

    public CatalogosController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: CATALOGOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Catalogos.ToListAsync());
    }

    // GET: CATALOGOS/Details/5
    public async Task<IActionResult> Details(int? catalogoid)
    {
        if (catalogoid == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos
            .FirstOrDefaultAsync(m => m.CatalogoId == catalogoid);
        if (catalogo == null)
        {
            return NotFound();
        }

        return View(catalogo);
    }

    // GET: CATALOGOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CATALOGOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CatalogoId,ISBN,Titulo,AutorId,Autor,Exemplares")] Catalogo catalogo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(catalogo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(catalogo);
    }

    // GET: CATALOGOS/Edit/5
    public async Task<IActionResult> Edit(int? catalogoid)
    {
        if (catalogoid == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos.FindAsync(catalogoid);
        if (catalogo == null)
        {
            return NotFound();
        }
        return View(catalogo);
    }

    // POST: CATALOGOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? catalogoid, [Bind("CatalogoId,ISBN,Titulo,AutorId,Autor,Exemplares")] Catalogo catalogo)
    {
        if (catalogoid != catalogo.CatalogoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(catalogo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalogoExists(catalogo.CatalogoId))
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
        return View(catalogo);
    }

    // GET: CATALOGOS/Delete/5
    public async Task<IActionResult> Delete(int? catalogoid)
    {
        if (catalogoid == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos
            .FirstOrDefaultAsync(m => m.CatalogoId == catalogoid);
        if (catalogo == null)
        {
            return NotFound();
        }

        return View(catalogo);
    }

    // POST: CATALOGOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? catalogoid)
    {
        var catalogo = await _context.Catalogos.FindAsync(catalogoid);
        if (catalogo != null)
        {
            _context.Catalogos.Remove(catalogo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CatalogoExists(int? catalogoid)
    {
        return _context.Catalogos.Any(e => e.CatalogoId == catalogoid);
    }
}
