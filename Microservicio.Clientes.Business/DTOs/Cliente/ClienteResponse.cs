namespace Microservicio.Clientes.Business.DTOs.Cliente;

/// <summary>
/// Respuesta con datos del cliente
/// </summary>
public class ClienteResponse
{
    /// <summary>
    /// ID único del cliente
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

    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Usuario que creó el registro
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Usuario que actualizó el registro
    /// </summary>
    public string? UpdatedBy { get; set; } = string.Empty;
}