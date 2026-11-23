using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Controllers;

public class CoursesController : Controller {
    private readonly UniversityDbContext _context;

    public CoursesController(UniversityDbContext context) { _context = context; }


    // GET: Courses?search=...&courseStateId=2&academicDegreeId=1
    public async Task<IActionResult> Index(string? search, int? courseStateId, int? academicDegreeId,
        string? successMessage = null, string? errorMessage = null) {

        if (successMessage != null) ViewBag.SuccessMessage = successMessage;
        if (errorMessage != null) ViewBag.ErrorMessage = errorMessage;

        var courses = _context.Course
            .Include(c => c.AcademicDegree)
            .Include(c => c.Docente)
            .Include(c => c.CourseState)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search)) {
            search = search.Trim();
            courses = courses.Where(c => c.Code.Contains(search)
                || c.Name.Contains(search)
                || c.Acronym.Contains(search));
        }

        if (academicDegreeId.HasValue) {
            courses = courses.Where(c => c.AcademicDegreeId == academicDegreeId.Value);
        }

        if (courseStateId.HasValue) {
            courses = courses.Where(c => c.CourseStateId == courseStateId.Value);
        }

        // Keep the filter values so the view can show them again
        ViewData["Search"] = search;
        ViewData["HasFilters"] = !string.IsNullOrWhiteSpace(search)
            || academicDegreeId.HasValue
            || courseStateId.HasValue;

        await PopulateDegreeAndStateListsAsync(academicDegreeId, courseStateId);

        return View(await courses.OrderBy(c => c.Name).ToListAsync());
    }

    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null) {
        if (id == null) return NotFound();

        var course = await _context.Course
            .Include(c => c.AcademicDegree)
            .Include(c => c.Docente)
            .Include(c => c.CourseState)
            .FirstOrDefaultAsync(m => m.CourseId == id);

        if (course == null) return View("NotFound");
        if (successMessage != null) ViewBag.SuccessMessage = successMessage;

        return View(course);
    }

    // GET: Courses/Create
    public async Task<IActionResult> Create() {
        await PopulateListsAsync();
        return View(new Course { CreationDate = DateOnly.FromDateTime(DateTime.Today) });
    }

    // POST: Courses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("CourseId,Code,Name,Acronym,CreationDate,Description,AcademicDegreeId,DocenteId,CourseStateId")] Course course) {
        await ValidateSelectionsAsync(course);

        if (ModelState.IsValid) {
            _context.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = course.CourseId, successMessage = "Curso criado com sucesso." }
            );
        }

        await PopulateListsAsync(course.AcademicDegreeId, course.DocenteId, course.CourseStateId);
        return View(course);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null) return NotFound();

        var course = await _context.Course.FindAsync(id);
        if (course == null) return View("NotFound");

        await PopulateListsAsync(course.AcademicDegreeId, course.DocenteId, course.CourseStateId);
        return View(course);
    }

    // POST: Courses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id,
        [Bind("CourseId,Code,Name,Acronym,CreationDate,Description,AcademicDegreeId,DocenteId,CourseStateId")] Course course) {
        if (id != course.CourseId) return NotFound();

        await ValidateSelectionsAsync(course);

        if (ModelState.IsValid) {
            try {
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!CourseExists(course.CourseId)) return View("NotFound");
                else throw;
            }

            return RedirectToAction(
                nameof(Details),
                new { id = course.CourseId, successMessage = "Dados do curso atualizados com sucesso." }
            );
        }

        await PopulateListsAsync(course.AcademicDegreeId, course.DocenteId, course.CourseStateId);
        return View(course);
    }

    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null) return NotFound();

        var course = await _context.Course
            .Include(c => c.AcademicDegree)
            .Include(c => c.Docente)
            .Include(c => c.CourseState)
            .FirstOrDefaultAsync(m => m.CourseId == id);

        // Se o registo já tiver sido eliminado noutra janela
        if (course == null) {
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Aviso: O curso já tinha sido eliminado por outro utilizador." });
        }

        return View(course);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id) {
        var course = await _context.Course.FindAsync(id);

        if (course == null) {
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Aviso: O curso já tinha sido eliminado por outro utilizador." });
        }

        try {
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException) {
            // O curso tem planos de estudo ou calendários associados (FK Restrict)
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Não é possível eliminar este curso porque existem planos de estudo ou calendários associados." });
        }

        return RedirectToAction(nameof(Index), new { successMessage = "Curso eliminado com sucesso." });
    }

    private bool CourseExists(int? id) {
        return _context.Course.Any(e => e.CourseId == id);
    }

    /// <summary> Graus Académicos, Docentes e Estados para as listas de seleção dos formulários. </summary>
    private async Task PopulateListsAsync(int? academicDegreeId = null, int? docenteId = null, int? courseStateId = null) {
        await PopulateDegreeAndStateListsAsync(academicDegreeId, courseStateId);

        var docentes = await _context.Set<Docente>()
            .OrderBy(d => d.Nome)
            .ToListAsync();

        ViewData["Docentes"] = new SelectList(docentes, nameof(Docente.DocenteId), nameof(Docente.Nome), docenteId);
    }

    /// <summary> Graus académicos e estados para os filtros e para os formulários. </summary>
    private async Task PopulateDegreeAndStateListsAsync(int? academicDegreeId = null, int? courseStateId = null) {
        var academicDegrees = await _context.AcademicDegree
            .OrderBy(a => a.Name)
            .ToListAsync();

        ViewData["AcademicDegrees"] = new SelectList(academicDegrees,
            nameof(AcademicDegree.AcademicDegreeId), nameof(AcademicDegree.Name), academicDegreeId);

        var courseStates = await _context.CourseState
            .OrderBy(s => s.Name)
            .ToListAsync();

        ViewData["CourseStates"] = new SelectList(courseStates,
            nameof(CourseState.CourseStateId), nameof(CourseState.Name), courseStateId);
    }

    /// <summary> Valida as chaves estrangeiras escolhidas e a unicidade do código do curso. </summary>
    private async Task ValidateSelectionsAsync(Course course) {
        if (course.CreationDate == default) {
            ModelState.AddModelError(nameof(Course.CreationDate), "A data de criação é obrigatória.");
        }

        if (!await _context.AcademicDegree.AnyAsync(a => a.AcademicDegreeId == course.AcademicDegreeId)) {
            ModelState.AddModelError(nameof(Course.AcademicDegreeId), "Selecione um grau académico válido.");
        }

        // O diretor do curso é opcional; se for indicado, tem de existir
        if (course.DocenteId.HasValue &&
            !await _context.Set<Docente>().AnyAsync(d => d.DocenteId == course.DocenteId)) {
            ModelState.AddModelError(nameof(Course.DocenteId), "Selecione um docente válido.");
        }

        if (!await _context.CourseState.AnyAsync(s => s.CourseStateId == course.CourseStateId)) {
            ModelState.AddModelError(nameof(Course.CourseStateId), "Selecione um estado de curso válido.");
        }

        if (await _context.Course.AnyAsync(c => c.Code == course.Code && c.CourseId != course.CourseId)) {
            ModelState.AddModelError(nameof(Course.Code), "Já existe um curso com este código.");
        }
    }
}
