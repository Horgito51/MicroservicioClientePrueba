using Microservicio.Clientes.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Clientes.DataAccess.Configurations;

/// <summary>
/// Configuración de la entidad UsuarioAppEntity para EF Core.
/// Define el mapeo a la tabla seguridad.UsuarioApp, sus restricciones e índices.
/// </summary>
internal class UsuarioAppConfiguration : IEntityTypeConfiguration<UsuarioAppEntity>
{
    public void Configure(EntityTypeBuilder<UsuarioAppEntity> builder)
    {
        // -----------------------------------------------------------------
        // Tabla y esquema
        // -----------------------------------------------------------------
        builder.ToTable("UsuarioApp", "seguridad");

        // -----------------------------------------------------------------
        // Clave primaria
        // -----------------------------------------------------------------
        builder.HasKey(u => u.IdUsuario)
               .HasName("PK_UsuarioApp");

        builder.Property(u => u.IdUsuario)
               .HasColumnName("id_usuario")
               .UseIdentityColumn();

        // -----------------------------------------------------------------
        // Propiedades y mapeo de columnas
        // -----------------------------------------------------------------
        builder.Property(u => u.NombreUsuario)
               .HasColumnName("nombre_usuario")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.PasswordHash)
               .HasColumnName("password_hash")
               .HasMaxLength(256)
               .IsRequired();

        builder.Property(u => u.Email)
               .HasColumnName("email")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.Nombres)
               .HasColumnName("nombres")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(u => u.Apellidos)
               .HasColumnName("apellidos")
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(u => u.EstadoUsuario)
               .HasColumnName("estado_usuario")
               .HasMaxLength(1)
               .IsRequired()
               .HasDefaultValue("A");

        builder.Property(u => u.RequiereCambioPassword)
               .HasColumnName("requiere_cambio_password")
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(u => u.IntentosFallidos)
               .HasColumnName("intentos_fallidos")
               .IsRequired()
               .HasDefaultValue(0);

        builder.Property(u => u.FechaBloqueo)
               .HasColumnName("fecha_bloqueo")
               .IsRequired(false);

        builder.Property(u => u.UltimoAcceso)
               .HasColumnName("ultimo_acceso")
               .IsRequired(false);

        builder.Property(u => u.TokenRecuperacion)
               .HasColumnName("token_recuperacion")
               .HasMaxLength(256)
               .IsRequired(false);

        builder.Property(u => u.FechaExpiracionToken)
               .HasColumnName("fecha_expiracion_token")
               .IsRequired(false);

        builder.Property(u => u.FechaCreacion)
               .HasColumnName("fecha_creacion")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAdd();

        builder.Property(u => u.FechaActualizacion)
               .HasColumnName("fecha_actualizacion")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAddOrUpdate();

        builder.Property(u => u.FechaEliminacion)
               .HasColumnName("fecha_eliminacion")
               .IsRequired(false);

        // -----------------------------------------------------------------
        // Índices
        // -----------------------------------------------------------------
        builder.HasIndex(u => u.NombreUsuario)
               .HasDatabaseName("IX_UsuarioApp_NombreUsuario")
               .IsUnique();

        builder.HasIndex(u => u.Email)
               .HasDatabaseName("IX_UsuarioApp_Email")
               .IsUnique();

        builder.HasIndex(u => u.EstadoUsuario)
               .HasDatabaseName("IX_UsuarioApp_EstadoUsuario");

        // -----------------------------------------------------------------
        // Relaciones
        // -----------------------------------------------------------------
        builder.HasMany(u => u.UsuarioRoles)
               .WithOne(ur => ur.Usuario)
               .HasForeignKey(ur => ur.IdUsuario)
               .HasConstraintName("FK_UsuarioRol_UsuarioApp")
               .OnDelete(DeleteBehavior.Cascade);
    }
}