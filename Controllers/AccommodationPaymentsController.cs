
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models.AccommodationManagement;
using UnivManagement.Data;

public class AccommodationPaymentsController : Controller
{
    private readonly UniversityDbContext _context;

    public AccommodationPaymentsController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ACCOMMODATIONPAYMENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.AccommodationPayment.ToListAsync());
    }

    // GET: ACCOMMODATIONPAYMENTS/Details/5
    public async Task<IActionResult> Details(int? accommodationpaymentid)
    {
        if (accommodationpaymentid == null)
        {
            return NotFound();
        }

        var accommodationpayment = await _context.AccommodationPayment
            .FirstOrDefaultAsync(m => m.AccommodationPaymentId == accommodationpaymentid);
        if (accommodationpayment == null)
        {
            return NotFound();
        }

        return View(accommodationpayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ACCOMMODATIONPAYMENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AccommodationPaymentId,AccommodationContractId,DueDate,Amount,PaidAmount,PaidAt,Status")] AccommodationPayment accommodationpayment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(accommodationpayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(accommodationpayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Edit/5
    public async Task<IActionResult> Edit(int? accommodationpaymentid)
    {
        if (accommodationpaymentid == null)
        {
            return NotFound();
        }

        var accommodationpayment = await _context.AccommodationPayment.FindAsync(accommodationpaymentid);
        if (accommodationpayment == null)
        {
            return NotFound();
        }
        return View(accommodationpayment);
    }

    // POST: ACCOMMODATIONPAYMENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? accommodationpaymentid, [Bind("AccommodationPaymentId,AccommodationContractId,DueDate,Amount,PaidAmount,PaidAt,Status")] AccommodationPayment accommodationpayment)
    {
        if (accommodationpaymentid != accommodationpayment.AccommodationPaymentId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(accommodationpayment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationPaymentExists(accommodationpayment.AccommodationPaymentId))
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
        return View(accommodationpayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Delete/5
    public async Task<IActionResult> Delete(int? accommodationpaymentid)
    {
        if (accommodationpaymentid == null)
        {
            return NotFound();
        }

        var accommodationpayment = await _context.AccommodationPayment
            .FirstOrDefaultAsync(m => m.AccommodationPaymentId == accommodationpaymentid);
        if (accommodationpayment == null)
        {
            return NotFound();
        }

        return View(accommodationpayment);
    }

    // POST: ACCOMMODATIONPAYMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? accommodationpaymentid)
    {
        var accommodationpayment = await _context.AccommodationPayment.FindAsync(accommodationpaymentid);
        if (accommodationpayment != null)
        {
            _context.AccommodationPayment.Remove(accommodationpayment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AccommodationPaymentExists(int? accommodationpaymentid)
    {
        return _context.AccommodationPayment.Any(e => e.AccommodationPaymentId == accommodationpaymentid);
    }
}
