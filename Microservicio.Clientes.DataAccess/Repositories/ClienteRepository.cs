using Microservicio.Clientes.DataAccess.Context;
using Microservicio.Clientes.DataAccess.Entities;
using Microservicio.Clientes.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataAccess.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesDbContext _context;

    public ClienteRepository(ClientesDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ClienteEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken);
    }

    public async Task<ClienteEntity?> GetByCedulaAsync(string cedulaRuc, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CedulaRuc == cedulaRuc, cancellationToken);
    }

    public async Task<IEnumerable<ClienteEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .Where(c => c.EstadoCli && !c.Eliminado)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClienteEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClienteEntity>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync(cancellationToken);

        return await _context.Clientes
            .AsNoTracking()
            .Where(c =>
                EF.Functions.Like(c.RazonSocial, $"%{searchTerm}%") ||
                EF.Functions.Like(c.CedulaRuc, $"%{searchTerm}%") ||
                EF.Functions.Like(c.Correo!, $"%{searchTerm}%"))
            .ToListAsync(cancellationToken);
    }

    public async Task<ClienteEntity> AddAsync(ClienteEntity cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cliente;
    }

    public async Task<bool> UpdateAsync(ClienteEntity cliente, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Clientes.FindAsync(new object[] { cliente.IdCliente }, cancellationToken);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(cliente);
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes.FindAsync(new object[] { id }, cancellationToken);
        if (cliente == null) return false;

        cliente.Eliminado = true;
        cliente.DeletedAt = DateTime.Now;

        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> HardDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _context.Clientes.FindAsync(new object[] { id }, cancellationToken);
        if (cliente == null) return false;

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsByCedulaAsync(string cedulaRuc, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Clientes
            .AsNoTracking()
            .Where(c => c.CedulaRuc == cedulaRuc);

        if (excludeId.HasValue)
            query = query.Where(c => c.IdCliente != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string correo, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Clientes
            .AsNoTracking()
            .Where(c => c.Correo == correo);

        if (excludeId.HasValue)
            query = query.Where(c => c.IdCliente != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<ClienteEntity?> ObtenerParaActualizarAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == id, cancellationToken);
    }
}