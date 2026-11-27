using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnivManagement.Data;
using UnivManagement.Models.AcademicManagement;

namespace UnivManagement.Controllers;

public class UCController : Controller {
    private readonly UniversityDbContext _context;

    public UCController(UniversityDbContext context) { _context = context; }


    // GET: UC?search=...&studyPlanId=2&semester=1&year=1
    public async Task<IActionResult> Index(string? search, int? studyPlanId, int? semester, int? year,
        string? successMessage = null, string? errorMessage = null) {

        if (successMessage != null) ViewBag.SuccessMessage = successMessage;
        if (errorMessage != null) ViewBag.ErrorMessage = errorMessage;

        var ucs = _context.UC
            .Include(u => u.StudyPlan)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search)) {
            search = search.Trim();
            ucs = ucs.Where(u => u.Code.Contains(search)
                || u.Name.Contains(search)
                || (u.Acronym != null && u.Acronym.Contains(search)));
        }

        // 0 means "UCs without a study plan"
        if (studyPlanId.HasValue) {
            ucs = studyPlanId.Value == 0
                ? ucs.Where(u => u.StudyPlanId == null)
                : ucs.Where(u => u.StudyPlanId == studyPlanId.Value);
        }

        if (semester.HasValue) {
            ucs = ucs.Where(u => u.Semester == semester.Value);
        }

        if (year.HasValue) {
            ucs = ucs.Where(u => u.Year == year.Value);
        }

        // Keep the filter values so the view can show them again
        ViewData["Search"] = search;
        ViewData["StudyPlanId"] = studyPlanId;
        ViewData["Semester"] = semester;
        ViewData["Year"] = year;
        ViewData["HasFilters"] = !string.IsNullOrWhiteSpace(search)
            || studyPlanId.HasValue
            || semester.HasValue
            || year.HasValue;

        await PopulateStudyPlanFilterAsync(studyPlanId);

        ViewData["Semesters"] = new SelectList(
            Enumerable.Range(1, 10).Select(n => new { Value = n, Label = $"{n}.º semestre" }),
            "Value", "Label", semester);

        ViewData["Years"] = new SelectList(
            Enumerable.Range(1, 5).Select(n => new { Value = n, Label = $"{n}.º ano" }),
            "Value", "Label", year);

        return View(await ucs.OrderBy(u => u.Name).ToListAsync());
    }

    // GET: UC/Details/5
    public async Task<IActionResult> Details(int? id, string? successMessage = null) {
        if (id == null) return NotFound();

        var uc = await _context.UC
            .Include(u => u.StudyPlan)
            .FirstOrDefaultAsync(m => m.UcId == id);

        if (uc == null) return View("NotFound");
        if (successMessage != null) ViewBag.SuccessMessage = successMessage;

        return View(uc);
    }

    // GET: UC/Create
    public async Task<IActionResult> Create() {
        await PopulateStudyPlansAsync();
        return View(new UC());
    }

    // POST: UC/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("UcId,Code,Name,Acronym,Description,ValueECTS,NumHours,StudyPlanId")] UC uc) {

        // Year and Semester belong to the StudyPlan editor; a standalone UC starts at 1/1.
        uc.Year = 1;
        uc.Semester = 1;

        // They are not part of this form, so discard the binding errors raised for the missing values.
        ModelState.Remove(nameof(UC.Year));
        ModelState.Remove(nameof(UC.Semester));

        await ValidateSelectionsAsync(uc);

        if (ModelState.IsValid) {
            _context.Add(uc);
            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = uc.UcId, successMessage = "Unidade Curricular criada com sucesso." }
            );
        }

        await PopulateStudyPlansAsync(uc.StudyPlanId);
        return View(uc);
    }

    // GET: UC/Edit/5
    public async Task<IActionResult> Edit(int? id) {
        if (id == null) return NotFound();

        var uc = await _context.UC.FindAsync(id);
        if (uc == null) return View("NotFound");

        await PopulateStudyPlansAsync(uc.StudyPlanId);
        return View(uc);
    }

    // POST: UC/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id,
        [Bind("UcId,Code,Name,Acronym,Description,ValueECTS,NumHours,StudyPlanId")] UC uc) {

        if (id != uc.UcId) return NotFound();

        var existing = await _context.UC.FindAsync(id);
        if (existing == null) return View("NotFound");

        // Year and Semester are managed by the StudyPlan editor; keep the stored values.
        uc.Year = existing.Year;
        uc.Semester = existing.Semester;

        // They are not part of this form, so discard the binding errors raised for the missing values.
        ModelState.Remove(nameof(UC.Year));
        ModelState.Remove(nameof(UC.Semester));

        await ValidateSelectionsAsync(uc);

        if (ModelState.IsValid) {
            try {
                // Copy only the fields exposed by the UC form; Year and Semester stay untouched.
                existing.Code = uc.Code;
                existing.Name = uc.Name;
                existing.Acronym = uc.Acronym;
                existing.Description = uc.Description;
                existing.ValueECTS = uc.ValueECTS;
                existing.NumHours = uc.NumHours;
                existing.StudyPlanId = uc.StudyPlanId;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!UCExists(uc.UcId)) return View("NotFound");
                else throw;
            }

            return RedirectToAction(
                nameof(Details),
                new { id = uc.UcId, successMessage = "Dados da UC atualizados com sucesso." }
            );
        }

        await PopulateStudyPlansAsync(uc.StudyPlanId);
        return View(uc);
    }

    // GET: UC/Delete/5
    public async Task<IActionResult> Delete(int? id) {
        if (id == null) return NotFound();

        var uc = await _context.UC
            .Include(u => u.StudyPlan)
            .FirstOrDefaultAsync(m => m.UcId == id);

        // Se o registo já tiver sido eliminado noutra janela
        if (uc == null) {
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Aviso: A Unidade Curricular já tinha sido eliminada por outro utilizador." });
        }

        return View(uc);
    }

    // POST: UC/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id) {
        var uc = await _context.UC.FindAsync(id);

        if (uc == null) {
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Aviso: A Unidade Curricular já tinha sido eliminada por outro utilizador." });
        }

        try {
            _context.UC.Remove(uc);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException) {
            // A UC tem inscrições (InscrUC) associadas (FK Restrict)
            return RedirectToAction(nameof(Index),
                new { errorMessage = "Não é possível eliminar esta unidade curricular porque existem inscrições associadas." });
        }

        return RedirectToAction(nameof(Index),
            new { successMessage = "Unidade Curricular eliminada com sucesso." });
    }

    private bool UCExists(int? id) {
        return _context.UC.Any(e => e.UcId == id);
    }

    /// <summary> Planos de estudos para a lista de seleção dos formulários. </summary>
    private async Task PopulateStudyPlansAsync(int? studyPlanId = null) {
        var studyPlans = await _context.StudyPlan
            .OrderBy(s => s.Code)
            .ToListAsync();

        ViewData["StudyPlans"] = new SelectList(studyPlans,
            nameof(StudyPlan.StudyPlanId), nameof(StudyPlan.Code), studyPlanId);
    }

    /// <summary> Opções do filtro por plano de estudos, incluindo "sem plano". </summary>
    private async Task PopulateStudyPlanFilterAsync(int? studyPlanId) {
        var studyPlans = await _context.StudyPlan
            .OrderBy(s => s.Code)
            .ToListAsync();

        var options = new List<SelectListItem> {
            new() { Value = "", Text = "Todos os planos", Selected = !studyPlanId.HasValue },
            new() { Value = "0", Text = "Sem plano de estudos", Selected = studyPlanId == 0 }
        };

        options.AddRange(studyPlans.Select(s => new SelectListItem {
            Value = s.StudyPlanId.ToString(),
            Text = s.Code,
            Selected = studyPlanId == s.StudyPlanId
        }));

        ViewData["StudyPlans"] = options;
    }

    /// <summary> Valida o plano de estudos escolhido (opcional) e a unicidade do código da UC. </summary>
    private async Task ValidateSelectionsAsync(UC uc) {
        // O plano de estudos é opcional; se for indicado, tem de existir
        if (uc.StudyPlanId.HasValue &&
            !await _context.StudyPlan.AnyAsync(s => s.StudyPlanId == uc.StudyPlanId)) {
            ModelState.AddModelError(nameof(UC.StudyPlanId), "Selecione um plano de estudos válido.");
        }

        if (await _context.UC.AnyAsync(u => u.Code == uc.Code && u.UcId != uc.UcId)) {
            ModelState.AddModelError(nameof(UC.Code), "Já existe uma unidade curricular com este código.");
        }
    }
}
