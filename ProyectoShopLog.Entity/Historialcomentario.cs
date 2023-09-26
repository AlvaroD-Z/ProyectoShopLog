using System;
using System.Collections.Generic;

namespace ProyectoShopLog.Entity;

public partial class Historialcomentario
{
    public int ComentarioId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Comentario { get; set; }

    public int? Calificacion { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
