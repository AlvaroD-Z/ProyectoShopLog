using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProyectoShopLog.AplicacionWeb.Models.ViewModels;
using ProyectoShopLog.BLL.Interfaces;

namespace ProyectoShopLog.AplicacionWeb.Controllers
{
    public class AdmiGastoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGastoService _gastoServicio;
        private readonly IRolService _rolServicio;
        public AdmiGastoController(IGastoService gastoServicio, IRolService rolServicio, IMapper mapper)
        {
            _mapper = mapper;
            _gastoServicio = gastoServicio;
            _rolServicio = rolServicio;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerHistGas()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMGasto> vmGastoLista = _mapper.Map<List<VMGasto>>(await _gastoServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmGastoLista });
        }
    }
}
