using Microservicio.Clientes.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataAccess.Repositories.Interfaces;

public interface IClienteRepository
{
    Task<ClienteEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ClienteEntity?> GetByCedulaAsync(string cedulaRuc, CancellationToken cancellationToken = default);

    Task<IEnumerable<ClienteEntity>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<ClienteEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<ClienteEntity>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task<ClienteEntity> AddAsync(ClienteEntity cliente, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(ClienteEntity cliente, CancellationToken cancellationToken = default);

    Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> HardDeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCedulaAsync(string cedulaRuc, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string correo, int? excludeId = null, CancellationToken cancellationToken = default);
   
    Task<ClienteEntity?> ObtenerParaActualizarAsync(int id, CancellationToken cancellationToken = default);


}