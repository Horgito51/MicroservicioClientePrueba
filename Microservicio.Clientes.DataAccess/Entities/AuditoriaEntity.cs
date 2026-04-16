using System;
using System.Collections.Generic;
using System.Text;


namespace Microservicio.Clientes.DataAccess.Entities;

/// <summary>
/// Entidad que representa la tabla seguridad.Log_Auditoria en SQL Server.
/// Almacena el historial de cambios (INSERT, UPDATE, DELETE) sobre las tablas principales.
/// Usada exclusivamente en la capa de acceso a datos (EF Core).
/// </summary>
public class AuditoriaEntity
{
    // -------------------------------------------------------------------------
    // Identificación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Clave primaria. Generada por la base de datos (IDENTITY).
    /// </summary>
    public long IdLog { get; set; }

    // -------------------------------------------------------------------------
    // Datos de auditoría
    // -------------------------------------------------------------------------

    /// <summary>
    /// Nombre de la tabla afectada por la operación (incluye schema, ej. 'hotel.Cliente').
    /// </summary>
    public string TablaAfectada { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de acción ejecutada.
    /// Valores válidos: INSERT | UPDATE | DELETE.
    /// </summary>
    public string TipoAccion { get; set; } = string.Empty;

    /// <summary>
    /// Descripción textual de la operación (opcional).
    /// Puede incluir información contextual como el ID del registro afectado.
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Datos anteriores a la modificación, en formato JSON.
    /// Se almacena como NVARCHAR(MAX) para soportar documentos grandes.
    /// </summary>
    public string? DatosOld { get; set; }

    /// <summary>
    /// Datos posteriores a la modificación, en formato JSON.
    /// Se almacena como NVARCHAR(MAX) para soportar documentos grandes.
    /// </summary>
    public string? DatosNew { get; set; }

    // -------------------------------------------------------------------------
    // Metadatos de la operación
    // -------------------------------------------------------------------------

    /// <summary>
    /// Usuario de base de datos que ejecutó la operación.
    /// Valor por defecto en BD: SYSTEM_USER.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Dirección IP de origen de la conexión (opcional).
    /// Formato: IPv4 o IPv6, máximo 45 caracteres.
    /// </summary>
    public string? IpOrigen { get; set; }

    /// <summary>
    /// Fecha y hora del evento.
    /// Asignada automáticamente por la BD con GETDATE().
    /// </summary>
    public DateTime FechaEvento { get; set; }
}