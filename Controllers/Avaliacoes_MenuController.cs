using Microsoft.AspNetCore.Mvc;

namespace UnivManagement.Controllers
{
    public class Avaliacoes_MenuController : Controller
    {
        // Ecrã de escolha de perfil (Docente ou Aluno)
        public IActionResult Index()
        {
            return View();
        }

        // Painel com funcionalidades de gestão do Docente
        public IActionResult PainelDocente()
        {
            return View();
        }

        // Painel com funcionalidades de consulta e submissão do Aluno
        public IActionResult PainelAluno()
        {
            return View();
        }
    }
}