using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class StayRecordsController : Controller
    {
        private readonly UniversityDbContext _context;

        public StayRecordsController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: StayRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.StayRecord.ToListAsync());
        }

        // GET: StayRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stayRecord = await _context.StayRecord
                .FirstOrDefaultAsync(m => m.StayRecordId == id);
            if (stayRecord == null)
            {
                return NotFound();
            }

            return View(stayRecord);
        }

        // GET: StayRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StayRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StayRecordId,RoomAllocationId,Type,OccurredAt,ConditionNotes,RecordedBy")] StayRecord stayRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stayRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stayRecord);
        }

        // GET: StayRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stayRecord = await _context.StayRecord.FindAsync(id);
            if (stayRecord == null)
            {
                return NotFound();
            }
            return View(stayRecord);
        }

        // POST: StayRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("StayRecordId,RoomAllocationId,Type,OccurredAt,ConditionNotes,RecordedBy")] StayRecord stayRecord)
        {
            if (id != stayRecord.StayRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stayRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StayRecordExists(stayRecord.StayRecordId))
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
            return View(stayRecord);
        }

        // GET: StayRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stayRecord = await _context.StayRecord
                .FirstOrDefaultAsync(m => m.StayRecordId == id);
            if (stayRecord == null)
            {
                return NotFound();
            }

            return View(stayRecord);
        }

        // POST: StayRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var stayRecord = await _context.StayRecord.FindAsync(id);
            if (stayRecord != null)
            {
                _context.StayRecord.Remove(stayRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StayRecordExists(int? id)
        {
            return _context.StayRecord.Any(e => e.StayRecordId == id);
        }
    }
}