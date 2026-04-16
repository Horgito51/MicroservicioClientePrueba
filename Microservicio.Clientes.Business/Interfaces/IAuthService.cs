using Microservicio.Clientes.Business.DTOs.Auth;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
        Task LogoutAsync(int userId, string token, CancellationToken cancellationToken = default);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
        Task<(int userId, string username, List<string> roles)> GetCurrentUserAsync(string token, CancellationToken cancellationToken = default);
    }
}