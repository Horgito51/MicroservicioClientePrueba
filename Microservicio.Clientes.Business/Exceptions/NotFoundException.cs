using System;

namespace Microservicio.Clientes.Business.Exceptions;

/// <summary>
/// Excepción para recursos no encontrados.
/// Se lanza cuando una entidad buscada no existe en la base de datos.
/// </summary>
public class NotFoundException : Exception
{
    public string? EntityName { get; set; }
    public object? EntityId { get; set; }

    public NotFoundException() : base()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotFoundException(string entityName, object entityId)
        : base($"No se encontró la entidad '{entityName}' con ID/identificador: {entityId}")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    public NotFoundException(string entityName, string criterio, object valor)
        : base($"No se encontró la entidad '{entityName}' con {criterio}: {valor}")
    {
        EntityName = entityName;
        EntityId = valor;
    }
}