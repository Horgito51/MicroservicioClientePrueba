using Microservicio.Clientes.DataAccess.Context;
using Microservicio.Clientes.DataAccess.Repositories;
using Microservicio.Clientes.DataAccess.Repositories.Interfaces;
using Microservicio.Clientes.DataManagement.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataManagement.Services;

public  class UnitOfWork : IUnitOfWork
{
    private readonly ClientesDbContext _context;

    private IDbContextTransaction? _transaction;

    // -------------------------------
    // Repositorios
    // -------------------------------
    public IClienteRepository Clientes { get; }

    public IUsuarioAppRepository Usuarios { get; }

    // -------------------------------
    // Constructor
    // -------------------------------
    public UnitOfWork(ClientesDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        // Inicializar repositorios
        Clientes = new ClienteRepository(_context);
        Usuarios = new UsuarioAppRepository(_context);
    }

    // -------------------------------
    // Guardar cambios
    // -------------------------------
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    // -------------------------------
    // Transacciones
    // -------------------------------
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // -------------------------------
    // Dispose
    // -------------------------------
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}