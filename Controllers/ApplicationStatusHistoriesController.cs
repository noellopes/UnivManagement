using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models.AccommodationManagement;

namespace UnivManagement.Controllers;

public class ApplicationStatusHistoriesController : Controller
{
    private readonly UniversityDbContext _context;

    public ApplicationStatusHistoriesController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ApplicationStatusHistories
    public async Task<IActionResult> Index(int? applicationId)
    {
        var query = _context.ApplicationStatusHistory
            .Include(h => h.AccommodationApplication)
            .AsQueryable();

        if (applicationId.HasValue)
        {
            query = query.Where(h => h.AccommodationApplicationId == applicationId.Value);
            ViewBag.FilteredApplicationId = applicationId.Value;
        }

        var histories = await query
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();

        return View(histories);
    }

    // GET: ApplicationStatusHistories/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var history = await _context.ApplicationStatusHistory
            .Include(h => h.AccommodationApplication)
            .FirstOrDefaultAsync(m => m.ApplicationStatusHistoryId == id);

        if (history == null)
        {
            return NotFound();
        }

        return View(history);
    }

    // GET: ApplicationStatusHistories/Create
    public IActionResult Create(int? applicationId)
    {
        PopulateApplicationsDropDown(applicationId);

        var model = new ApplicationStatusHistory
        {
            ChangedAt = DateTime.Now,
            ChangedBy = User.Identity?.Name ?? string.Empty
        };

        if (applicationId.HasValue)
        {
            model.AccommodationApplicationId = applicationId.Value;
        }

        return View(model);
    }

    // POST: ApplicationStatusHistories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AccommodationApplicationId,Status,ChangedBy,Notes")] ApplicationStatusHistory history)
    {
        var application = await _context.AccommodationApplication
            .FirstOrDefaultAsync(a => a.AccommodationApplicationId == history.AccommodationApplicationId);

        if (application == null)
        {
            ModelState.AddModelError("AccommodationApplicationId", "The selected application does not exist.");
        }
        else if (application.Status == history.Status)
        {
            ModelState.AddModelError("Status", $"The application is already in the '{history.Status}' status.");
        }

        if (ModelState.IsValid && application != null)
        {
            history.ChangedAt = DateTime.Now;

            // Synchronize the current status on the AccommodationApplication record
            application.Status = history.Status;
            _context.Update(application);
            _context.Add(history);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Application #{application.AccommodationApplicationId} status updated to {history.Status}.";
            return RedirectToAction(nameof(Index), new { applicationId = application.AccommodationApplicationId });
        }

        PopulateApplicationsDropDown(history.AccommodationApplicationId);
        return View(history);
    }

    private void PopulateApplicationsDropDown(int? selectedApplicationId = null)
    {
        var applications = _context.AccommodationApplication
            .OrderByDescending(a => a.AccommodationApplicationId)
            .Select(a => new
            {
                a.AccommodationApplicationId,
                Description = $"Application #{a.AccommodationApplicationId} (Student ID: {a.StudentId} | Current status: {a.Status})"
            })
            .ToList();

        ViewBag.AccommodationApplicationId = new SelectList(applications, "AccommodationApplicationId", "Description", selectedApplicationId);
    }
}