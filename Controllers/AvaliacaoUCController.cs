using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var avaliacoes = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .ToListAsync();

        return View(avaliacoes);
    }

    // GET: AvaliacaoUC/IndexDocente
    public async Task<IActionResult> IndexDocente()
    {
        var avaliacoes = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .ToListAsync();

        return View(avaliacoes);
    }

    // GET: AvaliacaoUC/IndexAluno
    public async Task<IActionResult> IndexAluno()
    {
        var avaliacoes = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .ToListAsync();

        return View(avaliacoes);
    }

    // GET: AvaliacaoUC/DetailsAluno/5
    public async Task<IActionResult> DetailsAluno(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacao = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

        if (avaliacao == null)
        {
            return NotFound();
        }

        return View(avaliacao);
    }

    // GET: AvaliacaoUC/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var avaliacaouc = await _context.AvaliacoesUC
            .Include(a => a.TipoAvaliacao)
            .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

        if (avaliacaouc == null)
        {
            return NotFound();
        }

        return View(avaliacaouc);
    }

    // GET: AvaliacaoUC/Create
    public IActionResult Create()
    {
        ViewBag.TiposAvaliacao = new SelectList(
            _context.TiposAvaliacao,
            "TipoAvaliacaoId",
            "TipoProva"
        );

        return View();
    }

    // POST: AvaliacaoUC/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("AvaliacaoUCId,TipoAvaliacaoId,UcId,Data")] AvaliacaoUC avaliacaouc)
    {
        if (ModelState.IsValid)
        {
            _context.AvaliacoesUC.Add(avaliacaouc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(IndexDocente));
        }

        ViewBag.TiposAvaliacao = new SelectList(
            _context.TiposAvaliacao,
            "TipoAvaliacaoId",
            "TipoProva",
            avaliacaouc.TipoAvaliacaoId
        );

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

        ViewBag.TiposAvaliacao = new SelectList(
            _context.TiposAvaliacao,
            "TipoAvaliacaoId",
            "TipoProva",
            avaliacaouc.TipoAvaliacaoId
        );

        return View(avaliacaouc);
    }

    // POST: AvaliacaoUC/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("AvaliacaoUCId,TipoAvaliacaoId,UcId,Data")] AvaliacaoUC avaliacaouc)
    {
        if (id != avaliacaouc.AvaliacaoUCId)
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
                if (!AvaliacaoUCExists(avaliacaouc.AvaliacaoUCId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(IndexDocente));
        }

        ViewBag.TiposAvaliacao = new SelectList(
            _context.TiposAvaliacao,
            "TipoAvaliacaoId",
            "TipoProva",
            avaliacaouc.TipoAvaliacaoId
        );

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
            .Include(a => a.TipoAvaliacao)
            .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

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
        // Impede apagar avaliações que já têm notas associadas
        if (_context.Notas.Any(n => n.AvaliacaoUCId == id))
        {
            ViewBag.Erro = "Não pode apagar esta avaliação porque existem notas associadas.";

            var avaliacao = await _context.AvaliacoesUC
                .Include(a => a.TipoAvaliacao)
                .FirstOrDefaultAsync(a => a.AvaliacaoUCId == id);

            return View(avaliacao);
        }

        var avaliacaouc = await _context.AvaliacoesUC.FindAsync(id);

        if (avaliacaouc != null)
        {
            _context.AvaliacoesUC.Remove(avaliacaouc);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(IndexDocente));
    }

    // Verifica se uma avaliação existe
    private bool AvaliacaoUCExists(int id)
    {
        return _context.AvaliacoesUC
            .Any(a => a.AvaliacaoUCId == id);
    }
}
