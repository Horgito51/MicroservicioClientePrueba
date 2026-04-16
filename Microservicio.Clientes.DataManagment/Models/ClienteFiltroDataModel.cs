using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Clientes.DataManagement.Models;

public class ClienteFiltroDataModel
{
    // -------------------------------
    // Filtros
    // -------------------------------

    public string? RazonSocial { get; set; }

    public string? CedulaRuc { get; set; }

    public string? Correo { get; set; }

    public bool? Estado { get; set; }

    // -------------------------------
    // Paginación
    // -------------------------------

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    // -------------------------------
    // Ordenamiento
    // -------------------------------

    public string SortBy { get; set; } = "IdCliente";

    public bool SortDescending { get; set; } = false;
}
