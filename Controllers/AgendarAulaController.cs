using Microsoft.AspNetCore.Mvc;
{
    public class AgendarAulaController : Controller

    {

        public IActionResult Agendar_Aula()
        {
            return View("~/Views/Agendar_Aula.cshtml");
        }
    }
}
