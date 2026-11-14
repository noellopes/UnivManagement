using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnivManagement.Models;
using UnivManagement.Data;

public class ServiceRequestCatalogItemsController : Controller
{
    private readonly UniversityDbContext _context;

    public ServiceRequestCatalogItemsController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: SERVICEREQUESTCATALOGITEMS
    public async Task<IActionResult> Index(string? successMessage = null)
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }
        return View(await _context.ServiceRequestCatalogItems.Include(c => c.Type).ToListAsync());
    }

    // GET: SERVICEREQUESTCATALOGITEMS/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequestcatalogitem = await _context.ServiceRequestCatalogItems
            .Include(c => c.Type)
            .FirstOrDefaultAsync(m => m.ServiceRequestCatalogItemId == id);
        if (servicerequestcatalogitem == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(servicerequestcatalogitem);
    }

    // GET: SERVICEREQUESTCATALOGITEMS/Create
    public IActionResult Create()
    {
        ViewData["ServiceRequestTypeId"] = new SelectList(_context.ServiceRequestTypes, "ServiceRequestTypeId", "Name");
        return View();
    }

    // POST: SERVICEREQUESTCATALOGITEMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ServiceRequestCatalogItemId,Name,Description,ServiceRequestTypeId,ProcessingDays,RequiredDocuments,IsActive")] ServiceRequestCatalogItem servicerequestcatalogitem)
    {
        // Two services cannot have the same name
        if (_context.ServiceRequestCatalogItems.Any(c => c.Name == servicerequestcatalogitem.Name))
        {
            ModelState.AddModelError("Name", "There is already a service with this name.");
        }

        // The selected type has to exist (0 already has its own message)
        if (servicerequestcatalogitem.ServiceRequestTypeId > 0
            && !_context.ServiceRequestTypes.Any(t => t.ServiceRequestTypeId == servicerequestcatalogitem.ServiceRequestTypeId))
        {
            ModelState.AddModelError("ServiceRequestTypeId", "The selected type does not exist.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(servicerequestcatalogitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details),
                new
                {
                    id = servicerequestcatalogitem.ServiceRequestCatalogItemId,
                    successMessage = "Service created successfully."
                });
        }

        ViewData["ServiceRequestTypeId"] = new SelectList(_context.ServiceRequestTypes,
            "ServiceRequestTypeId", "Name", servicerequestcatalogitem.ServiceRequestTypeId);
        return View(servicerequestcatalogitem);
    }

    // GET: SERVICEREQUESTCATALOGITEMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequestcatalogitem = await _context.ServiceRequestCatalogItems.FindAsync(id);
        if (servicerequestcatalogitem == null)
        {
            return View("NotFound");
        }

        ViewData["ServiceRequestTypeId"] = new SelectList(_context.ServiceRequestTypes, "ServiceRequestTypeId", "Name", servicerequestcatalogitem.ServiceRequestTypeId);
        return View(servicerequestcatalogitem);
    }

    // POST: SERVICEREQUESTCATALOGITEMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ServiceRequestCatalogItemId,Name,Description,ServiceRequestTypeId,ProcessingDays,RequiredDocuments,IsActive")] ServiceRequestCatalogItem servicerequestcatalogitem)
    {
        if (id != servicerequestcatalogitem.ServiceRequestCatalogItemId)
        {
            return NotFound();
        }

        // Two services cannot have the same name (the one being edited does not count)
        if (_context.ServiceRequestCatalogItems.Any(c => c.Name == servicerequestcatalogitem.Name
            && c.ServiceRequestCatalogItemId != servicerequestcatalogitem.ServiceRequestCatalogItemId))
        {
            ModelState.AddModelError("Name", "There is already a service with this name.");
        }

        // The selected type has to exist (0 already has its own message)
        if (servicerequestcatalogitem.ServiceRequestTypeId > 0
            && !_context.ServiceRequestTypes.Any(t => t.ServiceRequestTypeId == servicerequestcatalogitem.ServiceRequestTypeId))
        {
            ModelState.AddModelError("ServiceRequestTypeId", "The selected type does not exist.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(servicerequestcatalogitem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceRequestCatalogItemExists(servicerequestcatalogitem.ServiceRequestCatalogItemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Details),
                new
                {
                    id = servicerequestcatalogitem.ServiceRequestCatalogItemId,
                    successMessage = "Service updated successfully."
                });
        }

        ViewData["ServiceRequestTypeId"] = new SelectList(_context.ServiceRequestTypes, "ServiceRequestTypeId", "Name", servicerequestcatalogitem.ServiceRequestTypeId);
        return View(servicerequestcatalogitem);
    }

    // GET: SERVICEREQUESTCATALOGITEMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequestcatalogitem = await _context.ServiceRequestCatalogItems
            .Include(m => m.Type)
            .FirstOrDefaultAsync(m => m.ServiceRequestCatalogItemId == id);
        if (servicerequestcatalogitem == null)
        {
            return RedirectToAction(nameof(Index),
                new { successMessage = "Success: The service was already deleted by another user." });
        }

        return View(servicerequestcatalogitem);
    }

    // POST: SERVICEREQUESTCATALOGITEMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var servicerequestcatalogitem = await _context.ServiceRequestCatalogItems.FindAsync(id);
        if (servicerequestcatalogitem != null)
        {
            _context.ServiceRequestCatalogItems.Remove(servicerequestcatalogitem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index),
            new { successMessage = "Service deleted successfully." });
    }

    private bool ServiceRequestCatalogItemExists(int? id)
    {
        return _context.ServiceRequestCatalogItems.Any(e => e.ServiceRequestCatalogItemId == id);
    }
}