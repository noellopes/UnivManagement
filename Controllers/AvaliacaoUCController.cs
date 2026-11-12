
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

    // GET: AVALIACAOUCS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.AvaliacoesUC.ToListAsync());
    }

    // GET: AVALIACAOUCS/Details/5
    public async Task<IActionResult> Details(int? avaliacaoid)
    {
        if (avaliacaoid == null)
        {
            return NotFound();
        }

        var avaliacaouc = await _context.AvaliacoesUC
            .FirstOrDefaultAsync(m => m.AvaliacaoID == avaliacaoid);
        if (avaliacaouc == null)
        {
            return NotFound();
        }

        return View(avaliacaouc);
    }

    // GET: AVALIACAOUCS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AVALIACAOUCS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AvaliacaoID,T_Avaliacao,UcID,Data")] AvaliacaoUC avaliacaouc)
    {
        if (ModelState.IsValid)
        {
            _context.Add(avaliacaouc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(avaliacaouc);
    }

    // GET: AVALIACAOUCS/Edit/5
    public async Task<IActionResult> Edit(int? avaliacaoid)
    {
        if (avaliacaoid == null)
        {
            return NotFound();
        }

        var avaliacaouc = await _context.AvaliacoesUC.FindAsync(avaliacaoid);
        if (avaliacaouc == null)
        {
            return NotFound();
        }
        return View(avaliacaouc);
    }

    // POST: AVALIACAOUCS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? avaliacaoid, [Bind("AvaliacaoID,T_Avaliacao,UcID,Data")] AvaliacaoUC avaliacaouc)
    {
        if (avaliacaoid != avaliacaouc.AvaliacaoID)
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
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(avaliacaouc);
    }

    // GET: AVALIACAOUCS/Delete/5
    public async Task<IActionResult> Delete(int? avaliacaoid)
    {
        if (avaliacaoid == null)
        {
            return NotFound();
        }

        var avaliacaouc = await _context.AvaliacoesUC
            .FirstOrDefaultAsync(m => m.AvaliacaoID == avaliacaoid);
        if (avaliacaouc == null)
        {
            return NotFound();
        }

        return View(avaliacaouc);
    }

    // POST: AVALIACAOUCS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? avaliacaoid)
    {
        var avaliacaouc = await _context.AvaliacoesUC.FindAsync(avaliacaoid);
        if (avaliacaouc != null)
        {
            _context.AvaliacoesUC.Remove(avaliacaouc);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AvaliacaoUCExists(int? avaliacaoid)
    {
        return _context.AvaliacoesUC.Any(e => e.AvaliacaoID == avaliacaoid);
    }
}
