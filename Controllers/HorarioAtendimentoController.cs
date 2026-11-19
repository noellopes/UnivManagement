using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Controllers
{
    public class HorarioAtendimentoController : Controller
    {
        private readonly UniversityDbContext _context;

        public HorarioAtendimentoController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: HorarioAtendimento
        [HttpGet("HorarioAtendimento")]
        [HttpGet("HorarioAtendimento/Index")]
        public async Task<IActionResult> Index()
        {
            var atendimentos = _context.HorarioAtendimento
                .Include(a => a.Docente)
                .Include(a => a.Departamento)
                .Include(a => a.AnoLetivo);
            return View(await atendimentos.ToListAsync());
        }

        // GET: HorarioAtendimento/Details/5
        [HttpGet("HorarioAtendimento/Details/{atendimentoid}")]
        public async Task<IActionResult> Details(int? atendimentoid)
        {
            if (atendimentoid == null)
            {
                return NotFound();
            }

            var horarioatendimento = await _context.HorarioAtendimento
                .Include(a => a.Docente)
                .Include(a => a.Departamento)
                .Include(a => a.AnoLetivo)
                .FirstOrDefaultAsync(m => m.AtendimentoId == atendimentoid);
            if (horarioatendimento == null)
            {
                return NotFound();
            }

            return View(horarioatendimento);
        }

        // GET: HorarioAtendimento/Create
        [HttpGet("HorarioAtendimento/Create")]
        public IActionResult Create()
        {
            CarregarListas();
            return View();
        }

        // POST: HorarioAtendimento/Create
        [HttpPost("HorarioAtendimento/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Semestre,DiaSemana,HoraInicio,HoraFim,Modalidade,RequerMarcacao,Estado,Observacoes,DocenteId,DepartamentoId,AcademicYearId")] HorarioAtendimento horarioatendimento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horarioatendimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarListas(horarioatendimento);
            return View(horarioatendimento);
        }

        // GET: HorarioAtendimento/Edit/5
        [HttpGet("HorarioAtendimento/Edit/{atendimentoid}")]
        public async Task<IActionResult> Edit(int? atendimentoid)
        {
            if (atendimentoid == null)
            {
                return NotFound();
            }

            var horarioatendimento = await _context.HorarioAtendimento.FindAsync(atendimentoid);
            if (horarioatendimento == null)
            {
                return NotFound();
            }

            CarregarListas(horarioatendimento);
            return View(horarioatendimento);
        }

        // POST: HorarioAtendimento/Edit/5
        [HttpPost("HorarioAtendimento/Edit/{atendimentoid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? atendimentoid, [Bind("AtendimentoId,Semestre,DiaSemana,HoraInicio,HoraFim,Modalidade,RequerMarcacao,Estado,Observacoes,DataCriacao,DocenteId,DepartamentoId,AcademicYearId")] HorarioAtendimento horarioatendimento)
        {
            if (atendimentoid != horarioatendimento.AtendimentoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horarioatendimento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioAtendimentoExists(horarioatendimento.AtendimentoId))
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

            CarregarListas(horarioatendimento);
            return View(horarioatendimento);
        }

        // GET: HorarioAtendimento/Delete?atendimentoid=5
        public async Task<IActionResult> Delete(int? atendimentoid)
        {
            if (atendimentoid == null)
            {
                return NotFound();
            }

            var horarioatendimento = await _context.HorarioAtendimento
                .Include(a => a.Docente)
                .Include(a => a.Departamento)
                .Include(a => a.AnoLetivo)
                .FirstOrDefaultAsync(m => m.AtendimentoId == atendimentoid);
            if (horarioatendimento == null)
            {
                return NotFound();
            }

            return View(horarioatendimento);
        }

        // POST: HorarioAtendimento/Delete?atendimentoid=5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? atendimentoid)
        {
            var horarioatendimento = await _context.HorarioAtendimento.FindAsync(atendimentoid);
            if (horarioatendimento != null)
            {
                _context.HorarioAtendimento.Remove(horarioatendimento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioAtendimentoExists(int? atendimentoid)
        {
            return _context.HorarioAtendimento.Any(e => e.AtendimentoId == atendimentoid);
        }


        private void CarregarListas(HorarioAtendimento? atendimento = null)
        {
            ViewData["DocenteId"] = new SelectList(
                _context.Set<Docente>(), "DocenteId", "Nome", atendimento?.DocenteId);

            ViewData["DepartamentoId"] = new SelectList(
                _context.Set<Departamento>(), "DepartamentoId", "Nome", atendimento?.DepartamentoId);

            ViewData["AcademicYearId"] = new SelectList(
                _context.Set<AcademicYear>(), "AcademicYearId", "AcademicYearId", atendimento?.AcademicYearId);
        }
    }
}