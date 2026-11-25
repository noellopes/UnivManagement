using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnivManagement.Models;
using UnivManagement.Data;

public class ServiceRequestAttachmentsController : Controller
{
    private readonly UniversityDbContext _context;

    public ServiceRequestAttachmentsController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: SERVICEREQUESTATTACHMENTS
    public async Task<IActionResult> Index(string? successMessage = null)
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }
        return View(await _context.ServiceRequestAttachments
            .Include(a => a.ServiceRequest)
            .OrderByDescending(a => a.UploadedAt)
            .ToListAsync());
    }

    // GET: SERVICEREQUESTATTACHMENTS/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequestattachment = await _context.ServiceRequestAttachments
            .Include(a => a.ServiceRequest)
            .FirstOrDefaultAsync(m => m.ServiceRequestAttachmentId == id);
        if (servicerequestattachment == null)
        {
            return View("NotFound");
        }

        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(servicerequestattachment);
    }

    // GET: SERVICEREQUESTATTACHMENTS/Create
    public IActionResult Create()
    {
        ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title");
        return View();
    }

    // POST: SERVICEREQUESTATTACHMENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ServiceRequestAttachmentId,ServiceRequestId,FileName,SizeInKb,Description")] ServiceRequestAttachment servicerequestattachment)
    {
        // The selected request has to exist (0 already has its own message)
        if (servicerequestattachment.ServiceRequestId > 0
            && !_context.ServiceRequests.Any(r => r.ServiceRequestId == servicerequestattachment.ServiceRequestId))
        {
            ModelState.AddModelError("ServiceRequestId", "The selected request does not exist.");
        }

        // A request cannot have two attachments with the same file name
        if (_context.ServiceRequestAttachments.Any(a => a.ServiceRequestId == servicerequestattachment.ServiceRequestId
            && a.FileName == servicerequestattachment.FileName))
        {
            ModelState.AddModelError("FileName", "This request already has an attachment with this file name.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(servicerequestattachment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details),
                new
                {
                    id = servicerequestattachment.ServiceRequestAttachmentId,
                    successMessage = "Attachment created successfully."
                });
        }

        ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests,
            "ServiceRequestId", "Title", servicerequestattachment.ServiceRequestId);
        return View(servicerequestattachment);
    }

    // GET: SERVICEREQUESTATTACHMENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequestattachment = await _context.ServiceRequestAttachments.FindAsync(id);
        if (servicerequestattachment == null)
        {
            return View("NotFound");
        }

        ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", servicerequestattachment.ServiceRequestId);
        return View(servicerequestattachment);
    }

    // POST: SERVICEREQUESTATTACHMENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ServiceRequestAttachmentId,ServiceRequestId,FileName,SizeInKb,Description")] ServiceRequestAttachment servicerequestattachment)
    {
        if (id != servicerequestattachment.ServiceRequestAttachmentId)
        {
            return NotFound();
        }

        // The selected request has to exist (0 already has its own message)
        if (servicerequestattachment.ServiceRequestId > 0
            && !_context.ServiceRequests.Any(r => r.ServiceRequestId == servicerequestattachment.ServiceRequestId))
        {
            ModelState.AddModelError("ServiceRequestId", "The selected request does not exist.");
        }

        // A request cannot have two attachments with the same file name (the one being edited does not count)
        if (_context.ServiceRequestAttachments.Any(a => a.ServiceRequestId == servicerequestattachment.ServiceRequestId
            && a.FileName == servicerequestattachment.FileName
            && a.ServiceRequestAttachmentId != servicerequestattachment.ServiceRequestAttachmentId))
        {
            ModelState.AddModelError("FileName", "This request already has an attachment with this file name.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(servicerequestattachment);
                // The upload date is not in the form: keep the one that is already saved
                _context.Entry(servicerequestattachment).Property(a => a.UploadedAt).IsModified = false;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceRequestAttachmentExists(servicerequestattachment.ServiceRequestAttachmentId))
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
                    id = servicerequestattachment.ServiceRequestAttachmentId,
                    successMessage = "Attachment updated successfully."
                });
        }

        ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", servicerequestattachment.ServiceRequestId);
        return View(servicerequestattachment);
    }

    // GET: SERVICEREQUESTATTACHMENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicerequestattachment = await _context.ServiceRequestAttachments
            .Include(a => a.ServiceRequest)
            .FirstOrDefaultAsync(m => m.ServiceRequestAttachmentId == id);
        if (servicerequestattachment == null)
        {
            return RedirectToAction(nameof(Index),
                new { successMessage = "Success: The attachment was already deleted by another user." });
        }

        return View(servicerequestattachment);
    }

    // POST: SERVICEREQUESTATTACHMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var servicerequestattachment = await _context.ServiceRequestAttachments.FindAsync(id);
        if (servicerequestattachment != null)
        {
            _context.ServiceRequestAttachments.Remove(servicerequestattachment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index),
            new { successMessage = "Attachment deleted successfully." });
    }

    private bool ServiceRequestAttachmentExists(int? id)
    {
        return _context.ServiceRequestAttachments.Any(e => e.ServiceRequestAttachmentId == id);
    }
}
