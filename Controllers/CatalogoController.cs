
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class CatalogoController : Controller
{
    private readonly UniversityDbContext _context;

    public CatalogoController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Catalogo
    public async Task<IActionResult> Index()
    {
        var catalogos = await _context.Catalogos
            .Include(c => c.Autor)
            .ToListAsync();

        return View(catalogos);
    }

    // GET: Catalogo/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos
            .Include(c => c.Autor)
            .FirstOrDefaultAsync(c => c.CatalogoId == id);

        if (catalogo == null)
        {
            return NotFound();
        }

        return View(catalogo);
    }

    // GET: Catalogo/Create
    public async Task<IActionResult> Create()
    {
        await CarregarAutores();
        return View();
    }

    // POST: Catalogo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ISBN,Titulo,AutorId")] Catalogo catalogo)
    {
        if (!await _context.Autores
            .AnyAsync(a => a.AutorId == catalogo.AutorId))
        {
            ModelState.AddModelError(
                "AutorId", "Selecione um autor válido.");
        }

        if (ModelState.IsValid)
        {
            _context.Catalogos.Add(catalogo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await CarregarAutores(catalogo.AutorId);
        return View(catalogo);
    }

    // GET: Catalogo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos.FindAsync(id);

        if (catalogo == null)
        {
            return NotFound();
        }

        await CarregarAutores(catalogo.AutorId);
        return View(catalogo);
    }

    // POST: Catalogo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("CatalogoId,ISBN,Titulo,AutorId")] Catalogo catalogo)
    {
        if (id == null || id != catalogo.CatalogoId)
        {
            return NotFound();
        }

        if (!await _context.Autores
            .AnyAsync(a => a.AutorId == catalogo.AutorId))
        {
            ModelState.AddModelError(
                "AutorId", "Selecione um autor válido.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Catalogos.Update(catalogo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalogoExists(catalogo.CatalogoId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        await CarregarAutores(catalogo.AutorId);
        return View(catalogo);
    }

    // GET: Catalogo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos
            .Include(c => c.Autor)
            .FirstOrDefaultAsync(c => c.CatalogoId == id);

        if (catalogo == null)
        {
            return NotFound();
        }

        return View(catalogo);
    }

    // POST: Catalogo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalogo = await _context.Catalogos.FindAsync(id);

        if (catalogo == null)
        {
            return NotFound();
        }

        _context.Catalogos.Remove(catalogo);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarAutores(int? autorSelecionado = null)
    {
        var autores = await _context.Autores
            .OrderBy(a => a.Nome)
            .ToListAsync();

        ViewBag.AutorId = new SelectList(
            autores,
            "AutorId",
            "Nome",
            autorSelecionado
        );
    }

    private bool CatalogoExists(int id)
    {
        return _context.Catalogos
            .Any(c => c.CatalogoId == id);
    }
}
