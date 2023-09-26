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

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Historialcomentario> Historialcomentarios { get; set; }

    public virtual DbSet<Historialgastomensual> Historialgastomensuals { get; set; }

    public virtual DbSet<Limitegastomensual> Limitegastomensuals { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
