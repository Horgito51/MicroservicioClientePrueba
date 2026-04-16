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

public class UsuarioAppRepository : IUsuarioAppRepository
{
    private readonly ClientesDbContext _context;

    public UsuarioAppRepository(ClientesDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // -------------------------------
    // CONSULTAS
    // -------------------------------

    public async Task<UsuarioAppEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.UsuariosApp
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdUsuario == id, cancellationToken);
    }

    public async Task<UsuarioAppEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.UsuariosApp
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == username, cancellationToken);
    }

    public async Task<UsuarioAppEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.UsuariosApp
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<UsuarioAppEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.UsuariosApp
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UsuarioAppEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.UsuariosApp
            .AsNoTracking()
            .Where(u => u.EstadoUsuario == "A" && u.FechaEliminacion == null)
            .ToListAsync(cancellationToken);
    }

    // -------------------------------
    // ESCRITURA
    // -------------------------------

    public async Task<UsuarioAppEntity> AddAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default)
    {
        await _context.UsuariosApp.AddAsync(usuario, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return usuario;
    }

    public async Task<bool> UpdateAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default)
    {
        var existing = await _context.UsuariosApp.FindAsync(new object[] { usuario.IdUsuario }, cancellationToken);
        if (existing == null) return false;

        _context.Entry(existing).CurrentValues.SetValues(usuario);
        existing.FechaActualizacion = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await _context.UsuariosApp.FindAsync(new object[] { id }, cancellationToken);
        if (usuario == null) return false;

        usuario.EstadoUsuario = "I";
        usuario.FechaEliminacion = DateTime.Now;

        _context.UsuariosApp.Update(usuario);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> HardDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await _context.UsuariosApp.FindAsync(new object[] { id }, cancellationToken);
        if (usuario == null) return false;

        _context.UsuariosApp.Remove(usuario);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    // -------------------------------
    // VALIDACIONES
    // -------------------------------

    public async Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.UsuariosApp
            .AsNoTracking()
            .Where(u => u.NombreUsuario == username);

        if (excludeId.HasValue)
            query = query.Where(u => u.IdUsuario != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.UsuariosApp
            .AsNoTracking()
            .Where(u => u.Email == email);

        if (excludeId.HasValue)
            query = query.Where(u => u.IdUsuario != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    // -------------------------------
    // SEGURIDAD
    // -------------------------------

    public async Task IncrementarIntentosFallidosAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await _context.UsuariosApp.FindAsync(new object[] { id }, cancellationToken);
        if (usuario == null) return;

        usuario.IntentosFallidos += 1;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ResetIntentosFallidosAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await _context.UsuariosApp.FindAsync(new object[] { id }, cancellationToken);
        if (usuario == null) return;

        usuario.IntentosFallidos = 0;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BloquearUsuarioAsync(int id, DateTime fechaBloqueo, CancellationToken cancellationToken = default)
    {
        var usuario = await _context.UsuariosApp.FindAsync(new object[] { id }, cancellationToken);
        if (usuario == null) return;

        usuario.FechaBloqueo = fechaBloqueo;
        usuario.EstadoUsuario = "B";

        await _context.SaveChangesAsync(cancellationToken);
    }
}