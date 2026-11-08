using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    // Comments written about the requests
    public class ServiceRequestCommentsController : Controller
    {
        private readonly UniversityDbContext _context;

        public ServiceRequestCommentsController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestComments
        public async Task<IActionResult> Index()
        {
            var comments = _context.ServiceRequestComments.Include(c => c.ServiceRequest);
            return View(await comments.ToListAsync());
        }

        // GET: ServiceRequestComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _context.ServiceRequestComments
                .Include(c => c.ServiceRequest)
                .FirstOrDefaultAsync(m => m.ServiceRequestCommentId == id));
        }

        // GET: ServiceRequestComments/Create
        public IActionResult Create()
        {
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title");
            return View();
        }

        // POST: ServiceRequestComments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestCommentId,ServiceRequestId,Author,Text")] ServiceRequestComment serviceRequestComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // if the form has errors, the list has to be filled again
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestComment.ServiceRequestId);
            return View(serviceRequestComment);
        }

        // GET: ServiceRequestComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var serviceRequestComment = await _context.ServiceRequestComments.FindAsync(id);
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestComment?.ServiceRequestId);
            return View(serviceRequestComment);
        }

        // POST: ServiceRequestComments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ServiceRequestCommentId,ServiceRequestId,Author,Text,CreatedAt")] ServiceRequestComment serviceRequestComment)
        {
            if (ModelState.IsValid)
            {
                _context.Update(serviceRequestComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestComment.ServiceRequestId);
            return View(serviceRequestComment);
        }

        // GET: ServiceRequestComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _context.ServiceRequestComments
                .Include(c => c.ServiceRequest)
                .FirstOrDefaultAsync(m => m.ServiceRequestCommentId == id));
        }

        // POST: ServiceRequestComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequestComment = await _context.ServiceRequestComments.FindAsync(id);
            if (serviceRequestComment != null)
            {
                _context.ServiceRequestComments.Remove(serviceRequestComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
