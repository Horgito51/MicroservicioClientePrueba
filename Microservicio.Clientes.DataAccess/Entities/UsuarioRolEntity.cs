using System;

namespace Microservicio.Clientes.DataAccess.Entities;

public class UsuarioRolEntity
{
    // -------------------------------
    // PK compuesta
    // -------------------------------
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }

    // -------------------------------
    // Estado
    // -------------------------------
    public string EstadoAsignacion { get; set; } = "A";

    // -------------------------------
    // Auditoría
    // -------------------------------
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public DateTime? FechaEliminacion { get; set; }

    // -------------------------------
    // Navegación
    // -------------------------------
    public virtual UsuarioAppEntity Usuario { get; set; } = null!;
    public virtual RolEntity Rol { get; set; } = null!;
}