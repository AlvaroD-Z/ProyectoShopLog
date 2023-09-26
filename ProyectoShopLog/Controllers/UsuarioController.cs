using Microsoft.AspNetCore.Mvc;

namespace ProyectoShopLog.AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CambiarContra()
        {
            return View();
        }
    }
}
