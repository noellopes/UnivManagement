
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

public class SessionController : Controller
{
    private readonly UniversityDbContext _context;

    public SessionController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: SESSIONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ClassSession.Include(s => s.Group).ToListAsync());
    }

    // GET: SESSIONS/Details/5
    [HttpGet("Session/Details/{SessionId}")]
    public async Task<IActionResult> Details(int? SessionId)
    {
        if (SessionId == null)
        {
            return NotFound();
        }

        var session = await _context.ClassSession
            .Include(s => s.Group)
            .FirstOrDefaultAsync(m => m.SessionId == SessionId);
        if (session == null)
        {
            return NotFound();
        }

        return View(session);
    }

    // GET: SESSIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SESSIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ClassSessionId,DiaSemana,HoraInicio,HoraFim,TipoAula,Ativo,IdSala,IdTurma")] Session session)
    {
        if (ModelState.IsValid)
        {
            _context.Add(session);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(session);
    }

    // GET: SESSIONS/Edit/5
    [HttpGet("Session/Edit/{SessionId}")]
    public async Task<IActionResult> Edit(int? SessionId)
    {
        if (SessionId == null)
        {
            return NotFound();
        }

        var session = await _context.ClassSession.FindAsync(SessionId);
        if (session == null)
        {
            return NotFound();
        }
        ViewData["IdTurma"] = new SelectList(_context.ClassGroup, "GroupId", "NomeTurma", session.IdTurma);
        return View(session);
    }

    // POST: SESSIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int SessionId, [Bind("SessionId,DiaSemana,HoraInicio,HoraFim,TipoAula,Ativo,IdSala,IdTurma")] Session session)
    {
        if (SessionId != session.SessionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(session);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ClassSession.Any(e => e.SessionId == session.SessionId))
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
        ViewData["IdTurma"] = new SelectList(_context.ClassGroup, "GroupId", "NomeTurma", session.IdTurma);
        return View(session);
    }

    // GET: SESSIONS/Delete/5
    [HttpGet("Session/Delete/{SessionId}")]
    public async Task<IActionResult> Delete(int? SessionId)
    {
        if (SessionId == null)
        {
            return NotFound();
        }

        var session = await _context.ClassSession
            .Include(s => s.Group)
            .FirstOrDefaultAsync(m => m.SessionId == SessionId);
        if (session == null)
        {
            return NotFound();
        }

        return View(session);
    }

    // POST: SESSIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int SessionId)
    {
        var session = await _context.ClassSession.FindAsync(SessionId);
        if (session != null)
        {
            _context.ClassSession.Remove(session);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SessionExists(int? classsessionid)
    {
        return _context.ClassSession.Any(e => e.SessionId == classsessionid);
    }
}
