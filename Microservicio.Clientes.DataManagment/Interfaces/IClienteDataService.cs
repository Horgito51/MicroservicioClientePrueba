using Microservicio.Clientes.DataManagement.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataManagement.Interfaces;

public interface IClienteDataService
{
    // -------------------------------
    // Consultas
    // -------------------------------

    Task<ClienteDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<ClienteDataModel>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<ClienteDataModel>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    Task<DataPagedResult<ClienteDataModel>> GetPagedAsync(
        ClienteFiltroDataModel filtro,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ClienteDataModel>> SearchAsync(string term, CancellationToken cancellationToken = default);

    // -------------------------------
    // Escritura
    // -------------------------------

    Task<ClienteDataModel> CreateAsync(ClienteDataModel model, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(ClienteDataModel model, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    // -------------------------------
    // Validaciones
    // -------------------------------

    Task<bool> ExistsByCedulaAsync(string cedulaRuc, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(string correo, int? excludeId = null, CancellationToken cancellationToken = default);
}