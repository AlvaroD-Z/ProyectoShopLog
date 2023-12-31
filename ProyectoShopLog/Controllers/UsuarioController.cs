﻿using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Newtonsoft.Json;
using ProyectoShopLog.AplicacionWeb.Models.ViewModels;
using ProyectoShopLog.AplicacionWeb.Utilidades.Response;
using ProyectoShopLog.BLL.Interfaces;
using ProyectoShopLog.Entity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoShopLog.Models;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

using AutoMapper;
using ProyectoShopLog.AplicacionWeb.Models.ViewModels;
using ProyectoShopLog.AplicacionWeb.Utilidades.Response;
using ProyectoShopLog.BLL.Interfaces;
using ProyectoShopLog.Entity;
using ProyectoShopLog.AplicacionWeb.Utilidades.CustomFilter;

namespace ProyectoShopLog.AplicacionWeb.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioServicio;
        private readonly IRolService _rolServicio;
        public UsuarioController(IUsuarioService usuarioServicio, IRolService rolServicio, IMapper mapper)
        {
            _mapper = mapper;
            _usuarioServicio = usuarioServicio;
            _rolServicio = rolServicio;
        }
        [ClaimRequirement(controlador: "Usuario", accion: "Index")]
        public IActionResult Index()
        {
            return View();
        }
        [ClaimRequirement(controlador: "Usuario", accion: "CambiarContra")]
        public IActionResult CambiarContra()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            List<VMRol> vmListaRoles = _mapper.Map<List<VMRol>>(await _rolServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, vmListaRoles);
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMUsuario> vmUsuarioLista = _mapper.Map<List<VMUsuario>>(await _usuarioServicio.Lista());
            return StatusCode(StatusCodes.Status200OK, new {data = vmUsuarioLista});
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                VMUsuario vmUsuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);

                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/EnviarClave?correo=[correo]&clave=[clave]";

                Usuario usuario_creado = await _usuarioServicio.Crear(_mapper.Map<Usuario>(vmUsuario), urlPlantillaCorreo);

                vmUsuario = _mapper.Map<VMUsuario>(usuario_creado);

                gResponse.Estado = true;
                Console.WriteLine(vmUsuario);
                gResponse.Objeto = vmUsuario;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar( [FromForm] string modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();

            try
            {
                VMUsuario vmUsuario = JsonConvert.DeserializeObject<VMUsuario>(modelo);

                Usuario usuario_editado = await _usuarioServicio.Editar(_mapper.Map<Usuario>(vmUsuario));

                vmUsuario = _mapper.Map<VMUsuario>(usuario_editado);

                gResponse.Estado = true;
                gResponse.Objeto = vmUsuario;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int UsuarioId)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _usuarioServicio.Eliminar(UsuarioId);
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        [HttpPost]
        public async Task<IActionResult> CambiarClave([FromBody] VMCambiarClave modelo)
        {
            GenericResponse<bool> response = new GenericResponse<bool>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;
                string idUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                bool resultado = await _usuarioServicio.CambiarClave(
                    int.Parse(idUsuario),
                    modelo.claveActual,
                    modelo.claveNueva
                    );

                response.Estado = resultado;
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

    }
}
