using System;
using System.Collections.Generic;

namespace ProyectoShopLog.Entity;

public partial class Limitegastomensual
{
    public int? UsuarioId { get; set; }

    public int? LimiteGastoMensual1 { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
