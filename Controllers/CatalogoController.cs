
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class CatalogoController : Controller
{
    private readonly UniversityDbContext _context;

    public CatalogoController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: CATALOGOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Catalogos.ToListAsync());
    }

    // GET: CATALOGOS/Details/5
    public async Task<IActionResult> Details(int? idcatalogo)
    {
        if (idcatalogo == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos
            .FirstOrDefaultAsync(m => m.IdCatalogo == idcatalogo);
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
    public async Task<IActionResult> Create([Bind("IdCatalogo,ISBN,Titulo,Autor")] Catalogo catalogo)
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
    public async Task<IActionResult> Edit(int? idcatalogo)
    {
        if (idcatalogo == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos.FindAsync(idcatalogo);
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
    public async Task<IActionResult> Edit(int? idcatalogo, [Bind("IdCatalogo,ISBN,Titulo,Autor")] Catalogo catalogo)
    {
        if (idcatalogo != catalogo.IdCatalogo)
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
                if (!CatalogoExists(catalogo.IdCatalogo))
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
    public async Task<IActionResult> Delete(int? idcatalogo)
    {
        if (idcatalogo == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos
            .FirstOrDefaultAsync(m => m.IdCatalogo == idcatalogo);
        if (catalogo == null)
        {
            return NotFound();
        }

        return View(catalogo);
    }

    // POST: CATALOGOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idcatalogo)
    {
        var catalogo = await _context.Catalogos.FindAsync(idcatalogo);
        if (catalogo != null)
        {
            _context.Catalogos.Remove(catalogo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CatalogoExists(int? idcatalogo)
    {
        return _context.Catalogos.Any(e => e.IdCatalogo == idcatalogo);
    }
}
