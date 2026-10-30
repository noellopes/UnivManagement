using Microsoft.AspNetCore.Mvc;

public class HorariosController : Controller
{
    public IActionResult HorariosIndex()
    {
        // Correção: '~Views/...' alterado para '~/Views/...'
        return View("~/Views/Scheduling_Group_2/Horarios/Horarios.cshtml");
    }
}