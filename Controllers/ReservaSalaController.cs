
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using UnivManagement.Models;
using UnivManagement.Data;

public class ReservaSalaController : Controller
{
    private readonly UniversityDbContext _context;

    public ReservaSalaController(UniversityDbContext context)
    {
        _context = context;
    }

    // GET: ReservaSala

    public async Task<IActionResult> Index(string? successMessage = null)
    {
        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        var reservas = await _context.Reserva
            .Include(r => r.Sala)
            .ToListAsync();

        return View(reservas);
    }


    // GET: ReservaSala/Details/5
    [HttpGet("ReservaSala/Details/{ReservaSalaId}")]
    public async Task<IActionResult> Details(
        int? reservasalaid,
        string? successMessage = null)
    {
        if (reservasalaid == null)
        {
            return NotFound();
        }


        var reservasala = await _context.Reserva
            .Include(r => r.Sala)
            .FirstOrDefaultAsync(m => m.ReservaSalaId == reservasalaid);


        if (reservasala == null)
        {
            return View("NotFound");
        }



        if (successMessage != null)
        {
            ViewBag.SuccessMessage = successMessage;
        }

        return View(reservasala);
    }

    // GET: ReservaSala/Create
    public IActionResult Create()
    {
        ViewBag.SalaFk = new SelectList(
            _context.SalasAula,
            "RoomId",
            "NumeroSala"
        );

        return View();
    }

    // POST: ReservaSala/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ReservaSalaId,Responsavel,Motivo,DataInicio,DataFim,Estado,SalaFk")]
        ReservaSala reservasala)
    {
        // Validar as datas
        if (reservasala.DataFim <= reservasala.DataInicio)
        {
            ModelState.AddModelError(
                "DataFim",
                "A data e hora de fim devem ser posteriores à data e hora de início."
            );
        }

        // Deteção de conflitos temporariamente desativada
        /*
        if (reservasala.DataInicio.HasValue &&
            reservasala.DataFim.HasValue &&
            reservasala.DataFim > reservasala.DataInicio)
        {
            bool existeConflito = await _context.Reserva.AnyAsync(r =>
                r.SalaFk == reservasala.SalaFk &&
                r.DataInicio.HasValue &&
                r.DataFim.HasValue &&
                r.DataFim > r.DataInicio &&
                r.DataInicio < reservasala.DataFim &&
                r.DataFim > reservasala.DataInicio &&
                r.Estado != "Cancelada" &&
                r.Estado != "Cancelado");

            if (existeConflito)
            {
                ModelState.AddModelError(
                    "SalaFk",
                    "Esta sala já está reservada para o período escolhido."
                );
            }
        }
        */

        if (ModelState.IsValid)
        {
            _context.Reserva.Add(reservasala);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new
                {
                    ReservaSalaId = reservasala.ReservaSalaId,
                    successMessage = "Reserva de sala criada com sucesso!"
                }
            );
        }

        ViewBag.SalaFk = new SelectList(
            _context.SalasAula,
            "RoomId",
            "NumeroSala",
            reservasala.SalaFk
        );

        return View(reservasala);
    }

    // GET: ReservaSala/Edit/5
    [HttpGet("ReservaSala/Edit/{ReservaSalaId}")]
    public async Task<IActionResult> Edit(int? reservasalaid)
    {
        if (reservasalaid == null)
        {
            return NotFound();
        }

        var reservasala = await _context.Reserva
            .FindAsync(reservasalaid);

        if (reservasala == null)
        {
            return NotFound();
        }

        ViewBag.SalaFk = new SelectList(
            _context.SalasAula,
            "RoomId",
            "NumeroSala",
            reservasala.SalaFk
        );

        return View(reservasala);
    }

    // POST: ReservaSala/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? reservasalaid,
        [Bind("ReservaSalaId,Responsavel,Motivo,DataInicio,DataFim,Estado,SalaFk")]
        ReservaSala reservasala)
    {
        if (reservasalaid != reservasala.ReservaSalaId)
        {
            return NotFound();
        }

        // Validar as datas
        if (reservasala.DataFim <= reservasala.DataInicio)
        {
            ModelState.AddModelError(
                "DataFim",
                "A data e hora de fim devem ser posteriores à data e hora de início."
            );
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

                throw;
            }

            return RedirectToAction(
                nameof(Details),
                new
                {
                    ReservaSalaId = reservasala.ReservaSalaId,
                    successMessage = "Reserva de sala atualizada com sucesso!"
                }
            );
        }

        ViewBag.SalaFk = new SelectList(
            _context.SalasAula,
            "RoomId",
            "NumeroSala",
            reservasala.SalaFk
        );

        return View(reservasala);
    }

    // GET: ReservaSala/Delete/5
    [HttpGet("ReservaSala/Delete/{ReservaSalaId}")]
    public async Task<IActionResult> Delete(int? reservasalaid)
    {
        if (reservasalaid == null)
        {
            return NotFound();
        }


        var reservasala = await _context.Reserva
            .Include(r => r.Sala)
            .FirstOrDefaultAsync(m => m.ReservaSalaId == reservasalaid);


        // Reserva já eliminada ou inexistente
        if (reservasala == null)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    successMessage = "A reserva de sala não existe ou já foi eliminada."
                }
            );
        }


        return View(reservasala);
    }

    // POST: ReservaSala/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? reservasalaid)
    {
        var reservasala = await _context.Reserva
            .FindAsync(reservasalaid);

        // Reserva já eliminada ou inexistente
        if (reservasala == null)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    successMessage = "A reserva de sala não existe ou já foi eliminada."
                }
            );
        }

        _context.Reserva.Remove(reservasala);
        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(Index),
            new
            {
                successMessage = "Reserva de sala eliminada com sucesso!"
            }
        );
    }

    // Verificar se a reserva existe
    private bool ReservaSalaExists(int? reservasalaid)
    {
        return _context.Reserva
            .Any(e => e.ReservaSalaId == reservasalaid);
    }
}
