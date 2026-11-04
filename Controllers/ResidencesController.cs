using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

namespace UnivManagement.Controllers;

public class ResidencesController : Controller
{
    private readonly UniversityDbContext _context;

    public ResidencesController(UniversityDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Residence.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var residence = await _context.Residence
            .FirstOrDefaultAsync(m => m.ResidenceId == id);
        if (residence == null)
        {
            return NotFound();
        }

        return View(residence);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ResidenceId,Name,Address,Capacity,IsActive")] Residence residence)
    {
        if (ModelState.IsValid)
        {
            _context.Add(residence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(residence);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var residence = await _context.Residence.FindAsync(id);
        if (residence == null)
        {
            return NotFound();
        }
        return View(residence);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ResidenceId,Name,Address,Capacity,IsActive")] Residence residence)
    {
        if (id != residence.ResidenceId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(residence);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResidenceExists(residence.ResidenceId))
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
        return View(residence);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var residence = await _context.Residence
            .FirstOrDefaultAsync(m => m.ResidenceId == id);
        if (residence == null)
        {
            return NotFound();
        }

        return View(residence);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var residence = await _context.Residence.FindAsync(id);
        if (residence != null)
        {
            _context.Residence.Remove(residence);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ResidenceExists(int id)
    {
        return _context.Residence.Any(e => e.ResidenceId == id);
    }
}