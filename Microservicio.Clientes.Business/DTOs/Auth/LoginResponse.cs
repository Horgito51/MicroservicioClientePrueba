namespace Microservicio.Clientes.Business.DTOs.Auth;

/// <summary>
/// Respuesta de inicio de sesión exitoso
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Token JWT para autenticación
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de token (Bearer)
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Tiempo de expiración del token en segundos
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Fecha y hora de expiración del token
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// ID del usuario autenticado
    /// </summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Nombre de usuario
    /// </summary>
    public string NombreUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Nombres completos del usuario
    /// </summary>
    public string Nombres { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos del usuario
    /// </summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Roles asignados al usuario
    /// </summary>
    public List<string> Roles { get; set; } = new();
}