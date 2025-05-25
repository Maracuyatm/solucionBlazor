using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Models;

public partial class DbcrudBlazorContext : DbContext
{
    public DbcrudBlazorContext()
    {
    }

    public DbcrudBlazorContext(DbContextOptions<DbcrudBlazorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }
    public virtual DbSet<Marca> Marca { get; set; }
    public virtual DbSet<Procesador> Procesador { get; set; }
    public virtual DbSet<Activo> Activo { get; set; }
    public virtual DbSet<Estado> Estado { get; set; }
    public virtual DbSet<SistemaOperativo> SistemaOperativo { get; set; }
    public virtual DbSet<TipoActivo> TipoActivo { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__787A433DAFAE86EE");

            entity.ToTable("Departamento");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9EDEB9769C");

            entity.ToTable("Empleado");

            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Empleado__IdDepa__4D94879B");
        });

        modelBuilder.Entity<Activo>(entity =>
        {
            entity.ToTable("activo");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.TipoActivoId)
                  .HasColumnName("tipo_activo_id")
                  .IsRequired();

            entity.Property(e => e.MarcaId)
                  .HasColumnName("marca_id")
                  .IsRequired();

            entity.Property(e => e.EstadoId)
                  .HasColumnName("estado_id")
                  .IsRequired();

            entity.Property(e => e.Serial)
                  .HasColumnName("serial")
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(e => e.SistemaOperativoId)
                  .HasColumnName("sistema_operativo_id")
                  .IsRequired(false);

            entity.Property(e => e.Modelo)
                  .HasColumnName("modelo")
                  .HasMaxLength(50)
                  .IsRequired(false);

            entity.Property(e => e.Procesador)
                  .HasColumnName("procesador")
                  .IsRequired(false);

            entity.Property(e => e.Descripcion)
                  .HasColumnName("descripcion")
                  .HasMaxLength(250)
                  .IsRequired();

            entity.Property(e => e.Numero)
                  .HasColumnName("numero")
                  .IsRequired(false);

            entity.Property(e => e.Ram)
                  .HasColumnName("ram")
                  .IsRequired(false);

            entity.Property(e => e.Almacenamiento)
                  .HasColumnName("almacenamiento")
                  .IsRequired(false);

            entity.Property(e => e.FechaAdquisicion)
                  .HasColumnName("fecha_adquisicion")
                  .IsRequired();

            entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .IsRequired(false);

            entity.Property(e => e.UpdatedAt)
                  .HasColumnName("updated_at")
                  .IsRequired(false);

            entity.Property(e => e.DeletedAt)
                  .HasColumnName("deleted_at")
                  .IsRequired(false);
        });


        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("marca");

            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id).HasColumnName("id");
            entity.Property(m => m.Nombre).HasColumnName("nombre").HasMaxLength(50);
            entity.Property(m => m.TipoActivoId)
                  .HasColumnName("tipo_activo_id")
                  .IsRequired();
            entity.Property(m => m.CreatedAt).HasColumnName("created_at");
            entity.Property(m => m.UpdatedAt).HasColumnName("updated_at");
            entity.Property(m => m.DeletedAt).HasColumnName("deleted_at");
        });

        modelBuilder.Entity<Procesador>(entity =>
        {
            entity.ToTable("procesador");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Estado).HasColumnName("estado");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("estado");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        });

        modelBuilder.Entity<TipoActivo>(entity =>
        {
            entity.ToTable("tipo_activo");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        });

        modelBuilder.Entity<SistemaOperativo>(entity =>
        {
            entity.ToTable("sistema_operativo");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
