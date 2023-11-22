using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoShopLog.AplicacionWeb.Utilidades.CustomFilter;
using ProyectoShopLog.BLL.Interfaces;
using ProyectoShopLog.Entity;

namespace ProyectoShopLog.AplicacionWeb.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [ClaimRequirement(controlador: "Categoria", accion: "Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Categoria> categorias = await _categoriaService.Lista();

            return StatusCode(StatusCodes.Status200OK, new { data = categorias });
        }

        [HttpGet]
        public async Task<IActionResult> ListaByTipoMovimiento([FromQuery(Name = "tipoMovimiento")] string tipoMovimiento)
        {
            List<Categoria> categorias = await _categoriaService.ListaByTipoMovimiento(tipoMovimiento);

            return StatusCode(StatusCodes.Status200OK, new { data = categorias });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Categoria categoria)
        {
            try
            {
                Categoria categoriaCreado = await _categoriaService.Crear(categoria);

                return StatusCode(StatusCodes.Status200OK, new { categoriaId = categoriaCreado.CategoriaId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] Categoria categoria)
        {
            try
            {
                Categoria categoriaActualizada = await _categoriaService.Editar(categoria);

                return StatusCode(StatusCodes.Status200OK, new { categoriaId = categoriaActualizada.CategoriaId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { mensaje = ex.Message });
            }
        }
    }
}
