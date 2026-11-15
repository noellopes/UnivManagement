
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
    public async Task<IActionResult> Index()    
    {
        return View(await _context.CourseState.ToListAsync());
    }

    // GET: COURSESTATES/Details/5
    public async Task<IActionResult> Details(int? coursestateid)
    {
        if (coursestateid == null)
        {
            return NotFound();
        }

        var coursestate = await _context.CourseState
            .FirstOrDefaultAsync(m => m.CourseStateId == coursestateid);
        if (coursestate == null)
        {
            return NotFound();
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
        if (ModelState.IsValid)
        {
            _context.Add(coursestate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(coursestate);
    }

    // GET: COURSESTATES/Edit/5
    public async Task<IActionResult> Edit(int? coursestateid)
    {
        if (coursestateid == null)
        {
            return NotFound();
        }

        var coursestate = await _context.CourseState.FindAsync(coursestateid);
        if (coursestate == null)
        {
            return NotFound();
        }
        return View(coursestate);
    }

    // POST: COURSESTATES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? coursestateid, [Bind("CourseStateId,Name,Description")] CourseState coursestate)
    {
        if (coursestateid != coursestate.CourseStateId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(coursestate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseStateExists(coursestate.CourseStateId))
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
        return View(coursestate);
    }

    // GET: COURSESTATES/Delete/5
    public async Task<IActionResult> Delete(int? coursestateid)
    {
        if (coursestateid == null)
        {
            return NotFound();
        }

        var coursestate = await _context.CourseState
            .FirstOrDefaultAsync(m => m.CourseStateId == coursestateid);
        if (coursestate == null)
        {
            return NotFound();
        }

        return View(coursestate);
    }

    // POST: COURSESTATES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? coursestateid)
    {
        var coursestate = await _context.CourseState.FindAsync(coursestateid);
        if (coursestate != null)
        {
            _context.CourseState.Remove(coursestate);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CourseStateExists(int? coursestateid)
    {
        return _context.CourseState.Any(e => e.CourseStateId == coursestateid);
    }
}
