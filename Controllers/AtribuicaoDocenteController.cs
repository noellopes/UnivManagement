using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Controllers
{
    public class AtribuicaoDocenteController : Controller
    {
        private readonly UniversityDbContext _context;

        public AtribuicaoDocenteController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: AtribuicaoDocente
        [HttpGet("AtribuicaoDocente")]
        [HttpGet("AtribuicaoDocente/Index")]
        public async Task<IActionResult> Index()
        {
            var atribuicoes = _context.AtribuicaoDocente
                .Include(a => a.Docente)
                .Include(a => a.UnidadeCurricular)
                .Include(a => a.AnoLetivo);
            return View(await atribuicoes.ToListAsync());
        }

        // GET: AtribuicaoDocente/Details/5
        [HttpGet("AtribuicaoDocente/Details/{atribuicaodocenteid}")]
        public async Task<IActionResult> Details(int? atribuicaodocenteid)
        {
            if (atribuicaodocenteid == null)
            {
                return NotFound();
            }

            var atribuicao = await _context.AtribuicaoDocente
                .Include(a => a.Docente)
                .Include(a => a.UnidadeCurricular)
                .Include(a => a.AnoLetivo)
                .FirstOrDefaultAsync(m => m.AtribuicaoDocenteId == atribuicaodocenteid);
            if (atribuicao == null)
            {
                return NotFound();
            }

            return View(atribuicao);
        }

        // GET: AtribuicaoDocente/Create
        [HttpGet("AtribuicaoDocente/Create")]
        public IActionResult Create()
        {
            CarregarListas();
            return View();
        }

        // POST: AtribuicaoDocente/Create
        [HttpPost("AtribuicaoDocente/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocenteId,UcId,AcademicYearId")] AtribuicaoDocente atribuicao)
        {
            if (await JaExiste(atribuicao))
            {
                ModelState.AddModelError(string.Empty,
                    "Este docente já está atribuído a esta unidade curricular neste ano letivo.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(atribuicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarListas(atribuicao);
            return View(atribuicao);
        }

        // GET: AtribuicaoDocente/Edit/5
        [HttpGet("AtribuicaoDocente/Edit/{atribuicaodocenteid}")]
        public async Task<IActionResult> Edit(int? atribuicaodocenteid)
        {
            if (atribuicaodocenteid == null)
            {
                return NotFound();
            }

            var atribuicao = await _context.AtribuicaoDocente.FindAsync(atribuicaodocenteid);
            if (atribuicao == null)
            {
                return NotFound();
            }

            CarregarListas(atribuicao);
            return View(atribuicao);
        }

        // POST: AtribuicaoDocente/Edit/5
        [HttpPost("AtribuicaoDocente/Edit/{atribuicaodocenteid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? atribuicaodocenteid, [Bind("AtribuicaoDocenteId,DocenteId,UcId,AcademicYearId")] AtribuicaoDocente atribuicao)
        {
            if (atribuicaodocenteid != atribuicao.AtribuicaoDocenteId)
            {
                return NotFound();
            }

            if (await JaExiste(atribuicao))
            {
                ModelState.AddModelError(string.Empty,
                    "Este docente já está atribuído a esta unidade curricular neste ano letivo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(atribuicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtribuicaoExists(atribuicao.AtribuicaoDocenteId))
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

            CarregarListas(atribuicao);
            return View(atribuicao);
        }

        // GET: AtribuicaoDocente/Delete?atribuicaodocenteid=5
        public async Task<IActionResult> Delete(int? atribuicaodocenteid)
        {
            if (atribuicaodocenteid == null)
            {
                return NotFound();
            }

            var atribuicao = await _context.AtribuicaoDocente
                .Include(a => a.Docente)
                .Include(a => a.UnidadeCurricular)
                .Include(a => a.AnoLetivo)
                .FirstOrDefaultAsync(m => m.AtribuicaoDocenteId == atribuicaodocenteid);
            if (atribuicao == null)
            {
                return NotFound();
            }

            return View(atribuicao);
        }

        // POST: AtribuicaoDocente/Delete?atribuicaodocenteid=5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? atribuicaodocenteid)
        {
            var atribuicao = await _context.AtribuicaoDocente.FindAsync(atribuicaodocenteid);
            if (atribuicao != null)
            {
                _context.AtribuicaoDocente.Remove(atribuicao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtribuicaoExists(int? atribuicaodocenteid)
        {
            return _context.AtribuicaoDocente.Any(e => e.AtribuicaoDocenteId == atribuicaodocenteid);
        }

        // Evita duplicados antes do indice unico dar erro na base de dados
        private async Task<bool> JaExiste(AtribuicaoDocente a)
        {
            return await _context.AtribuicaoDocente.AnyAsync(e =>
                e.DocenteId == a.DocenteId &&
                e.UcId == a.UcId &&
                e.AcademicYearId == a.AcademicYearId &&
                e.AtribuicaoDocenteId != a.AtribuicaoDocenteId);
        }

        // Listas para os <select> (sem DbSet no contexto: usa-se Set<T>())
        // AJUSTAR: nomes das chaves e das colunas a mostrar conforme as classes existentes.
        private void CarregarListas(AtribuicaoDocente? atribuicao = null)
        {
            ViewData["DocenteId"] = new SelectList(
                _context.Set<Docente>(), "DocenteId", "Nome", atribuicao?.DocenteId);

            ViewData["UcId"] = new SelectList(
                _context.Set<UC>(), "UcId", "Name", atribuicao?.UcId);

            ViewData["AcademicYearId"] = new SelectList(
                _context.Set<AcademicYear>(), "AcademicYearId", "AcademicYearId", atribuicao?.AcademicYearId);
        }
    }
}
