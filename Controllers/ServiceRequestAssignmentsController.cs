using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    // Who is in charge of each request
    public class ServiceRequestAssignmentsController : Controller
    {
        private readonly UniversityDbContext _context;

        public ServiceRequestAssignmentsController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestAssignments
        public async Task<IActionResult> Index(string? successMessage = null)
        {
            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            var assignments = _context.ServiceRequestAssignments.Include(a => a.ServiceRequest);
            return View(await assignments.ToListAsync());
        }

        // GET: ServiceRequestAssignments/Details/5
        public async Task<IActionResult> Details(int? id, string? successMessage = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestAssignment = await _context.ServiceRequestAssignments
                .Include(a => a.ServiceRequest)
                .FirstOrDefaultAsync(m => m.ServiceRequestAssignmentId == id);
            if (serviceRequestAssignment == null)
            {
                return View("NotFound");
            }

            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            return View(serviceRequestAssignment);
        }

        // GET: ServiceRequestAssignments/Create
        public IActionResult Create()
        {
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title");
            return View();
        }

        // POST: ServiceRequestAssignments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceRequestAssignmentId,ServiceRequestId,FuncionarioId,DepartamentoId,AssignedAt,EndedAt,Note")] ServiceRequestAssignment serviceRequestAssignment)
        {
            // An assignment cannot end before it starts
            if (serviceRequestAssignment.EndedAt < serviceRequestAssignment.AssignedAt)
            {
                ViewBag.ErrorMessage = "The end date cannot be before the assignment date.";
            }
            else if (ModelState.IsValid)
            {
                _context.Add(serviceRequestAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details),
                    new { id = serviceRequestAssignment.ServiceRequestAssignmentId, successMessage = "Assignment created successfully." });
            }
            // if the form has errors, the list has to be filled again
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestAssignment.ServiceRequestId);
            return View(serviceRequestAssignment);
        }

        // GET: ServiceRequestAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestAssignment = await _context.ServiceRequestAssignments.FindAsync(id);
            if (serviceRequestAssignment == null)
            {
                return View("NotFound");
            }

            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestAssignment.ServiceRequestId);
            return View(serviceRequestAssignment);
        }

        // POST: ServiceRequestAssignments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ServiceRequestAssignmentId,ServiceRequestId,FuncionarioId,DepartamentoId,AssignedAt,EndedAt,Note")] ServiceRequestAssignment serviceRequestAssignment)
        {
            if (id != serviceRequestAssignment.ServiceRequestAssignmentId)
            {
                return NotFound();
            }

            // An assignment cannot end before it starts
            if (serviceRequestAssignment.EndedAt < serviceRequestAssignment.AssignedAt)
            {
                ViewBag.ErrorMessage = "The end date cannot be before the assignment date.";
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequestAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestAssignmentExists(serviceRequestAssignment.ServiceRequestAssignmentId))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details),
                    new { id = serviceRequestAssignment.ServiceRequestAssignmentId, successMessage = "Assignment updated successfully." });
            }
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestAssignment.ServiceRequestId);
            return View(serviceRequestAssignment);
        }

        // GET: ServiceRequestAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestAssignment = await _context.ServiceRequestAssignments
                .Include(a => a.ServiceRequest)
                .FirstOrDefaultAsync(m => m.ServiceRequestAssignmentId == id);
            if (serviceRequestAssignment == null)
            {
                return RedirectToAction(nameof(Index),
                    new { successMessage = "The assignment was already deleted by another user." });
            }

            return View(serviceRequestAssignment);
        }

        // POST: ServiceRequestAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequestAssignment = await _context.ServiceRequestAssignments.FindAsync(id);
            if (serviceRequestAssignment != null)
            {
                _context.ServiceRequestAssignments.Remove(serviceRequestAssignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { successMessage = "Assignment deleted successfully." });
        }

        private bool ServiceRequestAssignmentExists(int id)
        {
            return _context.ServiceRequestAssignments.Any(e => e.ServiceRequestAssignmentId == id);
        }
    }
}
