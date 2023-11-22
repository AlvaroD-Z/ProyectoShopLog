using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoShopLog.AplicacionWeb.Controllers
{
    [Authorize]
    public class CalificarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VerHisCal()
        {
            return View();
        }
    }
}
