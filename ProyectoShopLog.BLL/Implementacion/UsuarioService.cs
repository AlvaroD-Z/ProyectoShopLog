using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Net;
using ProyectoShopLog.BLL.Interfaces;
using ProyectoShopLog.DAL.Interfaces;
using ProyectoShopLog.Entity;
using Azure;
using System.Diagnostics;

namespace ProyectoShopLog.BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IUtilidadesService _utilidadesService;
        private readonly ICorreoService _correoService;

        public UsuarioService(
            IGenericRepository<Usuario> repositorio,
            IUtilidadesService utilidadesService,
            ICorreoService correoService
            )
        {
            _repositorio = repositorio;
            _utilidadesService = utilidadesService;
            _correoService = correoService;
        }
        public async Task<List<Usuario>> Lista()
        {
            IQueryable<Usuario> query = await _repositorio.Consultar();
            return query.Include(r => r.IdRolNavigation).ToList();
        }
        public async Task<Usuario> Crear(Usuario entidad, string UrlPlantillaCorreo = "")
        {
            Usuario usuario_existe = await _repositorio.Obtener(u => u.Correo == entidad.Correo);
            if(usuario_existe != null)
            {
                throw new TaskCanceledException("El correo ya existe");
            }

            try
            {
                string clave_generada = _utilidadesService.GenerarClave();
                entidad.Clave = _utilidadesService.ConvertirSha256(clave_generada);

                Usuario usuario_creado = await _repositorio.Crear(entidad);

                if(usuario_creado.UsuarioId == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el usuario");
                }
                
                if(UrlPlantillaCorreo != "")
                {
                    UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[correo]", usuario_creado.Correo).Replace("[clave]", clave_generada);

                    string htmlCorreo = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            StreamReader readerStream = null;
                            if(response.CharacterSet == null)
                            {
                                readerStream = new StreamReader(dataStream);
                            }
                            else
                            {
                                readerStream = new StreamReader(dataStream,Encoding.GetEncoding(response.CharacterSet));
                            }
                            htmlCorreo = readerStream.ReadToEnd();
                            response.Close();
                            readerStream.Close();
                        }
                    }
                    if (htmlCorreo != "")
                    {
                        await _correoService.EnviarCorreo(usuario_creado.Correo,"Cuenta Creada",htmlCorreo);
                    }
                }
                IQueryable<Usuario> query = await _repositorio.Consultar(u => u.UsuarioId == usuario_creado.UsuarioId);
                usuario_creado = query.Include(r => r.IdRolNavigation).First();

                return usuario_creado;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Usuario> Editar(Usuario entidad)
        {
 
            try
            {
                IQueryable<Usuario> queryUsuario = await _repositorio.Consultar(u => u.UsuarioId == entidad.UsuarioId);
                Usuario usuario_editar = queryUsuario.First();
                usuario_editar.Correo = entidad.Correo;
                usuario_editar.IdRol = entidad.IdRol;
                bool respuesta = await _repositorio.Editar(usuario_editar);

                if (respuesta)
                {
                    throw new TaskCanceledException("No se pudo modificar el usuario");
                }

                Usuario usuario_editado = queryUsuario.First();

                return usuario_editado;
            }
            catch
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int UsuarioId)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.UsuarioId == UsuarioId);

                if(usuario_encontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }
                bool respuesta = await _repositorio.Eliminar(usuario_encontrado);

                return true;
            }
            catch
            {

                throw;
            }
        }
        public async Task<Usuario> ObtenerPorCredenciales(string correo, string clave)
        {
            string clave_encriptada = _utilidadesService.ConvertirSha256(clave);

            Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Correo.Equals(correo) && u.Clave.Equals(clave_encriptada));
            return usuario_encontrado;
        }
        public async Task<Usuario> ObtenerPorId(int UsuarioId)
        {
            IQueryable<Usuario> query = await _repositorio.Consultar(u => u.UsuarioId == UsuarioId);

            Usuario resultado = query.Include(r => r.IdRolNavigation).FirstOrDefault();
            return resultado;
        }
        public async Task<bool> GuardarPerfil(Usuario entidad)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.UsuarioId == entidad.UsuarioId);
                if(usuario_encontrado == null)
                {
                    throw new TaskCanceledException("Usuario no existe");
                }
                usuario_encontrado.Correo = entidad.Correo;
                bool respuesta = await _repositorio.Editar(usuario_encontrado);
                return respuesta;
            }
            catch 
            {

                throw;
            }
        }
        public async Task<bool> CambiarClave(int UsuarioId, string ClaveActual, string ClaveNueva)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.UsuarioId == UsuarioId);
                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("Usuario no existe");
                }

                if (usuario_encontrado.Clave != _utilidadesService.ConvertirSha256(ClaveActual))
                {
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");
                }

                usuario_encontrado.Clave = _utilidadesService.ConvertirSha256(ClaveNueva);
                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch
            {

                throw;
            }
        }

        public async Task<bool> RestablecerClave(string Correo, string UrlPlantillaCorreo)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.Obtener(u => u.Correo == Correo);

                if (usuario_encontrado == null)
                {
                    throw new TaskCanceledException("No encontramos ningun usuario asociado al correo");
                }

                string clave_generada = _utilidadesService.GenerarClave();
                usuario_encontrado.Clave = _utilidadesService.ConvertirSha256(clave_generada);

                UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[clave]", clave_generada);

                string htmlCorreo = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;
                        if (response.CharacterSet == null)
                        {
                            readerStream = new StreamReader(dataStream);
                        }
                        else
                        {
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));
                        }
                        htmlCorreo = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();
                    }
                }

                bool correo_enviado = false;
                if (htmlCorreo != "")
                {
                    await _correoService.EnviarCorreo(Correo, "Contraseña restablecida", htmlCorreo);
                }

                if (!correo_enviado)
                {
                    throw new TaskCanceledException("Tenemos problemas. Por favor intentalo de nuevo mas tarde");
                }

                bool respuesta = await _repositorio.Editar(usuario_encontrado);

                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
