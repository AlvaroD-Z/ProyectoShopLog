using System;
using System.Collections.Generic;

namespace ProyectoShopLog.Entity;

public partial class Historialgastomensual
{
    public int? UsuarioId { get; set; }

    public int? GastoMensual { get; set; }

    public DateTime? FechaFinMes { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
