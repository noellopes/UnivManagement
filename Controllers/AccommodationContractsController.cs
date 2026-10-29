using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class AccommodationContractsController : Controller
    {
        private readonly UniversityDbContext _context;

        public AccommodationContractsController(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.AccommodationContract.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var accommodationcontract = await _context.AccommodationContract
                .FirstOrDefaultAsync(m => m.AccommodationContractId == id);
            if (accommodationcontract == null) return NotFound();

            return View(accommodationcontract);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccommodationContractId,StudentId,RoomAllocationId,SignatureDate,StartDate,EndDate,Status")] AccommodationContract accommodationcontract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accommodationcontract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accommodationcontract);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var accommodationcontract = await _context.AccommodationContract.FindAsync(id);
            if (accommodationcontract == null) return NotFound();

            return View(accommodationcontract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccommodationContractId,StudentId,RoomAllocationId,SignatureDate,StartDate,EndDate,Status")] AccommodationContract accommodationcontract)
        {
            if (id != accommodationcontract.AccommodationContractId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accommodationcontract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccommodationContractExists(accommodationcontract.AccommodationContractId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(accommodationcontract);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var accommodationcontract = await _context.AccommodationContract
                .FirstOrDefaultAsync(m => m.AccommodationContractId == id);
            if (accommodationcontract == null) return NotFound();

            return View(accommodationcontract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accommodationcontract = await _context.AccommodationContract.FindAsync(id);
            if (accommodationcontract != null)
            {
                _context.AccommodationContract.Remove(accommodationcontract);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationContractExists(int id)
        {
            return _context.AccommodationContract.Any(e => e.AccommodationContractId == id);
        }
    }
}