using System;
using System.Collections.Generic;
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
    //public virtual DbSet<Activo> Activo { get; set; }
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

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("marca");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
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
