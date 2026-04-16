using System;

namespace Microservicio.Clientes.DataAccess.Entities
{
    public class ClienteEntity
    {
        public int IdCliente { get; set; }

        public string CedulaRuc { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;

        public string? Direccion { get; set; }
        public string? Correo { get; set; }
        public string? Celular { get; set; }

        public bool EstadoCli { get; set; }
        public bool Eliminado { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        public string? CreatedIp { get; set; }
        public string? UpdatedIp { get; set; }
        public string? DeletedIp { get; set; }
    }
}