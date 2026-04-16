using Microservicio.Clientes.DataAccess.Common;
using Microservicio.Clientes.DataAccess.Context;
using Microservicio.Clientes.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataAccess.Queries
{
    internal class ClienteQueryRepository
    {
        private readonly ClientesDbContext _context;

        public ClienteQueryRepository(ClientesDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // -------------------------------
        // Filtro por fecha
        // -------------------------------
        public async Task<IEnumerable<ClienteEntity>> GetClientesByFechaCreacionAsync(
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AsNoTracking()
                .Where(c => c.CreatedAt >= fechaInicio && c.CreatedAt <= fechaFin)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        // -------------------------------
        // Búsqueda
        // -------------------------------
        public async Task<IEnumerable<ClienteEntity>> SearchClientesAsync(
            string term,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<ClienteEntity>();

            term = term.Trim();

            return await _context.Clientes
                .AsNoTracking()
                .Where(c =>
                    EF.Functions.Like(c.CedulaRuc, $"%{term}%") ||
                    EF.Functions.Like(c.RazonSocial, $"%{term}%") ||
                    (c.Correo != null && EF.Functions.Like(c.Correo, $"%{term}%"))
                )
                .OrderBy(c => c.RazonSocial)
                .Take(50)
                .ToListAsync(cancellationToken);
        }

        // -------------------------------
        // Paginado
        // -------------------------------
        public async Task<PagedResult<ClienteEntity>> GetPagedClientesAsync(
            int pageNumber,
            int pageSize,
            string? filterNombre = null,
            string? filterIdentificacion = null,
            bool? estado = null,
            string? sortBy = "IdCliente",
            bool sortDescending = false,
            CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Clientes.AsNoTracking();

            // 🔹 filtro nombre → ahora es razon social
            if (!string.IsNullOrEmpty(filterNombre))
            {
                query = query.Where(c =>
                    c.RazonSocial != null &&
                    EF.Functions.Like(c.RazonSocial, $"%{filterNombre}%"));
            }

            // 🔹 filtro identificación
            if (!string.IsNullOrEmpty(filterIdentificacion))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.CedulaRuc, $"%{filterIdentificacion}%"));
            }

            // 🔹 estado (bool)
            if (estado.HasValue)
            {
                query = query.Where(c => c.EstadoCli == estado.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            query = ApplySorting(query, sortBy!, sortDescending);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ClienteEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        // -------------------------------
        // Ordenamiento
        // -------------------------------
        private IQueryable<ClienteEntity> ApplySorting(
            IQueryable<ClienteEntity> query,
            string sortBy,
            bool sortDescending)
        {
            return sortBy.ToLowerInvariant() switch
            {
                "nombre" => sortDescending
                    ? query.OrderByDescending(c => c.RazonSocial)
                    : query.OrderBy(c => c.RazonSocial),

                "identificacion" => sortDescending
                    ? query.OrderByDescending(c => c.CedulaRuc)
                    : query.OrderBy(c => c.CedulaRuc),

                "fechacreacion" => sortDescending
                    ? query.OrderByDescending(c => c.CreatedAt)
                    : query.OrderBy(c => c.CreatedAt),

                "estado" => sortDescending
                    ? query.OrderByDescending(c => c.EstadoCli)
                    : query.OrderBy(c => c.EstadoCli),

                _ => sortDescending
                    ? query.OrderByDescending(c => c.IdCliente)
                    : query.OrderBy(c => c.IdCliente)
            };
        }
    }
}