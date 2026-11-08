using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class ServiceRequestTypesController : Controller
    {
        private readonly UniversityDbContext _context;

        public ServiceRequestTypesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRequestTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceRequestTypes.ToListAsync());
        }

        // GET: ServiceRequestTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequestTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] ServiceRequestType serviceRequestType)
        {
            if (await _context.ServiceRequestTypes.AnyAsync(t => t.Name == serviceRequestType.Name))
            {
                ModelState.AddModelError(nameof(ServiceRequestType.Name), "There is already a request type with this name.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(serviceRequestType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequestType);
        }

        // GET: ServiceRequestTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequestType = await _context.ServiceRequestTypes.FirstOrDefaultAsync(t => t.ServiceRequestTypeId == id);

            if (serviceRequestType == null)
            {
                return NotFound();
            }

            return View(serviceRequestType);
        }
    }
}