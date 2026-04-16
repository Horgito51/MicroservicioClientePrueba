using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservicio.Clientes.Business.Exceptions;

/// <summary>
/// Excepción para errores de validación de datos.
/// Se lanza cuando los datos de entrada no cumplen las reglas de validación.
/// </summary>
public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errores { get; set; }

    public string? Campo { get; set; }
    public ValidationException() : base("Se han producido uno o más errores de validación.")
    {
        Errores = new Dictionary<string, string[]>();
    }

    public ValidationException(string message) : base(message)
    {
        Errores = new Dictionary<string, string[]>();
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
        Errores = new Dictionary<string, string[]>();
    }

    public ValidationException(Dictionary<string, string[]> errores)
        : base("Se han producido uno o más errores de validación.")
    {
        Errores = errores;
    }

    public ValidationException(string campo, string mensaje) : base("Se han producido uno o más errores de validación.")
    {
        Errores = new Dictionary<string, string[]>
        {
            { campo, new[] { mensaje } }
        };
    }

    public ValidationException(string campo, string[] mensajes) : base("Se han producido uno o más errores de validación.")
    {
        Errores = new Dictionary<string, string[]>
        {
            { campo, mensajes }
        };
    }

    public void AgregarError(string campo, string mensaje)
    {
        if (!Errores.ContainsKey(campo))
            Errores[campo] = Array.Empty<string>();

        Errores[campo] = Errores[campo].Append(mensaje).ToArray();
    }

    public void AgregarError(string campo, string[] mensajes)
    {
        if (!Errores.ContainsKey(campo))
            Errores[campo] = Array.Empty<string>();

        Errores[campo] = Errores[campo].Concat(mensajes).ToArray();
    }
}