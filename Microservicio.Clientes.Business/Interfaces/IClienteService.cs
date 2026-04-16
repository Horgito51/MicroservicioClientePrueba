using Microservicio.Clientes.Business.DTOs.Cliente;
using Microservicio.Clientes.DataManagement.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.Business.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ClienteResponse> GetByCedulaAsync(string cedulaRuc, CancellationToken cancellationToken = default);
        Task<IEnumerable<ClienteResponse>> GetAllActiveAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ClienteResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        
        Task<IEnumerable<ClienteResponse>> SearchAsync(string term, CancellationToken cancellationToken = default);
        Task<ClienteResponse> CreateAsync(CrearClienteRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(ActualizarClienteRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, string? deletedBy, string? deletedIp, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCedulaAsync(string cedulaRuc, int? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string correo, int? excludeId = null, CancellationToken cancellationToken = default);
    }
}