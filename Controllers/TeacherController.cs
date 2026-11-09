
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class TeacherController : Controller
{
    private readonly UniversityDbContext _context;

    public TeacherController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: TEACHERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Teacher.ToListAsync());
    }

    // GET: TEACHERS/Details/5
    [HttpGet("Teacher/Details/{TeacherId}")]
    public async Task<IActionResult> Details(int? teacherid)
    {
        if (teacherid == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teacher
            .FirstOrDefaultAsync(m => m.TeacherId == teacherid);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    // GET: TEACHERS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TEACHERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TeacherId,NumeroMecanografico,Departamento")] Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    // GET: TEACHERS/Edit/5
    [HttpGet("Teacher/Edit/{TeacherId}")]
    public async Task<IActionResult> Edit(int? teacherid)
    {
        if (teacherid == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teacher.FindAsync(teacherid);
        if (teacher == null)
        {
            return NotFound();
        }
        return View(teacher);
    }

    // POST: TEACHERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? teacherid, [Bind("TeacherId,NumeroMecanografico,Departamento")] Teacher teacher)
    {
        if (teacherid != teacher.TeacherId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.TeacherId))
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
        return View(teacher);
    }

    // GET: TEACHERS/Delete/5
    [HttpGet("Teacher/Delete/{TeacherId}")]
    public async Task<IActionResult> Delete(int? teacherid)
    {
        if (teacherid == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teacher
            .FirstOrDefaultAsync(m => m.TeacherId == teacherid);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    // POST: TEACHERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? teacherid)
    {
        var teacher = await _context.Teacher.FindAsync(teacherid);
        if (teacher != null)
        {
            _context.Teacher.Remove(teacher);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(int? teacherid)
    {
        return _context.Teacher.Any(e => e.TeacherId == teacherid);
    }
}
