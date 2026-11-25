using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;
using UnivManagement.Services;

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
                .Include(a => a.AnoLetivo)
                .OrderBy(a => a.DiaSemana)
                .ThenBy(a => a.HoraInicio);
            return View(await atendimentos.ToListAsync());
        }

        // POST: HorarioAtendimento/Gerar
        // Gera o atendimento de cada docente: 1/3 das horas de aula, num espaco livre
        // do docente e dos alunos dos cursos onde ele leciona. Repetir substitui o anterior.
        [HttpPost("HorarioAtendimento/Gerar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Gerar(int semestre = 1)
        {
            if (semestre != 1 && semestre != 2)
            {
                semestre = 1;
            }

            var ano = await _context.Set<AcademicYear>()
                .OrderByDescending(a => a.StartYear)
                .FirstOrDefaultAsync();
            var departamento = await _context.Set<Departamento>()
                .OrderBy(d => d.DepartamentoId)
                .FirstOrDefaultAsync();

            if (ano == null || departamento == null)
            {
                TempData["Erro"] = "Faltam dados de base: é preciso haver pelo menos um ano letivo e um departamento.";
                return RedirectToAction(nameof(Index));
            }

            // DE ONDE VEM O HORARIO DE AULAS: hoje exemplos.
            // Quando a Aula entrar no master, trocar so esta linha por outra IFonteAulas.
            IFonteAulas fonte = new FonteAulasExemplo(_context);
            var docentes = await fonte.ObterDocentesAsync(semestre);

            if (docentes.Count == 0)
            {
                TempData["Erro"] = "Não há docentes com aulas para gerar atendimento.";
                return RedirectToAction(nameof(Index));
            }

            var gerados = 0;
            var semEspaco = new List<string>();

            foreach (var d in docentes)
            {
                // repetir a geracao: apaga o atendimento anterior deste docente e semestre
                var antigos = _context.HorarioAtendimento.Where(a =>
                    a.DocenteId == d.DocenteId &&
                    a.AcademicYearId == ano.AcademicYearId &&
                    a.Semestre == semestre);
                _context.HorarioAtendimento.RemoveRange(antigos);

                var minutosAula = d.AulasDoDocente.Sum(a => (int)(a.Fim - a.Inicio).TotalMinutes);
                var ocupados = d.AulasDoDocente.Concat(d.AulasDosAlunos);
                var intervalo = GeradorAtendimento.ProcurarIntervalo(minutosAula, ocupados);

                if (intervalo == null)
                {
                    semEspaco.Add(d.Nome);
                    continue;
                }

                _context.HorarioAtendimento.Add(new HorarioAtendimento
                {
                    Semestre = semestre,
                    DiaSemana = intervalo.Dia,
                    HoraInicio = TimeOnly.FromTimeSpan(intervalo.Inicio),
                    HoraFim = TimeOnly.FromTimeSpan(intervalo.Fim),
                    Modalidade = "Presencial",
                    RequerMarcacao = false,
                    Estado = "Provisório",
                    Observacoes = "Gabinete: " + d.Gabinete,
                    DocenteId = d.DocenteId,
                    DepartamentoId = departamento.DepartamentoId,
                    AcademicYearId = ano.AcademicYearId
                });
                gerados++;
            }

            await _context.SaveChangesAsync();

            var mensagem = $"Atendimentos gerados: {gerados}.";
            if (semEspaco.Count > 0)
            {
                mensagem += " Sem espaço livre para: " + string.Join(", ", semEspaco) + ".";
            }

            TempData["Mensagem"] = mensagem;
            return RedirectToAction(nameof(Index));
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

        // Listas para os <select> (sem DbSet no contexto: usa-se Set<T>())
        private void CarregarListas(HorarioAtendimento? atendimento = null)
        {
            ViewData["DocenteId"] = new SelectList(
                _context.Set<Docente>(), "DocenteId", "Nome", atendimento?.DocenteId);

            ViewData["DepartamentoId"] = new SelectList(
                _context.Set<Departamento>(), "DepartamentoId", "Nome", atendimento?.DepartamentoId);

            ViewData["AcademicYearId"] = new SelectList(
                _context.Set<AcademicYear>()
                    .Select(a => new { a.AcademicYearId, Texto = a.StartYear + "/" + a.EndYear }),
                "AcademicYearId", "Texto", atendimento?.AcademicYearId);
        }
    }
}
