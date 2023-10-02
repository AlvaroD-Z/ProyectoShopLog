using ProyectoShopLog.Entity;

namespace ProyectoShopLog.AplicacionWeb.Models.ViewModels
{
    public class VMGasto
    {
        public int GastoId { get; set; }

        public int? UsuarioId { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public int? Monto { get; set; }

        public DateTime? FechaDeIngreso { get; set; }

        public virtual Usuario? Usuario { get; set; }
    }
}
