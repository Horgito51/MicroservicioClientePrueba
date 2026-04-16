using Microservicio.Clientes.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class AuditoriaConfiguration : IEntityTypeConfiguration<AuditoriaEntity>
{
    public void Configure(EntityTypeBuilder<AuditoriaEntity> builder)
    {
        builder.ToTable("Log_Auditoria", "seguridad");

        builder.HasKey(a => a.IdLog);

        builder.Property(a => a.IdLog)
               .HasColumnName("id_log")
               .UseIdentityColumn();

        builder.Property(a => a.TablaAfectada)
               .HasColumnName("tabla_afectada")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(a => a.TipoAccion)
               .HasColumnName("tipo_accion")
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(a => a.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(255);

        builder.Property(a => a.DatosOld)
               .HasColumnName("datos_old");

        builder.Property(a => a.DatosNew)
               .HasColumnName("datos_new");

        builder.Property(a => a.Usuario)
               .HasColumnName("usuario")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(a => a.IpOrigen)
               .HasColumnName("ip_origen")
               .HasMaxLength(45);

        builder.Property(a => a.FechaEvento)
               .HasColumnName("fecha_evento")
               .HasDefaultValueSql("GETDATE()");
    }
}