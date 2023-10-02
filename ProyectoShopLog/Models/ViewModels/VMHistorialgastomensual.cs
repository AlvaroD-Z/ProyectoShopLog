using ProyectoShopLog.Entity;

namespace ProyectoShopLog.AplicacionWeb.Models.ViewModels
{
    public class VMHistorialgastomensual
    {
        public int? UsuarioId { get; set; }

        public int? GastoMensual { get; set; }

        public DateTime? FechaFinMes { get; set; }

        public virtual Usuario? Usuario { get; set; }
    }
}
