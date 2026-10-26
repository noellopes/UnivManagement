using Microsoft.AspNetCore.Mvc;
namespace UnivManagement.Controllers.Scheduling_Group_2.Agendar_Aula
{
    public class AgendarAulaController : Controller
    {

        public IActionResult Agendar_Aula()
        {
            return View("~/Views/Scheduling_Group_2/Agendar_Aula/Agendar_Aula.cshtml");
        }
    }
}
