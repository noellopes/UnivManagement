using Microsoft.AspNetCore.Mvc;

    public class CriarSalasController : Controller
    {
        public IActionResult CriarSalasIndex()
        {
            return View("~/Views/Scheduling_Group_2/CriarSalas/CriarSalas.cshtml");
        }
    }