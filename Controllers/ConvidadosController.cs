using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class ConvidadosController : Controller
    {
        private readonly UniversityDbContext _context;

        public ConvidadosController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Convidados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Convidado.ToListAsync());
        }

        // GET: Convidados/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convidado = await _context.Convidado
                .FirstOrDefaultAsync(m => m.ConvidadoId == id);
            if (convidado == null)
            {
                return NotFound();
            }

            return View(convidado);
        }

        // GET: Convidados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Convidados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConvidadoId,Nome,Nif,Email,Telefone,Ativo")] Convidado convidado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(convidado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(convidado);
        }

        // GET: Convidados/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convidado = await _context.Convidado.FindAsync(id);
            if (convidado == null)
            {
                return NotFound();
            }
            return View(convidado);
        }

        // POST: Convidados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ConvidadoId,Nome,Nif,Email,Telefone,Ativo")] Convidado convidado)
        {
            if (id != convidado.ConvidadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(convidado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConvidadoExists(convidado.ConvidadoId))
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
            return View(convidado);
        }

        // GET: Convidados/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convidado = await _context.Convidado
                .FirstOrDefaultAsync(m => m.ConvidadoId == id);
            if (convidado == null)
            {
                return NotFound();
            }

            return View(convidado);
        }

        // POST: Convidados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var convidado = await _context.Convidado.FindAsync(id);
            if (convidado != null)
            {
                _context.Convidado.Remove(convidado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConvidadoExists(long id)
        {
            return _context.Convidado.Any(e => e.ConvidadoId == id);
        }
    }
}