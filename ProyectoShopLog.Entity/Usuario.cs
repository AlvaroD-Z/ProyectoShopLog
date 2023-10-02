using System;
using System.Collections.Generic;

namespace ProyectoShopLog.Entity;

public partial class Usuario
{
    public Usuario()
    {
        Gastos = new HashSet<Gasto>();
    }
    public int UsuarioId { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public int? IdRol { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Historialcomentario> Historialcomentarios { get; set; } = new List<Historialcomentario>();

    public virtual Rol? IdRolNavigation { get; set; }
}
