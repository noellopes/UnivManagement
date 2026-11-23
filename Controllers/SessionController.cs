using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
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
            return View(await _context.ClassSession
                .Include(s => s.Group)
                .Include(s => s.Course)
                .Include(s => s.AcademicYear)
                .ToListAsync());
        }

        // GET: SESSIONS/Details/5
        [HttpGet("Session/Details/{SessionId}")]
        public async Task<IActionResult> Details(int? SessionId)
        {
            if (SessionId == null || SessionId <= 0)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var session = await _context.ClassSession
                .Include(s => s.Group)
                .Include(s => s.Course)
                .Include(s => s.AcademicYear)
                .FirstOrDefaultAsync(m => m.SessionId == SessionId);

            if (session == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            return View(session);
        }

        // GET: SESSIONS/Create
        public IActionResult Create()
        {
            CarregarDropdowns();
            return View();
        }

        // POST: SESSIONS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId,DiaSemana,HoraInicio,HoraFim,TipoAula,Ativo,IdSala,IdTurma,CourseId,AcademicYearId,Semestre")] Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Sessão registada com sucesso!";
                return RedirectToAction(nameof(Details), new { SessionId = session.SessionId });
            }

            CarregarDropdowns(session);
            return View(session);
        }

        // GET: SESSIONS/Edit/5
        [HttpGet("Session/Edit/{SessionId}")]
        public async Task<IActionResult> Edit(int? SessionId)
        {
            if (SessionId == null || SessionId <= 0)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var session = await _context.ClassSession.FindAsync(SessionId);
            if (session == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            CarregarDropdowns(session);
            return View(session);
        }

        // POST: SESSIONS/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int SessionId, [Bind("SessionId,DiaSemana,HoraInicio,HoraFim,TipoAula,Ativo,IdSala,IdTurma,CourseId,AcademicYearId,Semestre")] Session session)
        {
            if (SessionId != session.SessionId)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sessão atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.SessionId))
                    {
                        Response.StatusCode = 404;
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            CarregarDropdowns(session);
            return View(session);
        }

        // GET: SESSIONS/Delete/5
        [HttpGet("Session/Delete/{SessionId}")]
        public async Task<IActionResult> Delete(int? SessionId)
        {
            if (SessionId == null || SessionId <= 0)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            var session = await _context.ClassSession
                .Include(s => s.Group)
                .Include(s => s.Course)
                .Include(s => s.AcademicYear)
                .FirstOrDefaultAsync(m => m.SessionId == SessionId);

            if (session == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            return View(session);
        }

        // POST: SESSIONS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int SessionId)
        {
            var session = await _context.ClassSession.FindAsync(SessionId);
            if (session == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            _context.ClassSession.Remove(session);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sessão eliminada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private void CarregarDropdowns(Session? session = null)
        {
            var anosLetivos = _context.AcademicYear
                .Select(a => new
                {
                    a.AcademicYearId,
                    Descricao = a.StartYear + "/" + a.EndYear
                })
                .ToList();

            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "Name", session?.CourseId);
            ViewData["AcademicYearId"] = new SelectList(anosLetivos, "AcademicYearId", "Descricao", session?.AcademicYearId);
            ViewData["IdTurma"] = new SelectList(_context.ClassGroup, "GroupId", "NomeTurma", session?.IdTurma);
        }

        private bool SessionExists(int? sessionId)
        {
            return _context.ClassSession.Any(e => e.SessionId == sessionId);
        }
    }
}