
using UnivManagement.Data;
using UnivManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnivManagement.Controllers
{
    public class TipoCandidaturasController : Controller
    {
        private readonly UniversityDbContext _context;

        public TipoCandidaturasController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: TipoCandidaturas
        public async Task<IActionResult> Index(
            string? successMessage = null,
            string? successType = null)
        {
            // Display success messages only when they are for deletion
            if (successType == "danger" && !string.IsNullOrEmpty(successMessage))
            {
                ViewBag.SuccessMessage = successMessage;
                ViewBag.SuccessType = successType;
            }

            var tipoCandidaturas = await _context.TipoCandidatura
                .ToListAsync();

            return View(tipoCandidaturas);
        }

        // GET: TipoCandidaturas/Details/5
        public async Task<IActionResult> Details(
            int? id,
            string? successMessage = null,
            string? successType = null)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var tipoCandidatura = await _context.TipoCandidatura
                .FirstOrDefaultAsync(m => m.TipoCandidaturaId == id);

            if (tipoCandidatura == null)
            {
                return View("NotFound");
            }

            ViewBag.SuccessMessage = successMessage;
            ViewBag.SuccessType = successType ?? "success";

            return View(tipoCandidatura);
        }

        // GET: TipoCandidaturas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoCandidaturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TipoCandidaturaId,Designacao,Descricao")]
            TipoCandidatura tipoCandidatura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoCandidatura);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new
                {
                    id = tipoCandidatura.TipoCandidaturaId,
                    successMessage = "Application type successfully created.",
                    successType = "success"
                });
            }

            return View(tipoCandidatura);
        }

        // GET: TipoCandidaturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var tipoCandidatura = await _context.TipoCandidatura
                .FindAsync(id);

            if (tipoCandidatura == null)
            {
                return View("NotFound");
            }

            return View(tipoCandidatura);
        }

        // POST: TipoCandidaturas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? id,
            [Bind("TipoCandidaturaId,Designacao,Descricao")]
            TipoCandidatura tipoCandidatura)
        {
            if (id == null || id != tipoCandidatura.TipoCandidaturaId)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoCandidatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoCandidaturaExists(tipoCandidatura.TipoCandidaturaId))
                    {
                        return View("NotFound");
                    }

                    throw;
                }

                return RedirectToAction(nameof(Details), new
                {
                    id = tipoCandidatura.TipoCandidaturaId,
                    successMessage = "Application type successfully updated.",
                    successType = "warning"
                });
            }

            return View(tipoCandidatura);
        }

        // GET: TipoCandidaturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var tipoCandidatura = await _context.TipoCandidatura
                .FirstOrDefaultAsync(m => m.TipoCandidaturaId == id);

            if (tipoCandidatura == null)
            {
                return View("NotFound");
            }

            return View(tipoCandidatura);
        }

        // POST: TipoCandidaturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var tipoCandidatura = await _context.TipoCandidatura
                .FindAsync(id);

            if (tipoCandidatura == null)
            {
                return View("NotFound");
            }

            _context.TipoCandidatura.Remove(tipoCandidatura);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new
            {
                successMessage = "Application type successfully deleted.",
                successType = "danger"
            });
        }

        // Check whether an application type exists
        private bool TipoCandidaturaExists(int id)
        {
            return _context.TipoCandidatura.Any(
                e => e.TipoCandidaturaId == id);
        }
    }
}
