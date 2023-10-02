using ProyectoShopLog.Entity;

namespace ProyectoShopLog.AplicacionWeb.Models.ViewModels
{
    public class VMLimitegastomensual
    {
        public int? UsuarioId { get; set; }

        public int? LimiteGastoMensual1 { get; set; }

        public virtual Usuario? Usuario { get; set; }
    }
}
