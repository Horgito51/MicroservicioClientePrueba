namespace Microservicio.Clientes.Business.DTOs.Cliente;

/// <summary>
/// Filtros para búsqueda paginada de clientes
/// </summary>
public class ClienteFiltroRequest
{
    /// <summary>
    /// Filtrar por razón social (búsqueda parcial)
    /// </summary>
    public string? RazonSocial { get; set; }

    /// <summary>
    /// Filtrar por cédula/RUC (búsqueda parcial)
    /// </summary>
    public string? CedulaRuc { get; set; }

    /// <summary>
    /// Filtrar por correo electrónico (búsqueda parcial)
    /// </summary>
    public string? Correo { get; set; }

    /// <summary>
    /// Filtrar por estado (true=activo, false=inactivo)
    /// </summary>
    public bool? Estado { get; set; }

    /// <summary>
    /// Número de página (desde 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Tamaño de página (mínimo 1, máximo 100)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Campo por el cual ordenar (IdCliente, RazonSocial, CedulaRuc, CreatedAt, Estado)
    /// </summary>
    public string SortBy { get; set; } = "IdCliente";

    /// <summary>
    /// Orden descendente (true) o ascendente (false)
    /// </summary>
    public bool SortDescending { get; set; } = false;
}