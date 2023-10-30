using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ProyectoShopLog.Entity;

public partial class Gasto
{
    public int? GastoId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Monto { get; set; }

    public DateTime? FechaDeIngreso { get; set; }

    public string? TipoMovimiento { get; set; }
    public int? CategoriaId { get; set; }

    public virtual Usuario? Usuario { get; set; }
    public virtual Categoria? Categoria { get; set; }
}
