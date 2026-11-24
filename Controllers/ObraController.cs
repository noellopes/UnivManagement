
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
                .Include(o => o.Tipo)
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
                .Include(o => o.Tipo)
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
            await CarregarTipos();
            return View();
        }

        // POST: Obra/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ISBN_ISSN,Titulo,TipoObraId,ValorMulta")] Obra obra,
            List<int> autoresSelecionados)
        {
            await ValidarAutores(autoresSelecionados);

            var tipoExiste = await _context.Tipos
            .AnyAsync(t => t.TipoObraId == obra.TipoObraId);

            if (!tipoExiste)
            {
                ModelState.AddModelError(
                    "TipoObraId",
                    "Selecione um tipo de obra válido."
                );
            }

            await ValidarIdentificador(obra);

            if (ModelState.TryGetValue(nameof(Obra.ValorMulta), out var estado) &&
    estado.Errors.Count > 0)
            {
                var valorRecebido = estado.AttemptedValue;

                if (!double.TryParse(valorRecebido, out _))
                {
                    ModelState.Remove(nameof(Obra.ValorMulta));

                    ModelState.AddModelError(
                        nameof(Obra.ValorMulta),
                        "Introduz um valor de multa válido. Exemplo: 2,50 €.");
                }
            }



            if (ModelState.IsValid)
            {
                obra.ISBN_ISSN = Obra.NormalizarIdentificador(obra.ISBN_ISSN);

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
            await CarregarTipos(obra.TipoObraId);
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

            await CarregarTipos(obra.TipoObraId);

            return View(obra);
        }

        // POST: Obra/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int? id,
            [Bind("ObraId,ISBN_ISSN,Titulo,TipoObraId,ValorMulta")] Obra dados,
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

            var tipoExiste = await _context.Tipos
            .AnyAsync(t => t.TipoObraId == dados.TipoObraId);

            if (!tipoExiste)
            {
                ModelState.AddModelError(
                    "TipoObraId",
                    "Selecione um tipo de obra válido."
                );
            }

            await ValidarIdentificador(dados, obra.ObraId);

            if (ModelState.TryGetValue(nameof(Obra.ValorMulta), out var estado) &&
    estado.Errors.Any(e => e.Exception != null))
            {
                ModelState.Remove(nameof(Obra.ValorMulta));

                ModelState.AddModelError(
                    nameof(Obra.ValorMulta),
                    "Introduz um valor de multa válido. Exemplo: 2,50 €.");
            }

            if (ModelState.IsValid)
            {
                obra.ISBN_ISSN = Obra.NormalizarIdentificador(dados.ISBN_ISSN);
                obra.Titulo = dados.Titulo;
                obra.TipoObraId = dados.TipoObraId;
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
            await CarregarTipos(dados.TipoObraId);
            return View(dados);
        }

        // GET: Obra/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return View("NotFound");

            var obra = await _context.Obras
                .Include(o => o.Tipo)
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

        private async Task CarregarTipos(int? tipoSelecionado = null)
        {
            var tipos = await _context.Tipos
                .OrderBy(t => t.NomeTipo)
                .ToListAsync();

            ViewBag.Tipos = new SelectList(
                tipos,
                "TipoObraId",
                "NomeTipo",
                tipoSelecionado
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



        private async Task ValidarIdentificador(
        Obra obra, int? obraIdExcluir = null)
        {
            var tipoObra = await _context.Tipos
                .FirstOrDefaultAsync(t => t.TipoObraId == obra.TipoObraId);

            if (tipoObra == null)
                return;

            string nomeTipo = tipoObra.NomeTipo.Trim();

            bool usaISBN =
                nomeTipo.Equals("Livro", StringComparison.OrdinalIgnoreCase) ||
                nomeTipo.Equals("Livro Técnico", StringComparison.OrdinalIgnoreCase) ||
                nomeTipo.Equals("Manual", StringComparison.OrdinalIgnoreCase);

            bool usaISSN =
                nomeTipo.Equals("Revista", StringComparison.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(obra.ISBN_ISSN))
            {
                if (usaISBN || usaISSN)
                {
                    ModelState.AddModelError(
                        nameof(Obra.ISBN_ISSN),
                        usaISBN
                            ? "Introduz o ISBN da obra."
                            : "Introduz o ISSN da revista.");
                }

                return;
            }

            bool isbnValido = Obra.ISBNValido(obra.ISBN_ISSN);
            bool issnValido = Obra.ISSNValido(obra.ISBN_ISSN);

            bool valido = usaISBN
                ? isbnValido
                : usaISSN
                    ? issnValido
                    : isbnValido || issnValido;

            if (!valido)
            {
                ModelState.AddModelError(
                    nameof(Obra.ISBN_ISSN),
                    usaISBN
                        ? "Introduz um ISBN-13 válido. Exemplo: ISBN 978-972-0-04957-5."
                        : usaISSN
                            ? "Introduz um ISSN válido. Exemplo: ISSN 0378-5955."
                            : "Introduz um ISBN ou ISSN válido.");

                return;
            }

            if (isbnValido)
            {
                var isbnNormalizado =
                    Obra.NormalizarIdentificador(obra.ISBN_ISSN);

                var identificadores = await _context.Obras
                    .Where(o => o.ISBN_ISSN != null &&
                                o.ObraId != obraIdExcluir)
                    .Select(o => o.ISBN_ISSN)
                    .ToListAsync();

                bool isbnExiste = identificadores.Any(i =>
                    Obra.NormalizarIdentificador(i) == isbnNormalizado);

                if (isbnExiste)
                {
                    ModelState.AddModelError(
                        nameof(Obra.ISBN_ISSN),
                        "Já existe uma obra registada com este ISBN.");
                }
            }
        }
    }
}
    
