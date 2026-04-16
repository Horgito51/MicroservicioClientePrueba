using Microservicio.Clientes.DataAccess.Repositories.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataManagement.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // -------------------------------
    // Repositorios
    // -------------------------------
    IClienteRepository Clientes { get; }

    IUsuarioAppRepository Usuarios { get; }

    // (opcional luego)
    // IRolRepository Roles { get; }

    // -------------------------------
    // Persistencia
    // -------------------------------
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // -------------------------------
    // Transacciones (nivel pro)
    // -------------------------------
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}