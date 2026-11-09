using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class EstudantesController : Controller
    {
        private readonly UniversityDbContext _context;

        public EstudantesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Estudantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Estudantes.ToListAsync());
        }

        // GET: Estudantes/SeedData
        public async Task<IActionResult> SeedData()
        {
            if (!await _context.Estudantes.AnyAsync())
            {
                _context.Estudantes.AddRange(
                    new Estudante { Nome = "João Pereira", Email = "joao.pereira@universidade.pt", Curso = "Engenharia Informática", SituacaoProfissional = "Estudante" },
                    new Estudante { Nome = "Sofia Martins", Email = "sofia.martins@universidade.pt", Curso = "Engenharia Informática", SituacaoProfissional = "Estudante" },
                    new Estudante { Nome = "Daniel Fonseca", Email = "daniel.fonseca@universidade.pt", Curso = "Desporto", SituacaoProfissional = "Estudante" }
                );
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Estudantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudante = await _context.Estudantes
                .FirstOrDefaultAsync(m => m.IdAluno == id);
            if (estudante == null)
            {
                return NotFound();
            }

            return View(estudante);
        }

        // GET: Estudantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estudantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAluno,Nome,Email,Password,Curso,SituacaoProfissional")] Estudante estudante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estudante);
        }

        // GET: Estudantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudante = await _context.Estudantes.FindAsync(id);
            if (estudante == null)
            {
                return NotFound();
            }
            return View(estudante);
        }

        // POST: Estudantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAluno,Nome,Email,Password,Curso,SituacaoProfissional")] Estudante estudante)
        {
            if (id != estudante.IdAluno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudanteExists(estudante.IdAluno))
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
            return View(estudante);
        }

        // GET: Estudantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudante = await _context.Estudantes
                .FirstOrDefaultAsync(m => m.IdAluno == id);
            if (estudante == null)
            {
                return NotFound();
            }

            return View(estudante);
        }

        // POST: Estudantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudante = await _context.Estudantes.FindAsync(id);
            if (estudante != null)
            {
                _context.Estudantes.Remove(estudante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudanteExists(int id)
        {
            return _context.Estudantes.Any(e => e.IdAluno == id);
        }
    }
}