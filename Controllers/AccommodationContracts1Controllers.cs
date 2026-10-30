using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

namespace UnivManagement.Controllers // Adicionado o namespace correto
{
    public class AccommodationContractsControllers : Controller // Corrigido o nome
    {
        private readonly UniversityDbContext _context;

        public AccommodationContractsControllers(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: AccommodationContracts
        public async Task<IActionResult> Index()
        {
            return View(await _context.AccommodationContract.ToListAsync());
        }

        // GET: AccommodationContracts/Details/5
        public async Task<IActionResult> Details(int? id) // Corrigido de accommodationcontractid para id
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodationcontract = await _context.AccommodationContract
                .FirstOrDefaultAsync(m => m.AccommodationContractId == id); // Corrigido
            if (accommodationcontract == null)
            {
                return NotFound();
            }

            return View(accommodationcontract);
        }

        // GET: AccommodationContracts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccommodationContracts/Create
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

        // GET: AccommodationContracts/Edit/5
        public async Task<IActionResult> Edit(int? id) // Corrigido de accommodationcontractid para id
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodationcontract = await _context.AccommodationContract.FindAsync(id); // Corrigido
            if (accommodationcontract == null)
            {
                return NotFound();
            }
            return View(accommodationcontract);
        }

        // POST: AccommodationContracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccommodationContractId,StudentId,RoomAllocationId,SignatureDate,StartDate,EndDate,Status")] AccommodationContract accommodationcontract) // Corrigido de int? accommodationcontractid para int id (no POST convém não ser nullable)
        {
            if (id != accommodationcontract.AccommodationContractId) // Corrigido
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accommodationcontract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccommodationContractExists(accommodationcontract.AccommodationContractId))
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
            return View(accommodationcontract);
        }

        // GET: AccommodationContracts/Delete/5
        public async Task<IActionResult> Delete(int? id) // Corrigido de accommodationcontractid para id
        {
            if (id == null)
            {
                return NotFound();
            }

            var accommodationcontract = await _context.AccommodationContract
                .FirstOrDefaultAsync(m => m.AccommodationContractId == id); // Corrigido
            if (accommodationcontract == null)
            {
                return NotFound();
            }

            return View(accommodationcontract);
        }

        // POST: AccommodationContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Corrigido de int? accommodationcontractid para int id
        {
            var accommodationcontract = await _context.AccommodationContract.FindAsync(id); // Corrigido
            if (accommodationcontract != null)
            {
                _context.AccommodationContract.Remove(accommodationcontract);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationContractExists(int id) // Corrigido
        {
            return _context.AccommodationContract.Any(e => e.AccommodationContractId == id); // Corrigido
        }
    }
}
