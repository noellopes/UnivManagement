
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Data;

public class StateEnrollmentController : Controller
{
    private readonly UniversityDbContext _context;

    public StateEnrollmentController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: STATEENROLLMENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.StateEnrollment.ToListAsync());
    }

    // GET: STATEENROLLMENTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stateenrollment = await _context.StateEnrollment
            .FirstOrDefaultAsync(m => m.StateEnrollmentId == id);
        if (stateenrollment == null)
        {
            return NotFound();
        }

        return View(stateenrollment);
    }

    // GET: STATEENROLLMENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: STATEENROLLMENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StateEnrollmentId,Name,Description")] StateEnrollment stateenrollment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(stateenrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(stateenrollment);
    }

    // GET: STATEENROLLMENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stateenrollment = await _context.StateEnrollment.FindAsync(id);
        if (stateenrollment == null)
        {
            return NotFound();
        }
        return View(stateenrollment);
    }

    // POST: STATEENROLLMENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("StateEnrollmentId,Name,Description")] StateEnrollment stateenrollment)
    {
        if (id != stateenrollment.StateEnrollmentId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(stateenrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateEnrollmentExists(stateenrollment.StateEnrollmentId))
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
        return View(stateenrollment);
    }

    // GET: STATEENROLLMENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stateenrollment = await _context.StateEnrollment
            .FirstOrDefaultAsync(m => m.StateEnrollmentId == id);
        if (stateenrollment == null)
        {
            return NotFound();
        }

        return View(stateenrollment);
    }

    // POST: STATEENROLLMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var stateenrollment = await _context.StateEnrollment.FindAsync(id);
        if (stateenrollment != null)
        {
            _context.StateEnrollment.Remove(stateenrollment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StateEnrollmentExists(int? id)
    {
        return _context.StateEnrollment.Any(e => e.StateEnrollmentId == id);
    }
}
