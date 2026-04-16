using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Clientes.DataAccess.Entities;

/// <summary>
/// Entidad que representa la tabla seguridad.Rol en SQL Server.
/// Define los roles disponibles dentro del sistema del microservicio de Clientes.
/// Usada exclusivamente en la capa de acceso a datos (EF Core).
/// </summary>
public class RolEntity
{
    // -------------------------------------------------------------------------
    // Identificación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Clave primaria. Generada por la base de datos (IDENTITY).
    /// </summary>
    public int IdRol { get; set; }

    // -------------------------------------------------------------------------
    // Datos funcionales
    // -------------------------------------------------------------------------

    /// <summary>
    /// Nombre único del rol dentro del sistema.
    /// Ejemplos: DBA | ADMIN | VENDEDOR
    /// </summary>
    public string NombreRol { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada de las responsabilidades y alcance del rol.
    /// </summary>
    public string? Descripcion { get; set; }

    // -------------------------------------------------------------------------
    // Ciclo de vida
    // -------------------------------------------------------------------------

    /// <summary>
    /// Estado del rol en el sistema.
    /// Valores válidos: A (Activo) | I (Inactivo).
    /// Valor por defecto: 'A'.
    /// </summary>
    public string EstadoRol { get; set; } = "A";

    // -------------------------------------------------------------------------
    // Auditoría
    // -------------------------------------------------------------------------

    /// <summary>
    /// Fecha de creación del registro. Asignada por la BD (GETDATE()).
    /// </summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>
    /// Fecha de última actualización. Actualizada por trigger en BD.
    /// </summary>
    public DateTime FechaActualizacion { get; set; }

    /// <summary>
    /// Fecha de eliminación lógica. Null mientras el rol esté activo.
    /// </summary>
    public DateTime? FechaEliminacion { get; set; }

    // -------------------------------------------------------------------------
    // Relaciones (mismo bounded context)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Colección de usuarios asignados a este rol a través de la tabla intermedia
    /// seguridad.UsuarioRol. Un rol puede estar asignado a múltiples usuarios.
    /// </summary>
    public ICollection<UsuarioRolEntity> UsuarioRoles { get; set; } = new List<UsuarioRolEntity>();
}