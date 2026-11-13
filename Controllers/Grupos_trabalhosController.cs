using Microsoft.AspNetCore.Mvc;
using UnivManagement.Models;

namespace UnivManagement.Controllers
{
    public class Grupos_trabalhosController
    {
        public IActionResult CriarTrabalho()
        {
            return View(new CriarTrabalhoViewModel());
        }
    }
}
