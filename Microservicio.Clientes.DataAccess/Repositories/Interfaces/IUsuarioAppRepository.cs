using Microservicio.Clientes.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataAccess.Repositories.Interfaces;

public interface IUsuarioAppRepository
{
    // -------------------------------
    // Consultas
    // -------------------------------
    Task<UsuarioAppEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<UsuarioAppEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<UsuarioAppEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<IEnumerable<UsuarioAppEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<UsuarioAppEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    // -------------------------------
    // Escritura
    // -------------------------------
    Task<UsuarioAppEntity> AddAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default);

    Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> HardDeleteAsync(int id, CancellationToken cancellationToken = default);

    // -------------------------------
    // Seguridad
    // -------------------------------
    Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null, CancellationToken cancellationToken = default);

    Task IncrementarIntentosFallidosAsync(int id, CancellationToken cancellationToken = default);

    Task ResetIntentosFallidosAsync(int id, CancellationToken cancellationToken = default);

    Task BloquearUsuarioAsync(int id, DateTime fechaBloqueo, CancellationToken cancellationToken = default);
}