
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

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
        return View(await _context.ClassSession.ToListAsync());
    }

    // GET: SESSIONS/Details/5
    [HttpGet("Session/Details/{SessionId}")]
    public async Task<IActionResult> Details(int? classsessionid)
    {
        if (classsessionid == null)
        {
            return NotFound();
        }

        var session = await _context.ClassSession
            .FirstOrDefaultAsync(m => m.ClassSessionId == classsessionid);
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
    public async Task<IActionResult> Edit(int? classsessionid)
    {
        if (classsessionid == null)
        {
            return NotFound();
        }

        var session = await _context.ClassSession.FindAsync(classsessionid);
        if (session == null)
        {
            return NotFound();
        }
        return View(session);
    }

    // POST: SESSIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? classsessionid, [Bind("ClassSessionId,DiaSemana,HoraInicio,HoraFim,TipoAula,Ativo,IdSala,IdTurma,ClassGroup")] Session session)
    {
        if (classsessionid != session.ClassSessionId)
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
                if (!SessionExists(session.ClassSessionId))
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
        return View(session);
    }

    // GET: SESSIONS/Delete/5
    [HttpGet("Session/Delete/{SessionId}")]
    public async Task<IActionResult> Delete(int? classsessionid)
    {
        if (classsessionid == null)
        {
            return NotFound();
        }

        var session = await _context.ClassSession
            .FirstOrDefaultAsync(m => m.ClassSessionId == classsessionid);
        if (session == null)
        {
            return NotFound();
        }

        return View(session);
    }

    // POST: SESSIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? classsessionid)
    {
        var session = await _context.ClassSession.FindAsync(classsessionid);
        if (session != null)
        {
            _context.ClassSession.Remove(session);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SessionExists(int? classsessionid)
    {
        return _context.ClassSession.Any(e => e.ClassSessionId == classsessionid);
    }
}
