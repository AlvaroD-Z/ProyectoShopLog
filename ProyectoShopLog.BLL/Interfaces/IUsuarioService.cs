﻿using ProyectoShopLog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoShopLog.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> Crear(Usuario entidad, Stream Foto = null, string NombreFoto = "",string UrlPlantillaCorreo = "");
        Task<Usuario> Editar(Usuario entidad, Stream Foto = null, string NombreFoto = "");
        Task<bool> Eliminar(int UsuarioId);
        Task<Usuario> ObtenerPorCredenciales(string correo, string clave);
        Task<Usuario> ObtenerPorId(int UsuarioId);
        Task<bool> GuardarPerfil(Usuario entidad);
        Task<bool> CambiarClave(int UsuarioId, string ClaveActual, string ClaveNueva);
        Task<bool> RestablecerClave(string Correo, string UrlPlantillaCorreo);
    }
}
