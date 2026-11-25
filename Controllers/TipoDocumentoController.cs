
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class TipoDocumentoController : Controller
{
    private readonly UniversityDbContext _context;

    public TipoDocumentoController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: TIPODOCUMENTOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TipoDocumento.ToListAsync());
    }

    // GET: TIPODOCUMENTOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
             return View("NotFound");
        }

        var tipodocumento = await _context.TipoDocumento
            .FirstOrDefaultAsync(m => m.TipoDocumentoId == id);
        if (tipodocumento == null)
        {
            return View("NotFound");
        }

        return View(tipodocumento);
    }

    // GET: TIPODOCUMENTOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TIPODOCUMENTOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TipoDocumentoId,NomeTipo,Obrigatorio")] TipoDocumento tipodocumento)
    {
        if (await _context.TipoDocumento.AnyAsync(t => t.NomeTipo == tipodocumento.NomeTipo))
        {
            ModelState.AddModelError(nameof(TipoDocumento.NomeTipo), "Já existe um tipo de documento com este nome.");
        }
        if (ModelState.IsValid)
        {
            _context.Add(tipodocumento);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Tipo de documento criado com sucesso.";
            TempData["MessageType"] = "success";
            return RedirectToAction(nameof(Index));
        }
        return View(tipodocumento);
    }

    // GET: TIPODOCUMENTOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return View("NotFound");
        }

        var tipodocumento = await _context.TipoDocumento.FindAsync(id);
        if (tipodocumento == null)
        {
            return View("NotFound");

        }
        return View(tipodocumento);
    }

    // POST: TIPODOCUMENTOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("TipoDocumentoId,NomeTipo,Obrigatorio")] TipoDocumento tipodocumento)
    {
        if (id != tipodocumento.TipoDocumentoId)
        {
            return NotFound();
        }
        if (await _context.TipoDocumento.AnyAsync(t =>
        t.NomeTipo == tipodocumento.NomeTipo &&
        t.TipoDocumentoId != tipodocumento.TipoDocumentoId))
        {
            ModelState.AddModelError(nameof(TipoDocumento.NomeTipo), "Já existe um tipo de documento com este nome.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tipodocumento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoDocumentoExists(tipodocumento.TipoDocumentoId))
                {
                    return View("NotFound");

                }
                else
                {
                    throw;
                }
            }
            TempData["SuccessMessage"] = "Tipo de documento atualizado com sucesso.";
            TempData["MessageType"] = "warning";
            return RedirectToAction(nameof(Index));
        }
        return View(tipodocumento);
    }

    // GET: TIPODOCUMENTOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tipodocumento = await _context.TipoDocumento
            .FirstOrDefaultAsync(m => m.TipoDocumentoId == id);
        if (tipodocumento == null)
        {
            return View("NotFound");

        }

        return View(tipodocumento);
    }

    // POST: TIPODOCUMENTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var tipodocumento = await _context.TipoDocumento.FindAsync(id);
        if (tipodocumento != null)
        {
            _context.TipoDocumento.Remove(tipodocumento);
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Tipo de documento eliminado com sucesso.";
        TempData["MessageType"] = "danger";
        return RedirectToAction(nameof(Index));
    }

    private bool TipoDocumentoExists(int? id)
    {
        return _context.TipoDocumento.Any(e => e.TipoDocumentoId == id);
    }
}
