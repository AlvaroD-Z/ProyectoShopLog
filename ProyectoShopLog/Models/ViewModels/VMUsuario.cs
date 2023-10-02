using ProyectoShopLog.Entity;

namespace ProyectoShopLog.AplicacionWeb.Models.ViewModels
{
    public class VMUsuario
    {
        public int UsuarioId { get; set; }

        public string? Correo { get; set; }

        public string? Clave { get; set; }

        public int? IdRol { get; set; }

        public string? NombreRol { get; set; }

        
    }
}
