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
        public async Task<IActionResult> Index()
        {
            var assignments = _context.ServiceRequestAssignments.Include(a => a.ServiceRequest);
            return View(await assignments.ToListAsync());
        }

        // GET: ServiceRequestAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _context.ServiceRequestAssignments
                .Include(a => a.ServiceRequest)
                .FirstOrDefaultAsync(m => m.ServiceRequestAssignmentId == id));
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
                ModelState.AddModelError("EndedAt", "The end date cannot be before the assignment date.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // if the form has errors, the list has to be filled again
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestAssignment.ServiceRequestId);
            return View(serviceRequestAssignment);
        }

        // GET: ServiceRequestAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var serviceRequestAssignment = await _context.ServiceRequestAssignments.FindAsync(id);
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestAssignment?.ServiceRequestId);
            return View(serviceRequestAssignment);
        }

        // POST: ServiceRequestAssignments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ServiceRequestAssignmentId,ServiceRequestId,FuncionarioId,DepartamentoId,AssignedAt,EndedAt,Note")] ServiceRequestAssignment serviceRequestAssignment)
        {
            // An assignment cannot end before it starts
            if (serviceRequestAssignment.EndedAt < serviceRequestAssignment.AssignedAt)
            {
                ModelState.AddModelError("EndedAt", "The end date cannot be before the assignment date.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(serviceRequestAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceRequestId"] = new SelectList(_context.ServiceRequests, "ServiceRequestId", "Title", serviceRequestAssignment.ServiceRequestId);
            return View(serviceRequestAssignment);
        }

        // GET: ServiceRequestAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _context.ServiceRequestAssignments
                .Include(a => a.ServiceRequest)
                .FirstOrDefaultAsync(m => m.ServiceRequestAssignmentId == id));
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
            return RedirectToAction(nameof(Index));
        }
    }
}
