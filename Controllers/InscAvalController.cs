using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class InscAvalController : Controller
{
    private readonly UniversityDbContext _context;

    public InscAvalController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: InscAval
    public async Task<IActionResult> Index()
    {
        var inscricoes = await _context.InscAval
            .Include(i => i.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(i => i.InscrUC)
            .ToListAsync();

        return View(inscricoes);
    }

    // GET: InscAval/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inscAval = await _context.InscAval
            .Include(i => i.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(i => i.InscrUC)
            .FirstOrDefaultAsync(i => i.InscAvalId == id);

        if (inscAval == null)
        {
            return NotFound();
        }

        return View(inscAval);
    }

    // GET: InscAval/Create
    public IActionResult Create()
    {
        PreencherListas();
        return View();
    }

    // POST: InscAval/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("InscAvalId,AvaliacaoUCId,InscrUcId,Pago")] InscAval novaInscAval)
    {
        if (ModelState.IsValid)
        {
            _context.InscAval.Add(novaInscAval);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        PreencherListas(novaInscAval.AvaliacaoUCId, novaInscAval.InscrUcId);
        return View(novaInscAval);
    }

    // GET: InscAval/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inscAval = await _context.InscAval.FindAsync(id);

        if (inscAval == null)
        {
            return NotFound();
        }

        PreencherListas(inscAval.AvaliacaoUCId, inscAval.InscrUcId);
        return View(inscAval);
    }

    // POST: InscAval/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("InscAvalId,AvaliacaoUCId,InscrUcId,Pago")] InscAval inscAvalEditada)
    {
        if (id != inscAvalEditada.InscAvalId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.InscAval.Update(inscAvalEditada);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscAvalExiste(inscAvalEditada.InscAvalId))
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

        PreencherListas(inscAvalEditada.AvaliacaoUCId, inscAvalEditada.InscrUcId);
        return View(inscAvalEditada);
    }

    // GET: InscAval/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var inscAval = await _context.InscAval
            .Include(i => i.AvaliacaoUC)
                .ThenInclude(a => a!.TipoAvaliacao)
            .Include(i => i.InscrUC)
            .FirstOrDefaultAsync(i => i.InscAvalId == id);

        if (inscAval == null)
        {
            return NotFound();
        }

        return View(inscAval);
    }

    // POST: InscAval/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inscAval = await _context.InscAval.FindAsync(id);

        if (inscAval != null)
        {
            _context.InscAval.Remove(inscAval);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool InscAvalExiste(int id)
    {
        return _context.InscAval.Any(i => i.InscAvalId == id);
    }

    // Listas para escolher a avaliação e a inscrição nos formulários
    private void PreencherListas(int? avaliacaoEscolhida = null, int? inscricaoEscolhida = null)
    {
        var avaliacoes = _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .OrderBy(a => a.Data)
            .Select(a => new
            {
                a.AvaliacaoUCId,
                Texto = a.TipoAvaliacao.TipoProva + " - UC " + a.UcId + " - " + a.Data.ToString("dd/MM/yyyy")
            })
            .ToList();

        ViewData["AvaliacaoUCId"] = new SelectList(avaliacoes, "AvaliacaoUCId", "Texto", avaliacaoEscolhida);
        ViewData["InscrUcId"] = new SelectList(_context.InscrUC, "InscrUCId", "InscrUCId", inscricaoEscolhida);
    }
}
