using System;

namespace Microservicio.Clientes.Business.Exceptions;

/// <summary>
/// Excepción base para errores de lógica de negocio.
/// Se lanza cuando una regla de negocio no se cumple.
/// </summary>
public class BusinessException : Exception
{
    public string? CodigoError { get; set; }

    public BusinessException() : base()
    {
    }

    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public BusinessException(string codigoError, string message) : base(message)
    {
        CodigoError = codigoError;
    }

    public BusinessException(string codigoError, string message, Exception innerException) : base(message, innerException)
    {
        CodigoError = codigoError;
    }
}