using Microservicio.Clientes.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Clientes.DataAccess.Configurations;

/// <summary>
/// Configuración de la entidad UsuarioRolEntity para EF Core.
/// Define el mapeo a la tabla seguridad.UsuarioRol, sus restricciones e índices.
/// </summary>
internal class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRolEntity>
{
    public void Configure(EntityTypeBuilder<UsuarioRolEntity> builder)
    {
        // -----------------------------------------------------------------
        // Tabla y esquema
        // -----------------------------------------------------------------
        builder.ToTable("UsuarioRol", "seguridad");

        // -----------------------------------------------------------------
        // Clave primaria compuesta
        // -----------------------------------------------------------------
        builder.HasKey(ur => new { ur.IdUsuario, ur.IdRol })
               .HasName("PK_UsuarioRol");

        builder.Property(ur => ur.IdUsuario)
               .HasColumnName("id_usuario")
               .IsRequired();

        builder.Property(ur => ur.IdRol)
               .HasColumnName("id_rol")
               .IsRequired();

        // -----------------------------------------------------------------
        // Propiedades adicionales
        // -----------------------------------------------------------------
        builder.Property(ur => ur.EstadoAsignacion)
               .HasColumnName("estado_asignacion")
               .HasMaxLength(1)
               .IsRequired()
               .HasDefaultValue("A");

        builder.Property(ur => ur.FechaCreacion)
               .HasColumnName("fecha_creacion")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAdd();

        builder.Property(ur => ur.FechaActualizacion)
               .HasColumnName("fecha_actualizacion")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAddOrUpdate();

        builder.Property(ur => ur.FechaEliminacion)
               .HasColumnName("fecha_eliminacion")
               .IsRequired(false);

        // -----------------------------------------------------------------
        // Índices
        // -----------------------------------------------------------------
        builder.HasIndex(ur => new { ur.IdUsuario, ur.EstadoAsignacion })
               .HasDatabaseName("IX_UsuarioRol_Usuario_Estado");

        builder.HasIndex(ur => new { ur.IdRol, ur.EstadoAsignacion })
               .HasDatabaseName("IX_UsuarioRol_Rol_Estado");

        // -----------------------------------------------------------------
        // Relaciones (navegación ya definida en UsuarioApp y Rol, pero podemos configurar aquí también)
        // -----------------------------------------------------------------
        builder.HasOne(ur => ur.Usuario)
               .WithMany(u => u.UsuarioRoles)
               .HasForeignKey(ur => ur.IdUsuario)
               .HasConstraintName("FK_UsuarioRol_UsuarioApp")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Rol)
               .WithMany(r => r.UsuarioRoles)
               .HasForeignKey(ur => ur.IdRol)
               .HasConstraintName("FK_UsuarioRol_Rol")
               .OnDelete(DeleteBehavior.Restrict);
    }
}