
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Data;

public class CourseStateController : Controller
{
    private readonly UniversityDbContext _context;

    public CourseStateController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: COURSESTATES
    public async Task<IActionResult> Index(string? successMessage = null)    
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(await _context.CourseState.ToListAsync());
    }

    // GET: COURSESTATE/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null)
    {
        if (id == null)
        {
            return NotFound();
        }

        var coursestate = await _context.CourseState
            .FirstOrDefaultAsync(m => m.CourseStateId == id);

        if (coursestate == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(coursestate);
    }

    // GET: COURSESTATES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: COURSESTATES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CourseStateId,Name,Description")] CourseState coursestate)
    {

        /* Unicidade do Name */
        if (await _context.CourseState.AnyAsync(cs => cs.Name == coursestate.Name))
            ModelState.AddModelError(nameof(CourseState.Name),
                "Já existe um estado de curso com este Nome.");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(coursestate);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details),
                    new { id = coursestate.CourseStateId, successMessage = "Estado de curso criado com sucesso." }
                );
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                ModelState.AddModelError("",
                    "Não foi possível guardar: já existe um estado com este Nome.");
            }

        }
        return View(coursestate);
    }

    // GET: COURSESTATES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var coursestate = await _context.CourseState.FindAsync(id);
        
        if (coursestate == null)
        {
            return View("NotFound");
        }
        return View(coursestate);
    }

    // POST: COURSESTATES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("CourseStateId,Name,Description")] CourseState coursestate)
    {
        if (id != coursestate.CourseStateId)
        {
            return NotFound();
        }

        /* Unicidade do Name (excluindo o próprio) */
        if (await _context.CourseState.AnyAsync(cs =>
                cs.Name == coursestate.Name && cs.CourseStateId != coursestate.CourseStateId))
            ModelState.AddModelError(nameof(CourseState.Name),
                "Já existe outro estado de curso com este Nome.");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(coursestate);
                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(Details),
                    new { id = coursestate.CourseStateId, successMessage = "Dados do estado atualizados com sucesso." }
                );
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseStateExists(coursestate.CourseStateId))
                {
                    return View("NotFound");
                }
                
                    throw;
                
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                ModelState.AddModelError("",
                    "Não foi possível guardar: já existe um estado com este Nome.");
            }
          
        }
        return View(coursestate);
    }

    // GET: COURSESTATE/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var coursestate = await _context.CourseState
            .FirstOrDefaultAsync(m => m.CourseStateId == id);

        if (coursestate == null)
        {
            return RedirectToAction(nameof(Index),
                new { successMessage = "Aviso: O Estado já tinha sido eliminado por outro utilizador." });
        }


        return View(coursestate);
    }

    // POST: COURSESTATES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var coursestate = await _context.CourseState.FindAsync(id);

        if (coursestate == null)
        {
            return RedirectToAction(nameof(Index),
                new { successMessage = "Aviso: O Estado já não existe." });
        }

        try
        {
            _context.CourseState.Remove(coursestate);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // FK violation: o estado está a ser usado por cursos
            return RedirectToAction(nameof(Index),
                new { successMessage = "Não é possível eliminar este Estado porque está a ser utilizado por cursos." });
        }

        return RedirectToAction(nameof(Index),
            new { successMessage = "Estado de curso eliminado com sucesso." });
    }

    private bool CourseStateExists(int? id)
    {
        return _context.CourseState.Any(e => e.CourseStateId == id);
    }

    private static bool IsUniqueViolation(DbUpdateException ex)
    {
        return ex.InnerException is Microsoft.Data.SqlClient.SqlException sql
            && (sql.Number == 2601 || sql.Number == 2627);
    }
}
