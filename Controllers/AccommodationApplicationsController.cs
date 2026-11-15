using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

public class AccommodationApplicationsController : Controller
{
    private readonly UniversityDbContext _context;

    public AccommodationApplicationsController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ACCOMMODATIONAPPLICATIONS
    public async Task<IActionResult> Index()
    {
        return View(await _context.AccommodationApplication.ToListAsync());
    }

    // GET: ACCOMMODATIONAPPLICATIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationapplication = await _context.AccommodationApplication
            .FirstOrDefaultAsync(m => m.AccommodationApplicationId == id);
        if (accommodationapplication == null)
        {
            return NotFound();
        }

        return View(accommodationapplication);
    }

    // GET: ACCOMMODATIONAPPLICATIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ACCOMMODATIONAPPLICATIONS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AccommodationApplicationId,StudentId,AcademicYearId,Status,SubmittedAt,HasScholarshipPriority,PriorityCheckedAt,PriorityOverrideReason")] AccommodationApplication accommodationapplication)
    {
        if (ModelState.IsValid)
        {
            _context.Add(accommodationapplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(accommodationapplication);
    }

    // GET: ACCOMMODATIONAPPLICATIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationapplication = await _context.AccommodationApplication.FindAsync(id);
        if (accommodationapplication == null)
        {
            return NotFound();
        }
        return View(accommodationapplication);
    }

    // POST: ACCOMMODATIONAPPLICATIONS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("AccommodationApplicationId,StudentId,AcademicYearId,Status,SubmittedAt,HasScholarshipPriority,PriorityCheckedAt,PriorityOverrideReason")] AccommodationApplication accommodationapplication)
    {
        if (id != accommodationapplication.AccommodationApplicationId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(accommodationapplication);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationApplicationExists(accommodationapplication.AccommodationApplicationId))
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
        return View(accommodationapplication);
    }

    // GET: ACCOMMODATIONAPPLICATIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationapplication = await _context.AccommodationApplication
            .FirstOrDefaultAsync(m => m.AccommodationApplicationId == id);
        if (accommodationapplication == null)
        {
            return NotFound();
        }

        return View(accommodationapplication);
    }

    // POST: ACCOMMODATIONAPPLICATIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var accommodationapplication = await _context.AccommodationApplication.FindAsync(id);
        if (accommodationapplication != null)
        {
            _context.AccommodationApplication.Remove(accommodationapplication);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AccommodationApplicationExists(int? id)
    {
        return _context.AccommodationApplication.Any(e => e.AccommodationApplicationId == id);
    }
}