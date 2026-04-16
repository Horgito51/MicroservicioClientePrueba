using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Clientes.DataAccess.Entities;

/// <summary>
/// Entidad que representa la tabla seguridad.UsuarioApp en SQL Server.
/// Gestiona los usuarios que tienen acceso al sistema del microservicio de Clientes.
/// Usada exclusivamente en la capa de acceso a datos (EF Core).
/// </summary>
public class UsuarioAppEntity
{
    // -------------------------------------------------------------------------
    // Identificación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Clave primaria. Generada por la base de datos (IDENTITY).
    /// </summary>
    public int IdUsuario { get; set; }

    // -------------------------------------------------------------------------
    // Datos de autenticación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Nombre de usuario único para autenticación en el sistema.
    /// </summary>
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña almacenada como hash (bcrypt / SHA256).
    /// Nunca se almacena en texto plano.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario. Usado para notificaciones y recuperación de cuenta.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Datos funcionales
    // -------------------------------------------------------------------------

    /// <summary>
    /// Nombres del usuario para identificación interna.
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del usuario para identificación interna.
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Ciclo de vida
    // -------------------------------------------------------------------------

    /// <summary>
    /// Estado del usuario en el sistema.
    /// Valores válidos: A (Activo) | I (Inactivo) | B (Bloqueado).
    /// Valor por defecto: 'A'.
    /// </summary>
    public string EstadoUsuario { get; set; } = "A";

    /// <summary>
    /// Indica si el usuario debe cambiar su contraseña en el próximo inicio de sesión.
    /// </summary>
    public bool RequiereCambioPassword { get; set; } = false;

    // -------------------------------------------------------------------------
    // Control de acceso y seguridad
    // -------------------------------------------------------------------------

    /// <summary>
    /// Número de intentos fallidos de inicio de sesión consecutivos.
    /// Se resetea al iniciar sesión correctamente.
    /// </summary>
    public int IntentosFallidos { get; set; } = 0;

    /// <summary>
    /// Fecha hasta la que el usuario está bloqueado. Null si no está bloqueado.
    /// </summary>
    public DateTime? FechaBloqueo { get; set; }

    /// <summary>
    /// Fecha del último inicio de sesión exitoso.
    /// </summary>
    public DateTime? UltimoAcceso { get; set; }

    /// <summary>
    /// Token usado para recuperación de contraseña. Null si no hay proceso activo.
    /// </summary>
    public string? TokenRecuperacion { get; set; }

    /// <summary>
    /// Fecha de expiración del token de recuperación de contraseña.
    /// </summary>
    public DateTime? FechaExpiracionToken { get; set; }

    // -------------------------------------------------------------------------
    // Auditoría
    // -------------------------------------------------------------------------

    /// <summary>
    /// Fecha de creación del registro. Asignada por la BD (GETDATE()).
    /// </summary>
    public DateTime? FechaCreacion { get; set; }

    /// <summary>
    /// Fecha de última actualización. Actualizada por trigger en BD.
    /// </summary>
    public DateTime? FechaActualizacion { get; set; }

    /// <summary>
    /// Fecha de eliminación lógica. Null mientras el usuario esté activo.
    /// </summary>
    public DateTime? FechaEliminacion { get; set; }

    // -------------------------------------------------------------------------
    // Relaciones (mismo bounded context)
    // -------------------------------------------------------------------------

    /// <summary>
    /// Colección de roles asignados al usuario a través de la tabla intermedia
    /// seguridad.UsuarioRol. Un usuario puede tener múltiples roles.
    /// </summary>
    public ICollection<UsuarioRolEntity> UsuarioRoles { get; set; } = new List<UsuarioRolEntity>();
}