using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Models;
using UnivManagement.Data;

namespace UnivManagement.Controllers
{
    public class DocentesController : Controller
    {
        private readonly UniversityDbContext _context;

        public DocentesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Docentes
        public async Task<IActionResult> Index()
        {
            var docentes = _context.Docentes.Include(d => d.Departamento);
            return View(await docentes.ToListAsync());
        }

        // GET: Docentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var docente = await _context.Docentes
                .Include(d => d.Departamento)
                .FirstOrDefaultAsync(m => m.DocenteId == id);

            if (docente == null) return NotFound();

            return View(docente);
        }

        // GET: Docentes/Create
        public IActionResult Create()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nome");
            return View();
        }

        // POST: Docentes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocenteId,NumDocente,Nome,Nif,Email,Telefone,Ativo,DataAdmissao,Morada,FuncaoDocenteId,DepartamentoId")] Docente docente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docente);
                await _context.SaveChangesAsync();

                // Mensagem de sucesso para o Create
                TempData["MensagemSucesso"] = "Docente criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nome", docente.DepartamentoId);
            return View(docente);
        }

        // GET: Docentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var docente = await _context.Docentes
                .Include(d => d.Departamento)
                .FirstOrDefaultAsync(m => m.DocenteId == id);

            if (docente == null) return NotFound();

            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nome", docente.DepartamentoId);
            return View(docente);
        }

        // POST: Docentes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocenteId,NumDocente,Nome,Nif,Email,Telefone,Ativo,DataAdmissao,Morada,FuncaoDocenteId,DepartamentoId")] Docente docente)
        {
            if (id != docente.DocenteId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docente);
                    await _context.SaveChangesAsync();

                    // Mensagem de sucesso para o Edit
                    TempData["MensagemSucesso"] = "Docente atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocenteExists(docente.DocenteId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "DepartamentoId", "Nome", docente.DepartamentoId);
            return View(docente);
        }

        // GET: Docentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var docente = await _context.Docentes
                .Include(d => d.Departamento)
                .FirstOrDefaultAsync(m => m.DocenteId == id);

            if (docente == null) return NotFound();

            return View(docente);
        }

        // POST: Docentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
                await _context.SaveChangesAsync();

                // Mensagem de sucesso para o Delete
                TempData["MensagemSucesso"] = "Docente eliminado com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DocenteExists(int id)
        {
            return _context.Docentes.Any(e => e.DocenteId == id);
        }
    }
}