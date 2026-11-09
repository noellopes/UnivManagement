using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class CriterioElegibilidadesController : Controller
{
    private readonly UniversityDbContext _context;

    public CriterioElegibilidadesController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: CRITERIOELEGIBILIDADES
    public async Task<IActionResult> Index(
        string? successMessage = null,
        string? successType = null)
    {
        ViewBag.SuccessMessage = successMessage;
        ViewBag.SuccessType = successType ?? "success";

        var criterios = _context.CriterioElegibilidade
            .Include(c => c.TipoBolsa);

        return View(await criterios.ToListAsync());
    }

    // GET: CRITERIOELEGIBILIDADES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criterio = await _context.CriterioElegibilidade
            .Include(c => c.TipoBolsa)
            .FirstOrDefaultAsync(m => m.CriterioElegibilidadeId == id);

        if (criterio == null)
        {
            return View("NotFound");
        }

        return View(criterio);
    }

    // GET: CRITERIOELEGIBILIDADES/Create
    public IActionResult Create()
    {
        ViewBag.TipoBolsaId = new SelectList(
            _context.TipoBolsa,
            "TipoBolsaId",
            "Nome");

        return View();
    }

    // POST: CRITERIOELEGIBILIDADES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("CriterioElegibilidadeId,Descricao,RendimentoMaximo,EctsMinimos,TipoBolsaId")]
        CriterioElegibilidade criterio)
    {
        if (ModelState.IsValid)
        {
            _context.Add(criterio);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new
            {
                successMessage = "Eligibility criterion created successfully.",
                successType = "success"
            });
        }

        ViewBag.TipoBolsaId = new SelectList(
            _context.TipoBolsa,
            "TipoBolsaId",
            "Nome",
            criterio.TipoBolsaId);

        return View(criterio);
    }

    // GET: CRITERIOELEGIBILIDADES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criterio = await _context.CriterioElegibilidade.FindAsync(id);

        if (criterio == null)
        {
            return View("NotFound");
        }

        ViewBag.TipoBolsaId = new SelectList(
            _context.TipoBolsa,
            "TipoBolsaId",
            "Nome",
            criterio.TipoBolsaId);

        return View(criterio);
    }

    // POST: CRITERIOELEGIBILIDADES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("CriterioElegibilidadeId,Descricao,RendimentoMaximo,EctsMinimos,TipoBolsaId")]
        CriterioElegibilidade criterio)
    {
        if (id != criterio.CriterioElegibilidadeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(criterio);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriterioElegibilidadeExists(
                        criterio.CriterioElegibilidadeId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index), new
            {
                successMessage = "Eligibility criterion updated successfully.",
                successType = "warning"
            });
        }

        ViewBag.TipoBolsaId = new SelectList(
            _context.TipoBolsa,
            "TipoBolsaId",
            "Nome",
            criterio.TipoBolsaId);

        return View(criterio);
    }

    // GET: CRITERIOELEGIBILIDADES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criterio = await _context.CriterioElegibilidade
            .Include(c => c.TipoBolsa)
            .FirstOrDefaultAsync(m => m.CriterioElegibilidadeId == id);

        if (criterio == null)
        {
            return RedirectToAction(nameof(Index), new
            {
                successMessage =
                    "The eligibility criterion was already deleted by another user.",
                successType = "warning"
            });
        }

        return View(criterio);
    }

    // POST: CRITERIOELEGIBILIDADES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var criterio = await _context.CriterioElegibilidade.FindAsync(id);

        if (criterio != null)
        {
            _context.CriterioElegibilidade.Remove(criterio);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index), new
        {
            successMessage = "Eligibility criterion deleted successfully.",
            successType = "danger"
        });
    }

    private bool CriterioElegibilidadeExists(int? id)
    {
        return _context.CriterioElegibilidade
            .Any(e => e.CriterioElegibilidadeId == id);
    }
}