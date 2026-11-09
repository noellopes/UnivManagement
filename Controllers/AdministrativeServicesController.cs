using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers {
    public class AdministrativeServicesController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
