using Microservicio.Clientes.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Clientes.DataAccess.Configurations;

/// <summary>
/// Configuración de la entidad RolEntity para EF Core.
/// Define el mapeo a la tabla seguridad.Rol, sus restricciones e índices.
/// </summary>
internal class RolConfiguration : IEntityTypeConfiguration<RolEntity>
{
    public void Configure(EntityTypeBuilder<RolEntity> builder)
    {
        // -----------------------------------------------------------------
        // Tabla y esquema
        // -----------------------------------------------------------------
        builder.ToTable("Rol", "seguridad");

        // -----------------------------------------------------------------
        // Clave primaria
        // -----------------------------------------------------------------
        builder.HasKey(r => r.IdRol)
               .HasName("PK_Rol");

        builder.Property(r => r.IdRol)
               .HasColumnName("id_rol")
               .UseIdentityColumn();

        // -----------------------------------------------------------------
        // Propiedades y mapeo de columnas
        // -----------------------------------------------------------------
        builder.Property(r => r.NombreRol)
               .HasColumnName("nombre_rol")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(r => r.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(150)
               .IsRequired(false);

        builder.Property(r => r.EstadoRol)
               .HasColumnName("estado_rol")
               .HasMaxLength(1)
               .IsRequired()
               .HasDefaultValue("A");

        builder.Property(r => r.FechaCreacion)
               .HasColumnName("fecha_creacion")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAdd();

        builder.Property(r => r.FechaActualizacion)
               .HasColumnName("fecha_actualizacion")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAddOrUpdate();

        builder.Property(r => r.FechaEliminacion)
               .HasColumnName("fecha_eliminacion")
               .IsRequired(false);

        // -----------------------------------------------------------------
        // Índices
        // -----------------------------------------------------------------
        builder.HasIndex(r => r.NombreRol)
               .HasDatabaseName("IX_Rol_NombreRol")
               .IsUnique();

        builder.HasIndex(r => r.EstadoRol)
               .HasDatabaseName("IX_Rol_EstadoRol");

        // -----------------------------------------------------------------
        // Relaciones
        // -----------------------------------------------------------------
        builder.HasMany(r => r.UsuarioRoles)
               .WithOne(ur => ur.Rol)
               .HasForeignKey(ur => ur.IdRol)
               .HasConstraintName("FK_UsuarioRol_Rol")
               .OnDelete(DeleteBehavior.Restrict);
    }
}