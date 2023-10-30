using ProyectoShopLog.Entity;

namespace ProyectoShopLog.AplicacionWeb.Models.ViewModels
{
    public class VMGasto
    {
        public int? GastoId { get; set; }

        public int? UsuarioId { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public int? Monto { get; set; }

        public string? TipoMovimiento { get; set; }
        public int? CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }

        public DateTime? FechaDeIngreso { get; set; }

        public virtual Usuario? Usuario { get; set; }

        public static VMGasto From(Gasto gasto)
        {
            return new VMGasto
            {
                GastoId = gasto.GastoId,
                UsuarioId = gasto.UsuarioId,
                Nombre = gasto.Nombre,
                Descripcion = gasto.Descripcion,
                Monto = gasto.Monto,
                TipoMovimiento = gasto.TipoMovimiento,
                FechaDeIngreso = gasto.FechaDeIngreso,
                CategoriaId = gasto.Categoria?.CategoriaId,
                CategoriaNombre = gasto.Categoria?.Nombre
            };
        }
    }
}
