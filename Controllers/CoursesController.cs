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


    // GET: Courses?search=...&courseState=Active&academicDegreeId=1
    public async Task<IActionResult> Index(string? search, CourseStateEnum? courseState, int? academicDegreeId,
        string? successMessage = null, string? errorMessage = null) {

        if (successMessage != null) ViewBag.SuccessMessage = successMessage;
        if (errorMessage != null) ViewBag.ErrorMessage = errorMessage;

        var courses = _context.Course
            .Include(c => c.AcademicDegree)
            .Include(c => c.Docente)
            .AsQueryable();

        bool hasFilters = !string.IsNullOrWhiteSpace(search)
            || courseState.HasValue
            || academicDegreeId.HasValue;

        // Default show Active Course States:
        if (!hasFilters) {
            courses = courses.Where(c => c.CourseState == CourseStateEnum.Active);
        }
        // Apply Filters:
        else {
            // Name/Code/Acronym Filter:
            if (!string.IsNullOrWhiteSpace(search)) {
                search = search.Trim();
                courses = courses.Where(c => c.Code.Contains(search)
                    || c.Name.Contains(search)
                    || c.Acronym.Contains(search));
            }
            // Academic Degree Filter:
            if (academicDegreeId.HasValue)
                courses = courses.Where(c => c.AcademicDegreeId == academicDegreeId.Value);
            // Course State Filter:
            if (courseState.HasValue)
                courses = courses.Where(c => c.CourseState == courseState.Value);
        }

        // Keep the filter values so the view can show them again:
        ViewData["Search"] = search;
        ViewData["HasFilters"] = hasFilters;
        ViewData["CourseState"] = courseState;

        await PopulateAcademicDegreeListAsync(academicDegreeId);

        return View(await courses.OrderBy(c => c.Name).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id, string? successMessage = null) {
        if (id == null) return NotFound();

        var course = await _context.Course
            .Include(c => c.AcademicDegree)
            .Include(c => c.Docente)
            .FirstOrDefaultAsync(m => m.CourseId == id);

        if (course == null) return View("NotFound");
        if (successMessage != null) ViewBag.SuccessMessage = successMessage;

        await PopulateCourseCalendarsAsync(id);

        return View(course);
    }

    private async Task PopulateCourseCalendarsAsync(int? id) {
        ViewData["CourseCalendars"] = await _context.CourseCalendar
            .Where(cc => cc.CourseId == id)
            .Include(cc => cc.SchoolCalendar)
            .ThenInclude(sc => sc!.AcademicYear)
            .OrderByDescending(cc => cc.SchoolCalendar!.AcademicYear!.StartYear)
            .ThenByDescending(cc => cc.SchoolCalendar!.StartDate)
            .ToListAsync();
    }

    /// <summary>
    /// Loads the calendars available in the Course Edit page and the ids
    /// currently associated with the course. On a failed post, the posted ids
    /// are used instead of the database state so the user's edits are kept.
    /// </summary>
    private async Task PopulateCourseCalendarEditorAsync(int courseId, IEnumerable<int>? selectedIds = null) {
        var calendars = await _context.SchoolCalendar
            .Include(sc => sc.AcademicYear)
            .OrderByDescending(sc => sc.AcademicYear!.StartYear)
            .ThenBy(sc => sc.Code)
            .ToListAsync();

        var selected = selectedIds != null
            ? selectedIds.Where(id => id > 0).Distinct().ToList()
            : await _context.CourseCalendar
                .Where(cc => cc.CourseId == courseId)
                .Select(cc => cc.SchoolCalendarId)
                .Distinct()
                .ToListAsync();

        // Invalid ids are reported by validation; don't render a row without an option.
        var validIds = calendars.Select(c => c.SchoolCalendarId).ToHashSet();
        selected = selected.Where(validIds.Contains).ToList();

        ViewData["SchoolCalendars"] = calendars;
        ViewData["SelectedSchoolCalendarIds"] = selected;
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
        [Bind("CourseId,Code,Name,Acronym,CreationDate,Description,AcademicDegreeId,DocenteId,CourseState")] Course course) {
        await ValidateSelectionsAsync(course);

        if (ModelState.IsValid) {
            _context.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = course.CourseId, successMessage = "Curso criado com sucesso." }
            );
        }

        await PopulateListsAsync(course.AcademicDegreeId, course.DocenteId);
        return View(course);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null) return NotFound();

        var course = await _context.Course.FindAsync(id);
        if (course == null) return View("NotFound");

        await PopulateListsAsync(course.AcademicDegreeId, course.DocenteId);
        await PopulateCourseCalendarEditorAsync(course.CourseId);
        return View(course);
    }

    // POST: Courses/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id,
        [Bind("CourseId,Code,Name,Acronym,CreationDate,Description,AcademicDegreeId,DocenteId,CourseState")] Course course,
        List<int?>? schoolCalendarIds) {

        if (id != course.CourseId) return NotFound();

        // Empty <select> values bind to null; keep only real ids and remove duplicates.
        var selectedCalendarIds = (schoolCalendarIds ?? new List<int?>())
            .Where(value => value.HasValue && value.Value > 0)
            .Select(value => value!.Value)
            .Distinct()
            .ToList();

        await ValidateSelectionsAsync(course);
        await ValidateCourseCalendarsAsync(selectedCalendarIds);

        if (ModelState.IsValid) {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try {
                _context.Update(course);
                await ReconcileCourseCalendarsAsync(course.CourseId, selectedCalendarIds);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(
                    nameof(Details),
                    new { id = course.CourseId, successMessage = "Dados do curso atualizados com sucesso." }
                );
            }
            catch (DbUpdateConcurrencyException) {
                await transaction.RollbackAsync();
                _context.ChangeTracker.Clear();

                if (!CourseExists(course.CourseId)) return View("NotFound");
                throw;
            }
            catch (DbUpdateException) {
                await transaction.RollbackAsync();
                _context.ChangeTracker.Clear();
                ModelState.AddModelError(string.Empty,
                    "Não foi possível guardar as alterações ao curso. Verifique os dados e tente novamente.");
            }
        }

        await PopulateListsAsync(course.AcademicDegreeId, course.DocenteId);
        await PopulateCourseCalendarEditorAsync(course.CourseId, selectedCalendarIds);
        return View(course);
    }

    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null) return NotFound();

        var course = await _context.Course
            .Include(c => c.AcademicDegree)
            .Include(c => c.Docente)
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
            // Only StudyPlans block course deletion; CourseCalendar rows cascade.
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Não é possível eliminar este curso porque existem planos de estudo associados." });
        }

        return RedirectToAction(nameof(Index), new { successMessage = "Curso eliminado com sucesso." });
    }

    private bool CourseExists(int? id) {
        return _context.Course.Any(e => e.CourseId == id);
    }

    /// <summary> Graus Académicos para os filtros e para os formulários. </summary>
    private async Task PopulateAcademicDegreeListAsync(int? academicDegreeId = null) {
        var academicDegrees = await _context.AcademicDegree
            .OrderBy(a => a.Name)
            .ToListAsync();

        ViewData["AcademicDegrees"] = new SelectList(academicDegrees,
            nameof(AcademicDegree.AcademicDegreeId), nameof(AcademicDegree.Name), academicDegreeId);
    }

    /// <summary> Graus Académicos e Docentes para as listas de seleção dos formulários. </summary>
    private async Task PopulateListsAsync(int? academicDegreeId = null, int? docenteId = null) {
        await PopulateAcademicDegreeListAsync(academicDegreeId);

        var docentes = await _context.Set<Docente>()
            .OrderBy(d => d.Nome)
            .ToListAsync();

        ViewData["Docentes"] = new SelectList(docentes, nameof(Docente.DocenteId), nameof(Docente.Nome), docenteId);
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

        if (await _context.Course.AnyAsync(c => c.Code == course.Code && c.CourseId != course.CourseId)) {
            ModelState.AddModelError(nameof(Course.Code), "Já existe um curso com este código.");
        }
    }

    /// <summary>
    /// Validates the calendar associations posted by the Course Edit page:
    /// each calendar must exist and a course cannot use two calendars from the
    /// same academic year. The academic year is reached through SchoolCalendar.
    /// </summary>
    private async Task ValidateCourseCalendarsAsync(List<int> schoolCalendarIds) {
        if (schoolCalendarIds.Count == 0) return;

        var calendars = await _context.SchoolCalendar
            .Where(sc => schoolCalendarIds.Contains(sc.SchoolCalendarId))
            .Select(sc => new { sc.SchoolCalendarId, sc.AcademicYearId })
            .ToListAsync();

        if (calendars.Count != schoolCalendarIds.Count) {
            ModelState.AddModelError(string.Empty,
                "Um dos calendários escolares selecionados não existe.");
            return;
        }

        if (calendars.GroupBy(sc => sc.AcademicYearId).Any(group => group.Count() > 1)) {
            ModelState.AddModelError(string.Empty,
                "Não é possível associar mais do que um calendário escolar do mesmo ano letivo.");
        }
    }

    /// <summary>
    /// Reconciles the course's calendar links with the ids posted by the editor.
    /// Removed rows delete only the association; the SchoolCalendar itself is kept.
    /// </summary>
    private async Task ReconcileCourseCalendarsAsync(int courseId, List<int> desiredIds) {
        var existing = await _context.CourseCalendar
            .Where(cc => cc.CourseId == courseId)
            .ToListAsync();

        var desired = desiredIds.ToHashSet();
        var existingIds = existing.Select(cc => cc.SchoolCalendarId).ToHashSet();

        _context.CourseCalendar.RemoveRange(
            existing.Where(cc => !desired.Contains(cc.SchoolCalendarId)));

        foreach (var calendarId in desired.Where(id => !existingIds.Contains(id))) {
            _context.CourseCalendar.Add(new CourseCalendar {
                CourseId = courseId,
                SchoolCalendarId = calendarId
            });
        }
    }
}
