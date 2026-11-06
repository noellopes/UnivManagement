
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

public class ReservaSalaController : Controller
{
    private readonly UniversityDbContext _context;

    public ReservaSalaController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: RESERVASALAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Reserva.ToListAsync());
    }

    // GET: RESERVASALAS/Details/5
    [HttpGet("ReservaSala/Details/{ReservaSalaId}")]
    public async Task<IActionResult> Details(int? reservasalaid)
    {
        if (reservasalaid == null)
        {
            return NotFound();
        }

        var reservasala = await _context.Reserva
            .FirstOrDefaultAsync(m => m.ReservaSalaId == reservasalaid);
        if (reservasala == null)
        {
            return NotFound();
        }

        return View(reservasala);
    }

    // GET: RESERVASALAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: RESERVASALAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ReservaSalaId,Responsavel,Motivo,DataInicio,DataFim,Estado,SalaFk")] ReservaSala reservasala)
    {
        if (ModelState.IsValid)
        {
            _context.Add(reservasala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(reservasala);
    }

    // GET: RESERVASALAS/Edit/5
    [HttpGet("ReservaSala/Edit/{ReservaSalaId}")]
    public async Task<IActionResult> Edit(int? reservasalaid)
    {
        if (reservasalaid == null)
        {
            return NotFound();
        }

        var reservasala = await _context.Reserva.FindAsync(reservasalaid);
        if (reservasala == null)
        {
            return NotFound();
        }
        return View(reservasala);
    }

    // POST: RESERVASALAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? reservasalaid, [Bind("ReservaSalaId,Responsavel,Motivo,DataInicio,DataFim,Estado,SalaFk")] ReservaSala reservasala)
    {
        if (reservasalaid != reservasala.ReservaSalaId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(reservasala);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaSalaExists(reservasala.ReservaSalaId))
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
        return View(reservasala);
    }

    // GET: RESERVASALAS/Delete/5
    [HttpGet("ReservaSala/Delete/{ReservaSalaId}")]
    public async Task<IActionResult> Delete(int? reservasalaid)
    {
        if (reservasalaid == null)
        {
            return NotFound();
        }

        var reservasala = await _context.Reserva
            .FirstOrDefaultAsync(m => m.ReservaSalaId == reservasalaid);
        if (reservasala == null)
        {
            return NotFound();
        }

        return View(reservasala);
    }

    // POST: RESERVASALAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? reservasalaid)
    {
        var reservasala = await _context.Reserva.FindAsync(reservasalaid);
        if (reservasala != null)
        {
            _context.Reserva.Remove(reservasala);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReservaSalaExists(int? reservasalaid)
    {
        return _context.Reserva.Any(e => e.ReservaSalaId == reservasalaid);
    }
}
