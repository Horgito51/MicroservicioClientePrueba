using Microservicio.Clientes.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservicio.Clientes.DataAccess.Configurations;

internal class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
{
    public void Configure(EntityTypeBuilder<ClienteEntity> builder)
    {
        // -------------------------------
        // Tabla
        // -------------------------------
        builder.ToTable("Cliente", "dbo");

        // -------------------------------
        // PK
        // -------------------------------
        builder.HasKey(c => c.IdCliente);

        builder.Property(c => c.IdCliente)
               .HasColumnName("id_cliente")
               .UseIdentityColumn();

        // -------------------------------
        // Datos principales
        // -------------------------------
        builder.Property(c => c.CedulaRuc)
               .HasColumnName("cedula_ruc")
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(c => c.RazonSocial)
               .HasColumnName("razon_social")
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(c => c.Direccion)
               .HasColumnName("direccion")
               .HasMaxLength(300);

        builder.Property(c => c.Correo)
               .HasColumnName("correo")
               .HasMaxLength(150);

        builder.Property(c => c.Celular)
               .HasColumnName("celular")
               .HasMaxLength(20);

        // -------------------------------
        // Estado
        // -------------------------------
        builder.Property(c => c.EstadoCli)
               .HasColumnName("estado_cli")
               .IsRequired();

        builder.Property(c => c.Eliminado)
               .HasColumnName("eliminado")
               .IsRequired();

        // -------------------------------
        // Auditoría
        // -------------------------------
        builder.Property(c => c.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAdd();

        builder.Property(c => c.UpdatedAt)
               .HasColumnName("updated_at")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAddOrUpdate();

        builder.Property(c => c.DeletedAt)
               .HasColumnName("deleted_at");

        builder.Property(c => c.CreatedBy)
               .HasColumnName("created_by")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.UpdatedBy)
               .HasColumnName("updated_by")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.DeletedBy)
               .HasColumnName("deleted_by")
               .HasMaxLength(100);

        builder.Property(c => c.CreatedIp)
               .HasColumnName("created_ip")
               .HasMaxLength(45);

        builder.Property(c => c.UpdatedIp)
               .HasColumnName("updated_ip")
               .HasMaxLength(45);

        builder.Property(c => c.DeletedIp)
               .HasColumnName("deleted_ip")
               .HasMaxLength(45);

        // -------------------------------
        // Índices
        // -------------------------------
        builder.HasIndex(c => c.CedulaRuc)
               .IsUnique()
               .HasDatabaseName("IX_Cliente_CedulaRuc");

        builder.HasIndex(c => c.EstadoCli)
               .HasDatabaseName("IX_Cliente_Estado");

        builder.HasIndex(c => c.Correo)
               .HasDatabaseName("IX_Cliente_Correo")
               .HasFilter("[correo] IS NOT NULL");
    }
}