using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoShopLog.Models;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace ProyectoShopLog.Controllers
{

    public class AccesoController : Controller
    {
        static string chain = "Data Source=(PIPER-PC\\SQLEXPRESS);Initial Catalog = DB_SHOPLOG; Integrated Security = true";



        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(Usuario oUsuario)
        {

            bool registrado;
            string mensaje;

            if (oUsuario.Clave == oUsuario.ConfirmarClave)
            {
                
            }
            else
            {
                ViewData["Mensaje"] = "las contraseñas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(chain))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario",cn);
                cmd.Parameters.AddWithValue("Correo",oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,35).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Login","Acceso");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Usuario oUsuario)
        {
            var account = oUsuario;

			using (SqlConnection cn = new SqlConnection(chain))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();
            }

            if (oUsuario.IdUsuario != 0)
            {
                HttpContext.Session.SetString("usuario", oUsuario.Correo);
                ViewData["Mensaje"] = "ENTRASTE";
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }
            

            
        }



		
	}
}
