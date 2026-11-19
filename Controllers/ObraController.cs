
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class ObraController : Controller
    {
        private readonly UniversityDbContext _context;

        public ObraController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Obra
        public async Task<IActionResult> Index(string? successMessage = null)
        {
            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            var obras = await _context.Obras
                .Include(o => o.ObrasAutores)
                    .ThenInclude(oa => oa.Autor)
                .ToListAsync();

            return View(obras);
        }

        // GET: Obra/Details/5
        public async Task<IActionResult> Details(
            int? id,
            string? successMessage = null)
        {
            if (id == null)
                return View("NotFound");

            var obra = await _context.Obras
                .Include(o => o.ObrasAutores)
                    .ThenInclude(oa => oa.Autor)
                .FirstOrDefaultAsync(o => o.ObraId == id);

            if (obra == null)
                return View("NotFound");

            if (successMessage != null)
            {
                ViewBag.SuccessMessage = successMessage;
            }

            return View(obra);
        }

        // GET: Obra/Create
        public async Task<IActionResult> Create()
        {
            await CarregarAutores();
            return View();
        }

        // POST: Obra/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ISBN,Titulo,Tipo,ValorMulta")] Obra obra,
            List<int> autoresSelecionados)
        {
            await ValidarAutores(autoresSelecionados);

            if (ModelState.IsValid)
            {
                foreach (var autorId in autoresSelecionados.Distinct())
                {
                    obra.ObrasAutores.Add(new ObraAutor
                    {
                        AutorId = autorId
                    });
                }

                _context.Obras.Add(obra);
                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = obra.ObraId,
                        successMessage = "Obra criada com sucesso."
                    });
            }

            await CarregarAutores(autoresSelecionados);
            return View(obra);
        }

        // GET: Obra/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return View("NotFound");

            var obra = await _context.Obras
                .Include(o => o.ObrasAutores)
                .FirstOrDefaultAsync(o => o.ObraId == id);

            if (obra == null)
                return View("NotFound");

            await CarregarAutores(
                obra.ObrasAutores.Select(oa => oa.AutorId));

            return View(obra);
        }

        // POST: Obra/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? id,
            [Bind("ObraId,ISBN,Titulo,Tipo,ValorMulta")] Obra dados,
            List<int> autoresSelecionados)
        {
            if (id == null || id != dados.ObraId)
                return View("NotFound");

            var obra = await _context.Obras
                .Include(o => o.ObrasAutores)
                .FirstOrDefaultAsync(o => o.ObraId == id);

            if (obra == null)
                return View("NotFound");

            await ValidarAutores(autoresSelecionados);

            if (ModelState.IsValid)
            {
                obra.ISBN = dados.ISBN;
                obra.Titulo = dados.Titulo;
                obra.Tipo = dados.Tipo;
                obra.ValorMulta = dados.ValorMulta;

                var idsNovos = autoresSelecionados
                    .Distinct()
                    .ToHashSet();

                var associacoesRemover = obra.ObrasAutores
                    .Where(oa => !idsNovos.Contains(oa.AutorId))
                    .ToList();

                _context.ObrasAutores.RemoveRange(associacoesRemover);

                var idsAtuais = obra.ObrasAutores
                    .Select(oa => oa.AutorId)
                    .ToHashSet();

                foreach (var autorId in idsNovos.Except(idsAtuais))
                {
                    obra.ObrasAutores.Add(new ObraAutor
                    {
                        AutorId = autorId
                    });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = obra.ObraId,
                        successMessage = "Os detalhes da obra foram alterados com sucesso."
                    });
            }

            await CarregarAutores(autoresSelecionados);
            return View(dados);
        }

        // GET: Obra/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return View("NotFound");

            var obra = await _context.Obras
                .Include(o => o.ObrasAutores)
                    .ThenInclude(oa => oa.Autor)
                .FirstOrDefaultAsync(o => o.ObraId == id);

            if (obra == null)
                return View("NotFound");

            return View(obra);
        }

        // POST: Obra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return View("NotFound");

            var obra = await _context.Obras.FindAsync(id);

            if (obra == null)
                return View("NotFound");

            _context.Obras.Remove(obra);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Index),
                new
                {
                    successMessage = "Obra eliminada com sucesso."
                });
        }

        private async Task CarregarAutores(
            IEnumerable<int>? selecionados = null)
        {
            var autores = await _context.Autores
                .OrderBy(a => a.Nome)
                .ToListAsync();

            ViewBag.Autores = new MultiSelectList(
                autores,
                "AutorId",
                "Nome",
                selecionados
            );
        }

        private async Task ValidarAutores(
            List<int> autoresSelecionados)
        {
            var ids = autoresSelecionados
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                ModelState.AddModelError(
                    "autoresSelecionados",
                    "Selecione pelo menos um autor."
                );

                return;
            }

            var quantidadeExistente = await _context.Autores
                .CountAsync(a => ids.Contains(a.AutorId));

            if (quantidadeExistente != ids.Count)
            {
                ModelState.AddModelError(
                    "autoresSelecionados",
                    "Um ou mais autores selecionados são inválidos."
                );
            }
        }
    }
}
