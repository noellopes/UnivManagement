using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class FeedbackController : Controller
{
    private readonly UniversityDbContext _context;

    public FeedbackController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: FEEDBACKS
    public async Task<IActionResult> Index()
    {
        return View(await _context.Feedback.ToListAsync());
    }

    // GET: FEEDBACKS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var feedback = await _context.Feedback
            .FirstOrDefaultAsync(m => m.FeedbackID == id);
        if (feedback == null)
        {
            return NotFound();
        }

        return View(feedback);
    }

    // GET: FEEDBACKS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FEEDBACKS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FeedbackID,SubmissaoID,DocenteID,Data_Feedback,Descricao,Nota")] Feedback feedback)
    {
        if (ModelState.IsValid)
        {
            _context.Add(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(feedback);
    }

    // GET: FEEDBACKS/Edit/5
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
        return View(feedback);
    }

    // POST: FEEDBACKS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("FeedbackID,SubmissaoID,DocenteID,Data_Feedback,Descricao,Nota")] Feedback feedback)
    {
        if (id != feedback.FeedbackID)
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
                if (!FeedbackExists(feedback.FeedbackID))
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
        return View(feedback);
    }

    // GET: FEEDBACKS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var feedback = await _context.Feedback
            .FirstOrDefaultAsync(m => m.FeedbackID == id);
        if (feedback == null)
        {
            return NotFound();
        }

        return View(feedback);
    }

    // POST: FEEDBACKS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var feedback = await _context.Feedback.FindAsync(id);
        if (feedback != null)
        {
            _context.Feedback.Remove(feedback);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FeedbackExists(int id)
    {
        return _context.Feedback.Any(e => e.FeedbackID == id);
    }
}