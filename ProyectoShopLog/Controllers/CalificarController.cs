using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoShopLog.AplicacionWeb.Models.ViewModels;
using ProyectoShopLog.AplicacionWeb.Utilidades.CustomFilter;

namespace ProyectoShopLog.AplicacionWeb.Controllers
{
    [Authorize]
    public class CalificarController : Controller
    {
        private readonly IMapper _mapper;
        [ClaimRequirement(controlador: "Calificar", accion: "Index")]
        public IActionResult Index()
        {
            return View();
        }
        [ClaimRequirement(controlador: "Calificar", accion: "VerHisCal")]
        public IActionResult VerHisCal()
        {
            return View();
        }
    }
}
