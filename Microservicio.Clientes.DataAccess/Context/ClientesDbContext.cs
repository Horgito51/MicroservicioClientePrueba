using Microsoft.EntityFrameworkCore;
using Microservicio.Clientes.DataAccess.Entities;
using Microservicio.Clientes.DataAccess.Configurations;

namespace Microservicio.Clientes.DataAccess.Context;

/// <summary>
/// DbContext principal del microservicio de Clientes.
/// Administra las entidades relacionadas con clientes, usuarios, roles y auditoría.
/// </summary>
public class ClientesDbContext : DbContext
{
    // -----------------------------------------------------------------
    // Constructores
    // -----------------------------------------------------------------
    public ClientesDbContext(DbContextOptions<ClientesDbContext> options)
        : base(options)
    {
    }

    // -----------------------------------------------------------------
    // DbSets
    // -----------------------------------------------------------------

    /// <summary>
    /// Clientes del sistema (tabla hotel.Cliente).
    /// </summary>
    public DbSet<ClienteEntity> Clientes { get; set; }

    /// <summary>
    /// Usuarios de aplicación (tabla seguridad.UsuarioApp).
    /// </summary>
    public DbSet<UsuarioAppEntity> UsuariosApp { get; set; }

    /// <summary>
    /// Roles del sistema (tabla seguridad.Rol).
    /// </summary>
    public DbSet<RolEntity> Roles { get; set; }

    /// <summary>
    /// Asignaciones de roles a usuarios (tabla seguridad.UsuarioRol).
    /// </summary>
    public DbSet<UsuarioRolEntity> UsuarioRoles { get; set; }

    /// <summary>
    /// Registros de auditoría (tabla seguridad.Log_Auditoria).
    /// </summary>
    public DbSet<AuditoriaEntity> Auditorias { get; set; }

    // -----------------------------------------------------------------
    // Configuración del modelo
    // -----------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas las configuraciones definidas en el ensamblado
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioAppConfiguration());
        modelBuilder.ApplyConfiguration(new RolConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioRolConfiguration());
        modelBuilder.ApplyConfiguration(new AuditoriaConfiguration());

        // Nota: No se incluyen las configuraciones de Ciudad ni Pais
        // porque pertenecen a otros microservicios. Las FK se manejan
        // solo con valores numéricos en las entidades.
    }
}