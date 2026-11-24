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
            .Include(h => h.PossibleStatus)
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
            .Include(h => h.PossibleStatus)
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
        PopulateDropDowns(applicationId);

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
    public async Task<IActionResult> Create([Bind("AccommodationApplicationId,PossibleStatusId,ChangedBy,Notes")] ApplicationStatusHistory history)
    {
        var application = await _context.AccommodationApplication
            .FirstOrDefaultAsync(a => a.AccommodationApplicationId == history.AccommodationApplicationId);

        var selectedStatus = await _context.PossibleStatus
            .FirstOrDefaultAsync(s => s.PossibleStatusId == history.PossibleStatusId);

        if (application == null)
        {
            ModelState.AddModelError("AccommodationApplicationId", "The selected application does not exist.");
        }
        else
        {
            var currentStatusId = await _context.ApplicationStatusHistory
                .Where(h => h.AccommodationApplicationId == history.AccommodationApplicationId)
                .OrderByDescending(h => h.ChangedAt)
                .Select(h => (int?)h.PossibleStatusId)
                .FirstOrDefaultAsync();

            if (currentStatusId == history.PossibleStatusId)
            {
                ModelState.AddModelError("PossibleStatusId", $"The application is already in the '{selectedStatus?.Name}' status.");
            }
        }

        if (ModelState.IsValid && application != null)
        {
            history.ChangedAt = DateTime.Now;
            _context.Add(history);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Application #{application.AccommodationApplicationId} status updated to {selectedStatus?.Name}.";
            return RedirectToAction(nameof(Index), new { applicationId = application.AccommodationApplicationId });
        }

        PopulateDropDowns(history.AccommodationApplicationId, history.PossibleStatusId);
        return View(history);
    }

    // GET: ApplicationStatusHistories/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var history = await _context.ApplicationStatusHistory
            .Include(h => h.AccommodationApplication)
            .Include(h => h.PossibleStatus)
            .FirstOrDefaultAsync(m => m.ApplicationStatusHistoryId == id);

        if (history == null)
        {
            return NotFound();
        }

        return View(history);
    }

    // POST: ApplicationStatusHistories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var history = await _context.ApplicationStatusHistory.FindAsync(id);
        if (history != null)
        {
            _context.ApplicationStatusHistory.Remove(history);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Status history record deleted successfully.";
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateDropDowns(int? selectedApplicationId = null, int? selectedStatusId = null)
    {
        var applications = _context.AccommodationApplication
            .OrderByDescending(a => a.AccommodationApplicationId)
            .Select(a => new
            {
                a.AccommodationApplicationId,
                a.StudentId,
                CurrentStatus = _context.ApplicationStatusHistory
                    .Where(h => h.AccommodationApplicationId == a.AccommodationApplicationId)
                    .OrderByDescending(h => h.ChangedAt)
                    .Select(h => h.PossibleStatus != null ? h.PossibleStatus.Name : "Unknown")
                    .FirstOrDefault() ?? "No status"
            })
            .ToList()
            .Select(a => new
            {
                a.AccommodationApplicationId,
                Description = $"Application #{a.AccommodationApplicationId} (Student ID: {a.StudentId} | Current status: {a.CurrentStatus})"
            })
            .ToList();

        ViewBag.AccommodationApplicationId = new SelectList(applications, "AccommodationApplicationId", "Description", selectedApplicationId);
        ViewBag.PossibleStatusId = new SelectList(_context.PossibleStatus.OrderBy(s => s.Name), "PossibleStatusId", "Name", selectedStatusId);
    }
}