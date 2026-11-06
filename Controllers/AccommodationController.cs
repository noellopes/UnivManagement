using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class AccommodationController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/AccommodationManagement/Index.cshtml");
        }
    }
}
