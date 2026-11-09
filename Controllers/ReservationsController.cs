using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Scheduling_Group_2/Reservations/Index.cshtml");
        }
    }
}
