using System;
using System.Collections.Generic;

namespace Microservicio.Clientes.DataManagement.Models;

public class DataPagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}