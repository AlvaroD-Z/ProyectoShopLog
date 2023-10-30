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
        private static readonly int MaxNumeroDecimales = 2;
        private readonly IGastoService _gastoServicio;
        private readonly IRolService _rolServicio;
        private readonly ILogger<AdmiGastoController> _logger;

        public AdmiGastoController(IGastoService gastoServicio, IRolService rolServicio, ILogger<AdmiGastoController> logger)
        {
            _gastoServicio = gastoServicio;
            _rolServicio = rolServicio;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerHistGas()
        {
            return View();
        }

        public IActionResult VerBalance()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> Eliminar([FromBody] VMEliminarMovimiento movimiento)
        {
            if (movimiento.GastoId == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { });
            }

            await _gastoServicio.Eliminar(movimiento.GastoId.Value);
            return StatusCode(StatusCodes.Status200OK, new { });
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Gasto> movimientos = await _gastoServicio.Lista();
            List<VMGasto> vmMovimientos = movimientos.Select(movimiento => VMGasto.From(movimiento)).ToList();
            return StatusCode(StatusCodes.Status200OK, new { data = vmMovimientos });
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] Gasto gasto)
        {
            try
            {
                Gasto gastoActualizado = await _gastoServicio.Editar(gasto);

                return StatusCode(StatusCodes.Status200OK, new { gastoId = gastoActualizado.GastoId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Gasto gasto)
        {
            try
            {
                Gasto gastoCreado = await _gastoServicio.Crear(gasto);

                return StatusCode(StatusCodes.Status200OK, new { gastoId = gastoCreado.GastoId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance(
            [FromQuery(Name = "fechaInicio")] string fechaInicioParam,
            [FromQuery(Name = "fechaFin")] string fechaFinParam)
        {
            _logger.LogInformation("get balance params: {} {}", fechaInicioParam, fechaFinParam);

            DateTime fechaInicio = DateTime.Parse(fechaInicioParam);
            DateTime fechaFin = DateTime.Parse(fechaFinParam);

            _logger.LogInformation("get balance parsed: {} {}", fechaInicio, fechaFin);

            Balance balance = await _gastoServicio.GetBalance(fechaInicio, fechaFin);

            return StatusCode(StatusCodes.Status200OK, new
            {
                ingresos = balance.Ingresos.Select(ingreso => VMGasto.From(ingreso)).ToList(),
                gastos = balance.Gastos.Select(gasto => VMGasto.From(gasto)).ToList(),
                totalIngresos = Math.Round(balance.TotalIngresos, MaxNumeroDecimales),
                totalGastos = Math.Round(balance.TotalGastos, MaxNumeroDecimales),
                saldoResultante = Math.Round(balance.SaldoResultante, MaxNumeroDecimales)
            });
        }
    }
}

