
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
    public async Task<IActionResult> Details(int? tipodocumentoid)
    {
        if (tipodocumentoid == null)
        {
            return NotFound();
        }

        var tipodocumento = await _context.TipoDocumento
            .FirstOrDefaultAsync(m => m.TipoDocumentoId == tipodocumentoid);
        if (tipodocumento == null)
        {
            return NotFound();
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
        if (ModelState.IsValid)
        {
            _context.Add(tipodocumento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tipodocumento);
    }

    // GET: TIPODOCUMENTOS/Edit/5
    public async Task<IActionResult> Edit(int? tipodocumentoid)
    {
        if (tipodocumentoid == null)
        {
            return NotFound();
        }

        var tipodocumento = await _context.TipoDocumento.FindAsync(tipodocumentoid);
        if (tipodocumento == null)
        {
            return NotFound();
        }
        return View(tipodocumento);
    }

    // POST: TIPODOCUMENTOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? tipodocumentoid, [Bind("TipoDocumentoId,NomeTipo,Obrigatorio")] TipoDocumento tipodocumento)
    {
        if (tipodocumentoid != tipodocumento.TipoDocumentoId)
        {
            return NotFound();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tipodocumento);
    }

    // GET: TIPODOCUMENTOS/Delete/5
    public async Task<IActionResult> Delete(int? tipodocumentoid)
    {
        if (tipodocumentoid == null)
        {
            return NotFound();
        }

        var tipodocumento = await _context.TipoDocumento
            .FirstOrDefaultAsync(m => m.TipoDocumentoId == tipodocumentoid);
        if (tipodocumento == null)
        {
            return NotFound();
        }

        return View(tipodocumento);
    }

    // POST: TIPODOCUMENTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? tipodocumentoid)
    {
        var tipodocumento = await _context.TipoDocumento.FindAsync(tipodocumentoid);
        if (tipodocumento != null)
        {
            _context.TipoDocumento.Remove(tipodocumento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TipoDocumentoExists(int? tipodocumentoid)
    {
        return _context.TipoDocumento.Any(e => e.TipoDocumentoId == tipodocumentoid);
    }
}
