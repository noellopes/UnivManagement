
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

public class RoomAllocationsController : Controller
{
    private readonly UniversityDbContext _context;

    public RoomAllocationsController(UniversityDbContext context)
    {
        _context = context;
    }

    // Loads the available applications, students and rooms for the dropdown lists
    private async Task LoadDropdownsAsync(
        int? selectedApplicationId = null,
        int? selectedStudentId = null,
        int? selectedRoomId = null)
    {
        var applications = await _context.AccommodationApplication
            .OrderBy(a => a.AccommodationApplicationId)
            .Select(a => new SelectListItem
            {
                Value = a.AccommodationApplicationId.ToString(),
                Text = "Application #" + a.AccommodationApplicationId,
                Selected = a.AccommodationApplicationId == selectedApplicationId
            })
            .ToListAsync();

        var students = await _context.Student
            .OrderBy(s => s.StudentId)
            .Select(s => new SelectListItem
            {
                Value = s.StudentId.ToString(),
                Text = "Student #" + s.StudentId,
                Selected = s.StudentId == selectedStudentId
            })
            .ToListAsync();

        var rooms = await _context.Room
            .OrderBy(r => r.RoomId)
            .Select(r => new SelectListItem
            {
                Value = r.RoomId.ToString(),
                Text = "Room #" + r.RoomId,
                Selected = r.RoomId == selectedRoomId
            })
            .ToListAsync();

        ViewData["AccommodationApplicationId"] = applications;
        ViewData["StudentId"] = students;
        ViewData["RoomId"] = rooms;
    }

    // GET: ROOMALLOCATIONS
    public async Task<IActionResult> Index()
    {
        return View(await _context.RoomAllocation.ToListAsync());
    }

    // GET: ROOMALLOCATIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roomAllocation = await _context.RoomAllocation
            .FirstOrDefaultAsync(m => m.RoomAllocationId == id);

        if (roomAllocation == null)
        {
            return NotFound();
        }

        return View(roomAllocation);
    }

    // GET: ROOMALLOCATIONS/Create
    public async Task<IActionResult> Create()
    {
        await LoadDropdownsAsync();
        return View();
    }

    // POST: ROOMALLOCATIONS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("RoomAllocationId,AccommodationApplicationId,StudentId,RoomId,StartDate,EndDate,Status")]
        RoomAllocation roomAllocation)
    {
        await ValidateForeignKeysAsync(roomAllocation);

        if (ModelState.IsValid)
        {
            _context.Add(roomAllocation);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Room allocation created successfully!";
            return RedirectToAction(nameof(Index));
        }

        await LoadDropdownsAsync(
            roomAllocation.AccommodationApplicationId,
            roomAllocation.StudentId,
            roomAllocation.RoomId);

        TempData["ErrorMessage"] = "Could not create the room allocation. Please check the form errors.";
        return View(roomAllocation);
    }

    // GET: ROOMALLOCATIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roomAllocation = await _context.RoomAllocation.FindAsync(id);

        if (roomAllocation == null)
        {
            return NotFound();
        }

        await LoadDropdownsAsync(
            roomAllocation.AccommodationApplicationId,
            roomAllocation.StudentId,
            roomAllocation.RoomId);

        return View(roomAllocation);
    }

    // POST: ROOMALLOCATIONS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("RoomAllocationId,AccommodationApplicationId,StudentId,RoomId,StartDate,EndDate,Status")]
        RoomAllocation roomAllocation)
    {
        if (id != roomAllocation.RoomAllocationId)
        {
            return NotFound();
        }

        await ValidateForeignKeysAsync(roomAllocation);

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(roomAllocation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Room allocation updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomAllocationExists(roomAllocation.RoomAllocationId))
                {
                    return NotFound();
                }

                throw;
            }
        }

        await LoadDropdownsAsync(
            roomAllocation.AccommodationApplicationId,
            roomAllocation.StudentId,
            roomAllocation.RoomId);

        TempData["ErrorMessage"] = "Could not update the room allocation. Please check the form errors.";
        return View(roomAllocation);
    }

    // GET: ROOMALLOCATIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roomAllocation = await _context.RoomAllocation
            .FirstOrDefaultAsync(m => m.RoomAllocationId == id);

        if (roomAllocation == null)
        {
            return NotFound();
        }

        return View(roomAllocation);
    }

    // POST: ROOMALLOCATIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        try
        {
            var roomAllocation = await _context.RoomAllocation.FindAsync(id);

            if (roomAllocation != null)
            {
                _context.RoomAllocation.Remove(roomAllocation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Room allocation deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Room allocation not found.";
            }
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Cannot delete this room allocation because it has associated records.";
        }

        return RedirectToAction(nameof(Index));
    }

    // Validates the selected foreign keys
    private async Task ValidateForeignKeysAsync(RoomAllocation roomAllocation)
    {
        if (roomAllocation.AccommodationApplicationId.HasValue)
        {
            bool applicationExists = await _context.AccommodationApplication
                .AnyAsync(a => a.AccommodationApplicationId == roomAllocation.AccommodationApplicationId);

            if (!applicationExists)
            {
                ModelState.AddModelError(
                    nameof(roomAllocation.AccommodationApplicationId),
                    "The selected application does not exist.");
            }
        }

        if (roomAllocation.StudentId.HasValue)
        {
            bool studentExists = await _context.Student
                .AnyAsync(s => s.StudentId == roomAllocation.StudentId);

            if (!studentExists)
            {
                ModelState.AddModelError(
                    nameof(roomAllocation.StudentId),
                    "The selected student does not exist.");
            }
        }

        if (roomAllocation.RoomId.HasValue)
        {
            bool roomExists = await _context.Room
                .AnyAsync(r => r.RoomId == roomAllocation.RoomId);

            if (!roomExists)
            {
                ModelState.AddModelError(
                    nameof(roomAllocation.RoomId),
                    "The selected room does not exist.");
            }
        }
    }

    private bool RoomAllocationExists(int id)
    {
        return _context.RoomAllocation.Any(e => e.RoomAllocationId == id);
    }
}
