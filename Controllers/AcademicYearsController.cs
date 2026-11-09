
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Controllers;

public class AcademicYearsController : Controller
{
    private readonly UniversityDbContext _context;

    public AcademicYearsController(UniversityDbContext context)
    {
        _context = context;
    }

    // Lists all academic years
    public async Task<IActionResult> Index()
    {
        return View(await _context.AcademicYear.ToListAsync());
    }

    // Displays the details of an academic year
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var academicYear = await _context.AcademicYear
            .FirstOrDefaultAsync(m => m.AcademicYearId == id);

        if (academicYear == null)
        {
            return NotFound();
        }

        return View(academicYear);
    }

    // Opens the creation form
    public IActionResult Create()
    {
        return View();
    }

    // Saves a new academic year
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StartYear,EndYear")] AcademicYear academicYear)
    {
        if (ModelState.IsValid)
        {
            _context.Add(academicYear);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(academicYear);
    }

    // Opens the edit form
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var academicYear = await _context.AcademicYear.FindAsync(id);

        if (academicYear == null)
        {
            return NotFound();
        }

        return View(academicYear);
    }

    // Updates an academic year's information
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AcademicYearId,StartYear,EndYear")] AcademicYear academicYear)
    {
        if (id != academicYear.AcademicYearId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(academicYear);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AcademicYearExists(academicYear.AcademicYearId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(academicYear);
    }

    // Displays the academic year before deletion
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var academicYear = await _context.AcademicYear
            .FirstOrDefaultAsync(m => m.AcademicYearId == id);

        if (academicYear == null)
        {
            return NotFound();
        }

        return View(academicYear);
    }

    // Deletes an academic year
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var academicYear = await _context.AcademicYear.FindAsync(id);

        if (academicYear != null)
        {
            _context.AcademicYear.Remove(academicYear);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Checks if an academic year exists
    private bool AcademicYearExists(int id)
    {
        return _context.AcademicYear.Any(e => e.AcademicYearId == id);
    }
}
