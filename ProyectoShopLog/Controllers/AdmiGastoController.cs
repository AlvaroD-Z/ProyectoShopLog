using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoShopLog.AplicacionWeb.Models.ViewModels;
using ProyectoShopLog.AplicacionWeb.Utilidades.Response;
using ProyectoShopLog.BLL.Interfaces;
using ProyectoShopLog.Entity;

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

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] string modelo)
        {
            GenericResponse<VMGasto> gResponse = new GenericResponse<VMGasto>();

            try
            {
                VMGasto vmGasto = JsonConvert.DeserializeObject<VMGasto>(modelo);

                Gasto gasto_creado = await _gastoServicio.Crear(_mapper.Map<Gasto>(vmGasto));

                vmGasto = _mapper.Map<VMGasto>(gasto_creado);

                gResponse.Estado = true;
                Console.WriteLine(vmGasto);
                gResponse.Objeto = vmGasto;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

    }
}
