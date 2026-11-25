
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    // Loads the available contracts for the dropdown list
    private async Task LoadContractsAsync(int? selectedContractId = null)
    {
        var contracts = await _context.AccommodationContract
            .OrderBy(c => c.AccommodationContractId)
            .Select(c => new SelectListItem
            {
                Value = c.AccommodationContractId.ToString(),
                Text = "Contract #" + c.AccommodationContractId,
                Selected = c.AccommodationContractId == selectedContractId
            })
            .ToListAsync();

        ViewData["AccommodationContractId"] = contracts;
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

        var accommodationPayment = await _context.AccommodationPayment
            .FirstOrDefaultAsync(m => m.AccommodationPaymentId == id);

        if (accommodationPayment == null)
        {
            return NotFound();
        }

        return View(accommodationPayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Create
    public async Task<IActionResult> Create()
    {
        await LoadContractsAsync();
        return View();
    }

    // POST: ACCOMMODATIONPAYMENTS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("AccommodationPaymentId,AccommodationContractId,DueDate,Amount,PaidAmount,PaidAt,Status")]
        AccommodationPayment accommodationPayment)
    {
        // Check whether the selected contract exists
        if (accommodationPayment.AccommodationContractId.HasValue)
        {
            bool contractExists = await _context.AccommodationContract
                .AnyAsync(c => c.AccommodationContractId == accommodationPayment.AccommodationContractId);

            if (!contractExists)
            {
                ModelState.AddModelError(
                    nameof(accommodationPayment.AccommodationContractId),
                    "The selected contract does not exist.");
            }
        }

        if (ModelState.IsValid)
        {
            _context.Add(accommodationPayment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Payment created successfully!";
            return RedirectToAction(nameof(Index));
        }

        await LoadContractsAsync(accommodationPayment.AccommodationContractId);

        TempData["ErrorMessage"] = "Could not create the payment. Please check the form errors.";
        return View(accommodationPayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationPayment = await _context.AccommodationPayment.FindAsync(id);

        if (accommodationPayment == null)
        {
            return NotFound();
        }

        await LoadContractsAsync(accommodationPayment.AccommodationContractId);

        return View(accommodationPayment);
    }

    // POST: ACCOMMODATIONPAYMENTS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("AccommodationPaymentId,AccommodationContractId,DueDate,Amount,PaidAmount,PaidAt,Status")]
        AccommodationPayment accommodationPayment)
    {
        if (id != accommodationPayment.AccommodationPaymentId)
        {
            return NotFound();
        }

        // Check whether the selected contract exists
        if (accommodationPayment.AccommodationContractId.HasValue)
        {
            bool contractExists = await _context.AccommodationContract
                .AnyAsync(c => c.AccommodationContractId == accommodationPayment.AccommodationContractId);

            if (!contractExists)
            {
                ModelState.AddModelError(
                    nameof(accommodationPayment.AccommodationContractId),
                    "The selected contract does not exist.");
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(accommodationPayment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Payment updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationPaymentExists(accommodationPayment.AccommodationPaymentId))
                {
                    return NotFound();
                }

                throw;
            }
        }

        await LoadContractsAsync(accommodationPayment.AccommodationContractId);

        TempData["ErrorMessage"] = "Could not update the payment. Please check the form errors.";
        return View(accommodationPayment);
    }

    // GET: ACCOMMODATIONPAYMENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var accommodationPayment = await _context.AccommodationPayment
            .FirstOrDefaultAsync(m => m.AccommodationPaymentId == id);

        if (accommodationPayment == null)
        {
            return NotFound();
        }

        return View(accommodationPayment);
    }

    // POST: ACCOMMODATIONPAYMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        try
        {
            var accommodationPayment = await _context.AccommodationPayment.FindAsync(id);

            if (accommodationPayment != null)
            {
                _context.AccommodationPayment.Remove(accommodationPayment);
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

    private bool AccommodationPaymentExists(int id)
    {
        return _context.AccommodationPayment.Any(e => e.AccommodationPaymentId == id);
    }
}
