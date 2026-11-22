
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Data;

public class AcademicDegreeController : Controller
{
    private readonly UniversityDbContext _context;

    public AcademicDegreeController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ACADEMICDEGREE
    public async Task<IActionResult> Index(string? successMessage = null)    
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(await _context.AcademicDegree.ToListAsync());
    }

    // GET: ACADEMICDEGREE/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null)
    {
        if (id == null)
        {
            return NotFound();
        }

        var academicdegree = await _context.AcademicDegree
            .FirstOrDefaultAsync(m => m.AcademicDegreeId == id);
        if (academicdegree == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(academicdegree);
    }

    // GET: ACADEMICDEGREE/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ACADEMICDEGREE/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AcademicDegreeId,Name,Description")] AcademicDegree academicdegree)
    {
        if (ModelState.IsValid)
        {
            _context.Add(academicdegree);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = academicdegree.AcademicDegreeId, successMessage = "Grau académico criado com sucesso." });
        }
        return View(academicdegree);
    }

    // GET: ACADEMICDEGREE/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var academicdegree = await _context.AcademicDegree.FindAsync(id);
        if (academicdegree == null)
        {
            return View("NotFound");
        }
        return View(academicdegree);
    }

    // POST: ACADEMICDEGREE/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("AcademicDegreeId,Name,Description")] AcademicDegree academicdegree)
    {
        if (id != academicdegree.AcademicDegreeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(academicdegree);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcademicDegreeExists(academicdegree.AcademicDegreeId))
                {
                    return View("NotFound");
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Details), new { id = academicdegree.AcademicDegreeId, successMessage = "Grau académico atualizado com sucesso." });
        }
        return View(academicdegree);
    }

    // GET: ACADEMICDEGREE/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var academicdegree = await _context.AcademicDegree
            .FirstOrDefaultAsync(m => m.AcademicDegreeId == id);
        if (academicdegree == null)
        {
            return RedirectToAction(nameof(Index), new { successMessage = "Aviso: O Grau Académico já tinha sido eliminado por outro utilizador." });
        }

        return View(academicdegree);
    }

    // POST: ACADEMICDEGREE/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var academicdegree = await _context.AcademicDegree.FindAsync(id);
        if (academicdegree != null)
        {
            _context.AcademicDegree.Remove(academicdegree);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index),
    new { successMessage = "Grau Académico eliminado com sucesso." });
    }

    private bool AcademicDegreeExists(int? id)
    {
        return _context.AcademicDegree.Any(e => e.AcademicDegreeId == id);
    }
}
