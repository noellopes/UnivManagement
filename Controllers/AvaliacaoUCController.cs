
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class AvaliacaoUCController : Controller
{
    private readonly UniversityDbContext _context;

    public AvaliacaoUCController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: AvaliacaoUC
    public async Task<IActionResult> Index()
    {
        return View(await _context.AvaliacoesUC.ToListAsync());
    }

    // GET: AvaliacaoUC/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacaouc = await _context.AvaliacoesUC
            .FirstOrDefaultAsync(m => m.AvaliacaoID == id);

        if (avaliacaouc == null)
        {
            return NotFound();
        }

        return View(avaliacaouc);
    }

    // GET: AvaliacaoUC/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AvaliacaoUC/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("AvaliacaoID,T_Avaliacao,UcID,Data")] AvaliacaoUC avaliacaouc)
    {
        if (ModelState.IsValid)
        {
            _context.AvaliacoesUC.Add(avaliacaouc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(avaliacaouc);
    }

    // GET: AvaliacaoUC/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacaouc = await _context.AvaliacoesUC.FindAsync(id);

        if (avaliacaouc == null)
        {
            return NotFound();
        }

        return View(avaliacaouc);
    }

    // POST: AvaliacaoUC/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("AvaliacaoID,T_Avaliacao,UcID,Data")] AvaliacaoUC avaliacaouc)
    {
        if (id != avaliacaouc.AvaliacaoID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(avaliacaouc);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvaliacaoUCExists(avaliacaouc.AvaliacaoID))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(avaliacaouc);
    }

    // GET: AvaliacaoUC/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacaouc = await _context.AvaliacoesUC
            .FirstOrDefaultAsync(m => m.AvaliacaoID == id);

        if (avaliacaouc == null)
        {
            return NotFound();
        }

        return View(avaliacaouc);
    }

    // POST: AvaliacaoUC/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var avaliacaouc = await _context.AvaliacoesUC.FindAsync(id);

        if (avaliacaouc != null)
        {
            _context.AvaliacoesUC.Remove(avaliacaouc);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Verifica se uma avaliação existe
    private bool AvaliacaoUCExists(int id)
    {
        return _context.AvaliacoesUC
            .Any(e => e.AvaliacaoID == id);
    }
}
