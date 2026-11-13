
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ServiceRequestCatalogItems.ToListAsync());
    }

    // GET: SERVICEREQUESTCATALOGITEMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequestcatalogitem = await _context.ServiceRequestCatalogItems
            .FirstOrDefaultAsync(m => m.ServiceRequestCatalogItemId == id);
        if (servicerequestcatalogitem == null)
        {
            return NotFound();
        }

        return View(servicerequestcatalogitem);
    }

    // GET: SERVICEREQUESTCATALOGITEMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SERVICEREQUESTCATALOGITEMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ServiceRequestCatalogItemId,Name,Description,ServiceRequestTypeId,Type,ProcessingDays,RequiredDocuments,IsActive")] ServiceRequestCatalogItem servicerequestcatalogitem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(servicerequestcatalogitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
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
            return NotFound();
        }
        return View(servicerequestcatalogitem);
    }

    // POST: SERVICEREQUESTCATALOGITEMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ServiceRequestCatalogItemId,Name,Description,ServiceRequestTypeId,Type,ProcessingDays,RequiredDocuments,IsActive")] ServiceRequestCatalogItem servicerequestcatalogitem)
    {
        if (id != servicerequestcatalogitem.ServiceRequestCatalogItemId)
        {
            return NotFound();
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
            return RedirectToAction(nameof(Index));
        }
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
            .FirstOrDefaultAsync(m => m.ServiceRequestCatalogItemId == id);
        if (servicerequestcatalogitem == null)
        {
            return NotFound();
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
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ServiceRequestCatalogItemExists(int? id)
    {
        return _context.ServiceRequestCatalogItems.Any(e => e.ServiceRequestCatalogItemId == id);
    }
}
