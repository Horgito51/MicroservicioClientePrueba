using System.Text.Json.Serialization;

namespace Microservicio.Clientes.Business.DTOs.Auth;

/// <summary>
/// Solicitud de inicio de sesión
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Nombre de usuario o email del usuario
    /// </summary>
    /// <example>juan.perez</example>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario (en texto plano, se hasheará en el backend)
    /// </summary>
    /// <example>MiPassword123!</example>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Dirección IP del cliente (opcional, se captura automáticamente)
    /// </summary>
    [JsonIgnore]
    public string? IpOrigen { get; set; }
}