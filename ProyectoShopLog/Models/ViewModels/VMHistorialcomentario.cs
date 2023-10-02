using ProyectoShopLog.Entity;

namespace ProyectoShopLog.AplicacionWeb.Models.ViewModels
{
    public class VMHistorialcomentario
    {
        public int ComentarioId { get; set; }

        public int? UsuarioId { get; set; }

        public string? Comentario { get; set; }

        public int? Calificacion { get; set; }

        public virtual Usuario? Usuario { get; set; }
    }
}
