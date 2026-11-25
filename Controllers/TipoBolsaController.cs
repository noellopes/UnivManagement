using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class TipoBolsaController : Controller
{
    private readonly UniversityDbContext _context;

    public TipoBolsaController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: TIPOBOLSAS
    public async Task<IActionResult> Index(string? successMessage, string? successType)
    {
        ViewBag.SuccessMessage = successMessage;
        ViewBag.SuccessType = successType;
        return View(await _context.TipoBolsa.ToListAsync());
    }

    // GET: TIPOBOLSAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tipobolsa = await _context.TipoBolsa
            .FirstOrDefaultAsync(m => m.TipoBolsaId == id);
        if (tipobolsa == null)
        {
            return NotFound();
        }

        return View(tipobolsa);
    }

    // GET: TIPOBOLSAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TIPOBOLSAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TipoBolsaId,Nome,Descricao,RendimentoMaximo,EctsMinimos,MediaMinima,CriteriosElegibilidade")] TipoBolsa tipobolsa)
    {
        if (_context.TipoBolsa.Any(t => t.Nome == tipobolsa.Nome))
        {
            ViewBag.Erro = "Já existe um tipo de bolsa com este nome.";
            return View(tipobolsa);
        }

        if (ModelState.IsValid)
        {
            _context.Add(tipobolsa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { successMessage = "Tipo de bolsa criado com sucesso.", successType = "success" });
        }
        return View(tipobolsa);
    }

    // GET: TIPOBOLSAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tipobolsa = await _context.TipoBolsa.FindAsync(id);
        if (tipobolsa == null)
        {
            return NotFound();
        }
        return View(tipobolsa);
    }

    // POST: TIPOBOLSAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TipoBolsaId,Nome,Descricao,RendimentoMaximo,EctsMinimos,MediaMinima,CriteriosElegibilidade")] TipoBolsa tipobolsa)
    {
        if (id != tipobolsa.TipoBolsaId)
        {
            return NotFound();
        }

        if (_context.TipoBolsa.Any(t => t.Nome == tipobolsa.Nome && t.TipoBolsaId != tipobolsa.TipoBolsaId))
        {
            ViewBag.Erro = "Já existe um tipo de bolsa com este nome.";
            return View(tipobolsa);
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tipobolsa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoBolsaExists(tipobolsa.TipoBolsaId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), new { successMessage = "Tipo de bolsa atualizado com sucesso.", successType = "warning" });
        }
        return View(tipobolsa);
    }

    // GET: TIPOBOLSAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tipobolsa = await _context.TipoBolsa
            .FirstOrDefaultAsync(m => m.TipoBolsaId == id);
        if (tipobolsa == null)
        {
            return NotFound();
        }

        return View(tipobolsa);
    }

    // POST: TIPOBOLSAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tipobolsa = await _context.TipoBolsa.FindAsync(id);
        if (tipobolsa != null)
        {
            _context.TipoBolsa.Remove(tipobolsa);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { successMessage = "Tipo de bolsa eliminado com sucesso.", successType = "danger" });
    }

    private bool TipoBolsaExists(int id)
    {
        return _context.TipoBolsa.Any(e => e.TipoBolsaId == id);
    }
}