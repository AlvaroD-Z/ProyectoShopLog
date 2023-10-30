using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoShopLog.Entity;

namespace ProyectoShopLog.DAL.DBContext;

public partial class DbShoplogContext : DbContext
{
    public DbShoplogContext()
    {
    }

    public DbShoplogContext(DbContextOptions<DbShoplogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Historialcomentario> Historialcomentarios { get; set; }

    public virtual DbSet<Historialgastomensual> Historialgastomensuals { get; set; }

    public virtual DbSet<Limitegastomensual> Limitegastomensuals { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolMenu> RolMenus { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Configuracion");

            entity.Property(e => e.Propiedad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("propiedad");
            entity.Property(e => e.Recurso)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("recurso");
            entity.Property(e => e.Valor)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("valor");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.GastoId).HasName("PK__GASTO__815BB0F08B3ED8E3");

            entity.ToTable("GASTO");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.FechaDeIngreso).HasColumnType("date");
            entity.Property(e => e.Nombre)
                .HasMaxLength(35)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__GASTO__UsuarioId__656C112C");

            entity.HasOne(d => d.Categoria)
                .WithMany(p => p.Gastos)
                .HasForeignKey(d => d.CategoriaId);
        });

        modelBuilder.Entity<Historialcomentario>(entity =>
        {
            entity.HasKey(e => e.ComentarioId).HasName("PK__HISTORIA__F1844938F606B2EB");

            entity.ToTable("HISTORIALCOMENTARIOS");

            entity.Property(e => e.Comentario)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Historialcomentarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__HISTORIAL__Usuar__797309D9");
        });

        modelBuilder.Entity<Historialgastomensual>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HISTORIALGASTOMENSUAL");

            entity.Property(e => e.FechaFinMes).HasColumnType("date");

            entity.HasOne(d => d.Usuario).WithMany()
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__HISTORIAL__Usuar__693CA210");
        });

        modelBuilder.Entity<Limitegastomensual>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LIMITEGASTOMENSUAL");

            entity.Property(e => e.LimiteGastoMensual1).HasColumnName("LimiteGastoMensual");

            entity.HasOne(d => d.Usuario).WithMany()
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__LIMITEGAS__Usuar__6754599E");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu).HasName("PK__Menu__C26AF483FDFE01FB");

            entity.ToTable("Menu");

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.Controlador)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("controlador");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Icono)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("icono");
            entity.Property(e => e.IdMenuPadre).HasColumnName("idMenuPadre");
            entity.Property(e => e.PaginaAccion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("paginaAccion");

            entity.HasOne(d => d.IdMenuPadreNavigation).WithMany(p => p.InverseIdMenuPadreNavigation)
                .HasForeignKey(d => d.IdMenuPadre)
                .HasConstraintName("FK__Menu__idMenuPadr__18EBB532");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__3C872F764323DF93");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
        });

        modelBuilder.Entity<RolMenu>(entity =>
        {
            entity.HasKey(e => e.IdRolMenu).HasName("PK__RolMenu__CD2045D8C306E060");

            entity.ToTable("RolMenu");

            entity.Property(e => e.IdRolMenu).HasColumnName("idRolMenu");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.RolMenus)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("FK__RolMenu__idMenu__1DB06A4F");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolMenus)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__RolMenu__idRol__1CBC4616");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__USUARIO__2B3DE7B8BDD36CF8");

            entity.ToTable("USUARIO");

            entity.Property(e => e.Clave)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__USUARIO__idRol__1F98B2C1");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId);
            entity.ToTable("Categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(8)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
