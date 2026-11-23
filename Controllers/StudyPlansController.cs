using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Controllers;

public class StudyPlansController : Controller {
    private readonly UniversityDbContext _context;

    public StudyPlansController(UniversityDbContext context) { _context = context; }

    // GET: StudyPlans?search=...
    public async Task<IActionResult> Index(string? search,
        string? successMessage = null, string? errorMessage = null) {

        if (successMessage != null) ViewBag.SuccessMessage = successMessage;
        if (errorMessage != null) ViewBag.ErrorMessage = errorMessage;

        var plans = _context.StudyPlan
            .Include(s => s.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search)) {
            search = search.Trim();
            plans = plans.Where(s => s.Code.Contains(search)
                || (s.Course != null && (s.Course.Name.Contains(search)
                    || (s.Course.Acronym != null && s.Course.Acronym.Contains(search)))));
        }

        ViewData["Search"] = search;
        ViewData["HasFilters"] = !string.IsNullOrWhiteSpace(search);

        return View(await plans.OrderBy(s => s.Code).ToListAsync());
    }

    // GET: StudyPlans/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null) {
        if (id == null) return NotFound();

        var plan = await _context.StudyPlan
            .Include(s => s.Course)
            .FirstOrDefaultAsync(s => s.StudyPlanId == id);

        if (plan == null) return View("NotFound");
        if (successMessage != null) ViewBag.SuccessMessage = successMessage;

        ViewData["Years"] = await _context.StudyPlanYear
            .Where(y => y.StudyPlanId == id)
            .OrderBy(y => y.NumYear)
            .ToListAsync();

        ViewData["Ucs"] = await _context.UC
            .Where(u => u.StudyPlanId == id)
            .OrderBy(u => u.Year)
            .ThenBy(u => u.Semester)
            .ThenBy(u => u.Code)
            .ToListAsync();

        return View(plan);
    }

    // GET: StudyPlans/Create
    public async Task<IActionResult> Create() {
        await PopulateListsAsync();

        var model = new StudyPlanFormModel {
            CreationDate = DateOnly.FromDateTime(DateTime.Today),
            StartVigor = 0,
            EndVigor = 0,
            Years = {
                new StudyPlanYearFormModel { ECTSTotalYear = 60, MinECTS = 0 }
            }
        };

        return View(model);
    }

    // POST: StudyPlans/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudyPlanFormModel model) {
        Normalize(model);
        await ValidateFormAsync(model);

        if (ModelState.IsValid) {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try {
                var plan = new StudyPlan {
                    Code = model.Code.Trim(),
                    CourseId = model.CourseId!.Value,
                    CreationDate = model.CreationDate!.Value,
                    StartVigor = model.StartVigor ?? 0,
                    EndVigor = model.EndVigor ?? 0,
                    NumYears = model.Years.Count,
                    EctsTotal = model.Years.Sum(y => y.ECTSTotalYear ?? 0)
                };

                _context.StudyPlan.Add(plan);
                await _context.SaveChangesAsync();

                for (var i = 0; i < model.Years.Count; i++) {
                    _context.StudyPlanYear.Add(new StudyPlanYear {
                        StudyPlanId = plan.StudyPlanId,
                        NumYear = i + 1,
                        ECTSTotalYear = model.Years[i].ECTSTotalYear ?? 0,
                        MinECTS = model.Years[i].MinECTS ?? 0
                    });
                }

                await _context.SaveChangesAsync();

                foreach (var row in model.Ucs.Where(u => u.UcId > 0)) {
                    var uc = await _context.UC.FirstOrDefaultAsync(u => u.UcId == row.UcId);
                    if (uc == null) continue; // Already reported by validation.

                    uc.StudyPlanId = plan.StudyPlanId;
                    uc.Year = row.Year!.Value;
                    uc.Semester = row.Semester!.Value;
                    uc.ValueECTS = row.ValueECTS!.Value;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Details), new {
                    id = plan.StudyPlanId,
                    successMessage = "Plano de estudos criado com sucesso."
                });
            }
            catch (DbUpdateException) {
                await transaction.RollbackAsync();
                _context.ChangeTracker.Clear();
                ModelState.AddModelError(string.Empty,
                    "Não foi possível criar o plano de estudos. Verifique os dados e tente novamente.");
            }
        }

        await PopulateListsAsync(model.CourseId, model.StudyPlanId,
            model.Ucs.Where(u => u.UcId > 0).Select(u => u.UcId));

        return View(model);
    }

    // GET: StudyPlans/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null) return NotFound();

        var plan = await _context.StudyPlan.FirstOrDefaultAsync(s => s.StudyPlanId == id);
        if (plan == null) return View("NotFound");

        var years = await _context.StudyPlanYear
            .Where(y => y.StudyPlanId == id)
            .OrderBy(y => y.NumYear)
            .ToListAsync();

        var ucs = await _context.UC
            .Where(u => u.StudyPlanId == id)
            .OrderBy(u => u.Year)
            .ThenBy(u => u.Semester)
            .ThenBy(u => u.Code)
            .ToListAsync();

        var model = new StudyPlanFormModel {
            StudyPlanId = plan.StudyPlanId,
            Code = plan.Code,
            CourseId = plan.CourseId,
            CreationDate = plan.CreationDate,
            StartVigor = plan.StartVigor,
            EndVigor = plan.EndVigor,
            Years = years.Select(y => new StudyPlanYearFormModel {
                StudyPlanYearId = y.StudyPlanYearId,
                ECTSTotalYear = y.ECTSTotalYear,
                MinECTS = y.MinECTS
            }).ToList(),
            Ucs = ucs.Select(u => new StudyPlanUcFormModel {
                UcId = u.UcId,
                Year = u.Year,
                Semester = u.Semester,
                ValueECTS = u.ValueECTS
            }).ToList(),
            OriginalYearIds = string.Join(",", years.Select(y => y.StudyPlanYearId)),
            OriginalUcIds = string.Join(",", ucs.Select(u => u.UcId))
        };

        await PopulateListsAsync(plan.CourseId, plan.StudyPlanId, ucs.Select(u => u.UcId));

        return View(model);
    }

    // POST: StudyPlans/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, StudyPlanFormModel model) {
        if (id == null) return NotFound();
        if (model.StudyPlanId != id) return NotFound();

        Normalize(model);
        await ValidateFormAsync(model);

        if (ModelState.IsValid) {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try {
                var plan = await _context.StudyPlan.FirstOrDefaultAsync(s => s.StudyPlanId == id);
                if (plan == null) return View("NotFound");

                plan.Code = model.Code.Trim();
                plan.CourseId = model.CourseId!.Value;
                plan.CreationDate = model.CreationDate!.Value;
                plan.StartVigor = model.StartVigor ?? 0;
                plan.EndVigor = model.EndVigor ?? 0;

                var existingYears = await _context.StudyPlanYear
                    .Where(y => y.StudyPlanId == id)
                    .ToListAsync();

                var postedYears = model.Years;
                var postedExistingIds = postedYears
                    .Where(y => y.StudyPlanYearId > 0)
                    .Select(y => y.StudyPlanYearId)
                    .ToHashSet();

                // Delete only the year rows that were removed in the editor.
                var removedYears = existingYears
                    .Where(y => !postedExistingIds.Contains(y.StudyPlanYearId))
                    .ToList();

                _context.StudyPlanYear.RemoveRange(removedYears);

                // Phase 1: move surviving rows out of the 1..N range so the
                // unique index (StudyPlanId, NumYear) cannot collide while
                // rows are reordered or renumbered.
                var survivors = existingYears
                    .Where(y => postedExistingIds.Contains(y.StudyPlanYearId))
                    .ToList();

                for (var i = 0; i < survivors.Count; i++) {
                    survivors[i].NumYear = 1000 + i;

                    var posted = postedYears.First(y => y.StudyPlanYearId == survivors[i].StudyPlanYearId);

                    survivors[i].ECTSTotalYear = posted.ECTSTotalYear ?? 0;
                    survivors[i].MinECTS = posted.MinECTS ?? 0;
                }

                await _context.SaveChangesAsync();

                // Phase 2: assign the final 1..N sequence and insert new cards.
                for (var i = 0; i < postedYears.Count; i++) {
                    var posted = postedYears[i];

                    if (posted.StudyPlanYearId > 0) {
                        var existing = survivors.First(y =>
                            y.StudyPlanYearId == posted.StudyPlanYearId);

                        existing.NumYear = i + 1;
                    }
                    else {
                        _context.StudyPlanYear.Add(new StudyPlanYear {
                            StudyPlanId = plan.StudyPlanId,
                            NumYear = i + 1,
                            ECTSTotalYear = posted.ECTSTotalYear ?? 0,
                            MinECTS = posted.MinECTS ?? 0
                        });
                    }
                }

                await _context.SaveChangesAsync();

                // Reconcile the complete desired UC list: update, attach, detach.
                var existingUcs = await _context.UC
                    .Where(u => u.StudyPlanId == id)
                    .ToListAsync();

                var desiredRows = model.Ucs.Where(u => u.UcId > 0).ToList();
                var desiredIds = desiredRows.Select(u => u.UcId).ToHashSet();

                foreach (var row in desiredRows) {
                    var uc = existingUcs.FirstOrDefault(u => u.UcId == row.UcId)
                        ?? await _context.UC.FirstOrDefaultAsync(u => u.UcId == row.UcId);

                    if (uc == null) continue; // Already reported by validation.

                    uc.StudyPlanId = id;
                    uc.Year = row.Year!.Value;
                    uc.Semester = row.Semester!.Value;
                    uc.ValueECTS = row.ValueECTS!.Value;
                }

                foreach (var uc in existingUcs.Where(u => !desiredIds.Contains(u.UcId))) {
                    uc.StudyPlanId = null;
                }

                plan.NumYears = postedYears.Count;
                plan.EctsTotal = postedYears.Sum(y => y.ECTSTotalYear ?? 0);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Details), new {
                    id = id,
                    successMessage = "Plano de estudos atualizado com sucesso."
                });
            }
            catch (DbUpdateException) {
                await transaction.RollbackAsync();
                _context.ChangeTracker.Clear();
                ModelState.AddModelError(string.Empty,
                    "Não foi possível guardar as alterações ao plano de estudos. Verifique os dados e tente novamente.");
            }
        }

        await PopulateListsAsync(model.CourseId, model.StudyPlanId,
            model.Ucs.Where(u => u.UcId > 0).Select(u => u.UcId));

        return View(model);
    }

    // GET: StudyPlans/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null) return NotFound();

        var plan = await _context.StudyPlan
            .Include(s => s.Course)
            .FirstOrDefaultAsync(s => s.StudyPlanId == id);

        if (plan == null) {
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Aviso: o plano de estudos já tinha sido eliminado ou não existe." });
        }

        ViewData["UcCount"] = await _context.UC.CountAsync(u => u.StudyPlanId == id);
        ViewData["EnrollmentCount"] = await _context.Enrollment.CountAsync(e => e.StudyPlanId == id);

        return View(plan);
    }

    // POST: StudyPlans/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id) {
        if (id == null) return NotFound();

        var plan = await _context.StudyPlan.FirstOrDefaultAsync(s => s.StudyPlanId == id);
        if (plan == null) {
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Aviso: o plano de estudos já tinha sido eliminado ou não existe." });
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try {
            // 1) Detach attributed UCs so they become available for another plan.
            var attributedUcs = await _context.UC
                .Where(u => u.StudyPlanId == id)
                .ToListAsync();

            foreach (var uc in attributedUcs) {
                uc.Year = 1;
                uc.Semester = 1;
                uc.StudyPlanId = null;
            }

            // 2) Remove the plan's year rows.
            var years = await _context.StudyPlanYear
                .Where(y => y.StudyPlanId == id)
                .ToListAsync();

            _context.StudyPlanYear.RemoveRange(years);

            // 3) Remove the plan itself.
            _context.StudyPlan.Remove(plan);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException) {
            await transaction.RollbackAsync();
            _context.ChangeTracker.Clear();
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Não foi possível eliminar este plano de estudos. Verifique se existem inscrições associadas." });
        }

        return RedirectToAction(nameof(Index),
            new { successMessage = "Plano de estudos eliminado. As unidades curriculares atribuídas ficaram novamente disponíveis." });
    }

    private static void Normalize(StudyPlanFormModel model) {
        model.Years ??= new List<StudyPlanYearFormModel>();
        model.Ucs ??= new List<StudyPlanUcFormModel>();
        model.Code = model.Code?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Validates keys, uniqueness, collection-level rules, UC availability,
    /// and the edit-time concurrency snapshots.
    /// </summary>
    private async Task ValidateFormAsync(StudyPlanFormModel model) {
        if (!model.CreationDate.HasValue) {
            ModelState.AddModelError(nameof(model.CreationDate),
                "A data de criação é obrigatória.");
        }

        if (!model.CourseId.HasValue
                || !await _context.Course.AnyAsync(c => c.CourseId == model.CourseId.Value)) {
            ModelState.AddModelError(nameof(model.CourseId),
                "Selecione um curso válido.");
        }

        if (await _context.StudyPlan.AnyAsync(s =>
                s.Code == model.Code && s.StudyPlanId != model.StudyPlanId)) {
            ModelState.AddModelError(nameof(model.Code),
                "Já existe um plano de estudos com este código.");
        }

        foreach (var error in StudyPlanFormValidator.ValidateStructure(model)) {
            ModelState.AddModelError(string.Empty, error);
        }

        if (model.StudyPlanId == 0 && model.Years.Any(y => y.StudyPlanYearId != 0)) {
            ModelState.AddModelError(string.Empty,
                "Os anos de um novo plano não podem referenciar anos já existentes.");
        }

        var selectedRows = model.Ucs.Where(u => u.UcId > 0).ToList();

        if (selectedRows.Count > 0) {
            var selectedIds = selectedRows.Select(u => u.UcId).Distinct().ToList();

            var selectedUcs = await _context.UC
                .Where(u => selectedIds.Contains(u.UcId))
                .Select(u => new { u.UcId, u.StudyPlanId })
                .ToListAsync();

            var foundIds = selectedUcs.Select(u => u.UcId).ToHashSet();

            if (selectedIds.Any(id => !foundIds.Contains(id))) {
                ModelState.AddModelError(string.Empty,
                    "Uma das unidades curriculares selecionadas não existe.");
            }

            if (selectedUcs.Any(u => u.StudyPlanId.HasValue
                    && u.StudyPlanId.Value != model.StudyPlanId)) {
                ModelState.AddModelError(string.Empty,
                    "Uma das unidades curriculares selecionadas já está associada a outro plano de estudos.");
            }
        }

        if (model.StudyPlanId > 0) {
            var postedYearIds = model.Years
                .Where(y => y.StudyPlanYearId > 0)
                .Select(y => y.StudyPlanYearId)
                .ToList();

            if (postedYearIds.GroupBy(y => y).Any(g => g.Count() > 1)) {
                ModelState.AddModelError(string.Empty,
                    "O mesmo ano foi enviado mais do que uma vez.");
            }

            if (postedYearIds.Count > 0) {
                var validIds = await _context.StudyPlanYear
                    .Where(y => y.StudyPlanId == model.StudyPlanId
                        && postedYearIds.Contains(y.StudyPlanYearId))
                    .Select(y => y.StudyPlanYearId)
                    .ToListAsync();

                if (validIds.Count != postedYearIds.Distinct().Count()) {
                    ModelState.AddModelError(string.Empty,
                        "Um dos anos do plano já não existe. Recarregue a página e tente novamente.");
                }
            }

            // Concurrency snapshots: no rowversion exists, so compare the ids
            // that were present when the page was loaded with the current set.
            var currentYearIds = await _context.StudyPlanYear
                .Where(y => y.StudyPlanId == model.StudyPlanId)
                .Select(y => y.StudyPlanYearId)
                .ToListAsync();

            if (!ParseIds(model.OriginalYearIds).SetEquals(currentYearIds)) {
                ModelState.AddModelError(string.Empty,
                    "O plano de estudos foi alterado por outro utilizador. Recarregue a página e tente novamente.");
            }

            var currentUcIds = await _context.UC
                .Where(u => u.StudyPlanId == model.StudyPlanId)
                .Select(u => u.UcId)
                .ToListAsync();

            if (!ParseIds(model.OriginalUcIds).SetEquals(currentUcIds)) {
                ModelState.AddModelError(string.Empty,
                    "As unidades curriculares do plano foram alteradas por outro utilizador. Recarregue a página e tente novamente.");
            }
        }
    }

    private static HashSet<int> ParseIds(string? value) {
        if (string.IsNullOrWhiteSpace(value)) return new HashSet<int>();

        return value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(part => int.TryParse(part, out var id) ? id : 0)
            .Where(id => id > 0)
            .ToHashSet();
    }

    /// <summary>
    /// Courses and the UC options offered by the tree editor. On edit, the
    /// plan's current UCs are included so they can be displayed even though
    /// they are already attributed.
    /// </summary>
    private async Task PopulateListsAsync(int? courseId = null, int? studyPlanId = null,
        IEnumerable<int>? includeUcIds = null) {

        var courses = await _context.Course
            .OrderBy(c => c.Name)
            .ToListAsync();

        ViewData["Courses"] = new SelectList(courses,
            nameof(Course.CourseId), nameof(Course.Name), courseId);

        var included = includeUcIds?
            .Where(id => id > 0)
            .ToHashSet() ?? new HashSet<int>();

        var ucsQuery = _context.UC.AsQueryable();

        if (studyPlanId.HasValue && studyPlanId.Value > 0) {
            ucsQuery = ucsQuery.Where(u => u.StudyPlanId == null
                || u.StudyPlanId == studyPlanId.Value
                || included.Contains(u.UcId));
        }
        else {
            ucsQuery = ucsQuery.Where(u => u.StudyPlanId == null
                || included.Contains(u.UcId));
        }

        ViewData["UcOptions"] = await ucsQuery
            .OrderBy(u => u.Code)
            .ToListAsync();
    }
}
