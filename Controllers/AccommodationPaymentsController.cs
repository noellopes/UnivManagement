
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
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationpayment = await _context.AccommodationPayment
            .FirstOrDefaultAsync(m => m.AccommodationPaymentId == id);
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

            TempData["SuccessMessage"] = "Payment created successfully!";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = "Could not create the payment. Please check the form errors.";
        return View(accommodationpayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationpayment = await _context.AccommodationPayment.FindAsync(id);
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
    public async Task<IActionResult> Edit(int? id, [Bind("AccommodationPaymentId,AccommodationContractId,DueDate,Amount,PaidAmount,PaidAt,Status")] AccommodationPayment accommodationpayment)
    {
        if (id != accommodationpayment.AccommodationPaymentId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(accommodationpayment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Payment updated successfully!";
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

        TempData["ErrorMessage"] = "Could not update the payment. Please check the form errors.";
        return View(accommodationpayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationpayment = await _context.AccommodationPayment
            .FirstOrDefaultAsync(m => m.AccommodationPaymentId == id);
        if (accommodationpayment == null)
        {
            return NotFound();
        }

        return View(accommodationpayment);
    }

    // POST: ACCOMMODATIONPAYMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        try
        {
            var accommodationpayment = await _context.AccommodationPayment.FindAsync(id);
            if (accommodationpayment != null)
            {
                _context.AccommodationPayment.Remove(accommodationpayment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Payment deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Payment not found.";
            }
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Cannot delete this payment because it has associated records.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool AccommodationPaymentExists(int? id)
    {
        return _context.AccommodationPayment.Any(e => e.AccommodationPaymentId == id);
    }
}
