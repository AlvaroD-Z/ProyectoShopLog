namespace ProyectoShopLog.Entity;

public class Categoria
{
    public int? CategoriaId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? TipoMovimiento { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}