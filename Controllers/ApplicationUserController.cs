
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class ApplicationUserController : Controller
{
    private readonly UniversityDbContext _context;

    public ApplicationUserController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: APPLICATIONUSERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ApplicationUser.ToListAsync());
    }

    // GET: APPLICATIONUSERS/Details/5
    [HttpGet("ApplicationUser/Details/{ApplicationUserId}")]
    public async Task<IActionResult> Details(int? applicationuserid)
    {
        if (applicationuserid == null)
        {
            return NotFound();
        }

        var applicationuser = await _context.ApplicationUser
            .FirstOrDefaultAsync(m => m.ApplicationUserId == applicationuserid);
        if (applicationuser == null)
        {
            return NotFound();
        }

        return View(applicationuser);
    }

    // GET: APPLICATIONUSERS/Create

    public IActionResult Create()
    {
        return View();
    }

    // POST: APPLICATIONUSERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ApplicationUserId,Nome,Email,PalavraPasse")] ApplicationUser applicationuser)
    {
        if (ModelState.IsValid)
        {
            _context.Add(applicationuser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(applicationuser);
    }

    // GET: APPLICATIONUSERS/Edit/5
    [HttpGet("ApplicationUser/Edit/{ApplicationUserId}")]
    public async Task<IActionResult> Edit(int? applicationuserid)
    {
        if (applicationuserid == null)
        {
            return NotFound();
        }

        var applicationuser = await _context.ApplicationUser.FindAsync(applicationuserid);
        if (applicationuser == null)
        {
            return NotFound();
        }
        return View(applicationuser);
    }

    // POST: APPLICATIONUSERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? applicationuserid, [Bind("ApplicationUserId,Nome,Email,PalavraPasse")] ApplicationUser applicationuser)
    {
        if (applicationuserid != applicationuser.ApplicationUserId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(applicationuser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(applicationuser.ApplicationUserId))
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
        return View(applicationuser);
    }

    // GET: APPLICATIONUSERS/Delete/5
    [HttpGet("ApplicationUser/Delete/{ApplicationUserId}")]
    public async Task<IActionResult> Delete(int? applicationuserid)
    {
        if (applicationuserid == null)
        {
            return NotFound();
        }

        var applicationuser = await _context.ApplicationUser
            .FirstOrDefaultAsync(m => m.ApplicationUserId == applicationuserid);
        if (applicationuser == null)
        {
            return NotFound();
        }

        return View(applicationuser);
    }

    // POST: APPLICATIONUSERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? applicationuserid)
    {
        var applicationuser = await _context.ApplicationUser.FindAsync(applicationuserid);
        if (applicationuser != null)
        {
            _context.ApplicationUser.Remove(applicationuser);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ApplicationUserExists(int? applicationuserid)
    {
        return _context.ApplicationUser.Any(e => e.ApplicationUserId == applicationuserid);
    }
}
