using System.Text.Json.Serialization;

namespace Microservicio.Clientes.Business.DTOs.Cliente;

/// <summary>
/// Solicitud para actualizar un cliente existente
/// </summary>
public class ActualizarClienteRequest
{
    /// <summary>
    /// ID del cliente a actualizar
    /// </summary>
    /// <example>1</example>
    public int IdCliente { get; set; }

    /// <summary>
    /// Cédula (persona natural) o RUC (empresa)
    /// </summary>
    /// <example>1712345678</example>
    public string CedulaRuc { get; set; } = string.Empty;

    /// <summary>
    /// Razón social o nombre completo
    /// </summary>
    /// <example>Juan Carlos Morales Vega</example>
    public string RazonSocial { get; set; } = string.Empty;

    /// <summary>
    /// Dirección del cliente
    /// </summary>
    /// <example>Av. 6 de Diciembre N24-100, Quito</example>
    public string? Direccion { get; set; }

    /// <summary>
    /// Correo electrónico
    /// </summary>
    /// <example>jmorales@gmail.com</example>
    public string? Correo { get; set; }

    /// <summary>
    /// Número de celular
    /// </summary>
    /// <example>0991234567</example>
    public string? Celular { get; set; }

    /// <summary>
    /// Estado del cliente (activo/inactivo)
    /// </summary>
    public bool Estado { get; set; }

    // ============================================================
    // Campos de auditoría (se llenan automáticamente en el servicio)
    // ============================================================

    [JsonIgnore]
    public string? UpdatedBy { get; set; }

    [JsonIgnore]
    public string? UpdatedIp { get; set; }
}