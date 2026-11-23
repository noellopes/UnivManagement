using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models.AccommodationManagement;

namespace UnivManagement.Controllers
{
    public class AccommodationContractsController : Controller
    {
        private readonly UniversityDbContext _context;

        public AccommodationContractsController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: AccommodationContracts
        public async Task<IActionResult> Index()
        {
            var contracts = _context.AccommodationContract
                .Include(c => c.Status);

            return View(await contracts.ToListAsync());
        }

        // GET: AccommodationContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var accommodationContract = await _context.AccommodationContract
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.AccommodationContractId == id);

            if (accommodationContract == null) return NotFound();

            return View(accommodationContract);
        }

        // GET: AccommodationContracts/Create
        public IActionResult Create()
        {
            ViewData["ContractStatusId"] = new SelectList(_context.ContractStatus, "ContractStatusId", "Name");
            return View();
        }

        // POST: AccommodationContracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccommodationContractId,RoomAllocationId,StartDate,EndDate,MonthlyRent,Deposit,ContractStatusId,SignedDocumentId")] AccommodationContract accommodationContract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accommodationContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ContractStatusId"] = new SelectList(_context.ContractStatus, "ContractStatusId", "Name", accommodationContract.ContractStatusId);
            return View(accommodationContract);
        }

        // GET: AccommodationContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var accommodationContract = await _context.AccommodationContract.FindAsync(id);
            if (accommodationContract == null) return NotFound();

            ViewData["ContractStatusId"] = new SelectList(_context.ContractStatus, "ContractStatusId", "Name", accommodationContract.ContractStatusId);
            return View(accommodationContract);
        }

        // POST: AccommodationContracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccommodationContractId,RoomAllocationId,StartDate,EndDate,MonthlyRent,Deposit,ContractStatusId,SignedDocumentId")] AccommodationContract accommodationContract)
        {
            if (id != accommodationContract.AccommodationContractId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accommodationContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccommodationContractExists(accommodationContract.AccommodationContractId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ContractStatusId"] = new SelectList(_context.ContractStatus, "ContractStatusId", "Name", accommodationContract.ContractStatusId);
            return View(accommodationContract);
        }

        // GET: AccommodationContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var accommodationContract = await _context.AccommodationContract
                .Include(c => c.Status)
                .FirstOrDefaultAsync(m => m.AccommodationContractId == id);

            if (accommodationContract == null) return NotFound();

            return View(accommodationContract);
        }

        // POST: AccommodationContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accommodationContract = await _context.AccommodationContract.FindAsync(id);
            if (accommodationContract != null)
            {
                _context.AccommodationContract.Remove(accommodationContract);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationContractExists(int id)
        {
            return _context.AccommodationContract.Any(e => e.AccommodationContractId == id);
        }
    }
}