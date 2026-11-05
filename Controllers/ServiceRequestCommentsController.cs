using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers {
    // Comments are added and deleted from the details page of a request
    public class ServiceRequestCommentsController : Controller {
        private readonly UniversityDbContext _context;

        public ServiceRequestCommentsController(UniversityDbContext context) {
            _context = context;
        }

        // POST: ServiceRequestComments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestId,Author,Text")] ServiceRequestComment comment) {
            var serviceRequest = await _context.ServiceRequests.FindAsync(comment.ServiceRequestId);
            if (serviceRequest == null) {
                return NotFound();
            }

            if (!ModelState.IsValid) {
                TempData["Error"] = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return RedirectToDetails(comment.ServiceRequestId);
            }

            comment.Author = comment.Author.Trim();
            comment.Text = comment.Text.Trim();
            comment.CreatedAt = DateTime.Now;
            _context.ServiceRequestComments.Add(comment);

            serviceRequest.History.Add(new ServiceRequestHistory { Action = $"Comment added by {comment.Author}" });
            serviceRequest.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Message"] = "The comment was added.";
            return RedirectToDetails(comment.ServiceRequestId);
        }

        // POST: ServiceRequestComments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            var comment = await _context.ServiceRequestComments
                .Include(c => c.ServiceRequest)
                .FirstOrDefaultAsync(c => c.ServiceRequestCommentId == id);
            if (comment == null) {
                return NotFound();
            }

            comment.ServiceRequest!.History.Add(new ServiceRequestHistory { Action = $"Comment by {comment.Author} deleted" });
            _context.ServiceRequestComments.Remove(comment);
            await _context.SaveChangesAsync();

            TempData["Message"] = "The comment was deleted.";
            return RedirectToDetails(comment.ServiceRequestId);
        }

        private IActionResult RedirectToDetails(int serviceRequestId) {
            return RedirectToAction("Details", "ServiceRequests", new { id = serviceRequestId }, "comments");
        }
    }
}
