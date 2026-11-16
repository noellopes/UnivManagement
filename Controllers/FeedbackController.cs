using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class FeedbackController : Controller
{
    private readonly UniversityDbContext _context;

    public FeedbackController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: Feedback
    public async Task<IActionResult> Index()
    {
        var feedbacks = await _context.Feedback
            .Include(f => f.Docente)
            .ToListAsync();

        return View(feedbacks);
    }

    // GET: Feedback/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var feedback = await _context.Feedback
            .Include(f => f.Docente)
            .FirstOrDefaultAsync(m => m.FeedbackId == id);

        if (feedback == null)
        {
            return NotFound();
        }

        return View(feedback);
    }

    // GET: Feedback/Create
    public IActionResult Create()
    {
        PreencherListas();
        return View();
    }

    // POST: Feedback/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FeedbackId,SubmissaoTrabalhoId,DocenteId,DataFeedback,Descricao,Nota")] Feedback feedback)
    {
        if (ModelState.IsValid)
        {
            _context.Add(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PreencherListas(feedback.DocenteId);
        return View(feedback);
    }

    // GET: Feedback/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var feedback = await _context.Feedback.FindAsync(id);
        if (feedback == null)
        {
            return NotFound();
        }

        PreencherListas(feedback.DocenteId);
        return View(feedback);
    }

    // POST: Feedback/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("FeedbackId,SubmissaoTrabalhoId,DocenteId,DataFeedback,Descricao,Nota")] Feedback feedback)
    {
        if (id != feedback.FeedbackId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(feedback);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(feedback.FeedbackId))
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

        PreencherListas(feedback.DocenteId);
        return View(feedback);
    }

    // GET: Feedback/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var feedback = await _context.Feedback
            .Include(f => f.Docente)
            .FirstOrDefaultAsync(m => m.FeedbackId == id);

        if (feedback == null)
        {
            return NotFound();
        }

        return View(feedback);
    }

    // POST: Feedback/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var feedback = await _context.Feedback.FindAsync(id);
        if (feedback != null)
        {
            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool FeedbackExists(int id)
    {
        return _context.Feedback.Any(e => e.FeedbackId == id);
    }

    private void PreencherListas(int? docenteEscolhido = null)
    {
        ViewBag.Docentes = new SelectList(_context.Set<Docente>(), "DocenteId", "Nome", docenteEscolhido);
    }
}