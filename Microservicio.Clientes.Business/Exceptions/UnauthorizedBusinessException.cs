using System;

namespace Microservicio.Clientes.Business.Exceptions;

/// <summary>
/// Excepción para errores de autorización/autenticación en la capa de negocio.
/// Se lanza cuando el usuario no tiene permisos para realizar una acción.
/// </summary>
public class UnauthorizedBusinessException : Exception
{
    public string? CodigoError { get; set; }
    public string? Recurso { get; set; }
    public string? Accion { get; set; }

    public UnauthorizedBusinessException() : base()
    {
    }

    public UnauthorizedBusinessException(string message) : base(message)
    {
    }

    public UnauthorizedBusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public UnauthorizedBusinessException(string codigoError, string message) : base(message)
    {
        CodigoError = codigoError;
    }

    public UnauthorizedBusinessException(string codigoError, string recurso, string accion)
        : base($"No autorizado para realizar '{accion}' sobre el recurso '{recurso}'")
    {
        CodigoError = codigoError;
        Recurso = recurso;
        Accion = accion;
    }

    public UnauthorizedBusinessException(string codigoError, string recurso, string accion, string detalle)
        : base($"No autorizado para realizar '{accion}' sobre el recurso '{recurso}': {detalle}")
    {
        CodigoError = codigoError;
        Recurso = recurso;
        Accion = accion;
    }
}