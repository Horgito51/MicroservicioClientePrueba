using Microservicio.Clientes.Business.DTOs.Auth;
using Microservicio.Clientes.Business.Exceptions;
using Microservicio.Clientes.Business.Interfaces;
using Microservicio.Clientes.DataAccess.Entities;
using Microservicio.Clientes.DataManagement.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ValidationException("Username", "El nombre de usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException("Password", "La contraseña es obligatoria");

            var usuario = await _unitOfWork.Usuarios.GetByUsernameAsync(request.Username, cancellationToken);

            if (usuario == null)
                usuario = await _unitOfWork.Usuarios.GetByEmailAsync(request.Username, cancellationToken);

            if (usuario == null)
                throw new UnauthorizedBusinessException("AUTH_001", "Credenciales inválidas");

            if (usuario.PasswordHash != request.Password)
            {
                await _unitOfWork.Usuarios.IncrementarIntentosFallidosAsync(usuario.IdUsuario, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                throw new UnauthorizedBusinessException("AUTH_002", "Credenciales inválidas");
            }

            if (usuario.EstadoUsuario == "I")
                throw new UnauthorizedBusinessException("AUTH_003", "Usuario inactivo");

            if (usuario.EstadoUsuario == "B")
                throw new UnauthorizedBusinessException("AUTH_004", "Usuario bloqueado");

            await _unitOfWork.Usuarios.ResetIntentosFallidosAsync(usuario.IdUsuario, cancellationToken);

            usuario.UltimoAcceso = DateTime.Now;
            await _unitOfWork.Usuarios.UpdateAsync(usuario, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var roles = new List<string> { "User" };
            if (usuario.NombreUsuario == "admin")
                roles.Add("Admin");

            var token = GenerarTokenJWT(usuario, roles);

            var expirationMinutesStr = _configuration["Jwt:ExpirationMinutes"];
            var expirationMinutes = string.IsNullOrEmpty(expirationMinutesStr) ? 60 : int.Parse(expirationMinutesStr);

            return new LoginResponse
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = expirationMinutes * 60,
                ExpiresAt = DateTime.Now.AddMinutes(expirationMinutes),
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Roles = roles
            };
        }

        public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecret = _configuration["Jwt:Secret"] ?? "MiClaveSuperSecretaParaJWT1234567890!";
                var key = Encoding.ASCII.GetBytes(jwtSecret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task LogoutAsync(int userId, string token, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ValidationException("CurrentPassword", "La contraseña actual es obligatoria");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ValidationException("NewPassword", "La nueva contraseña es obligatoria");

            if (newPassword.Length < 6)
                throw new ValidationException("NewPassword", "La nueva contraseña debe tener al menos 6 caracteres");

            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(userId, cancellationToken);
            if (usuario == null)
                throw new NotFoundException("Usuario", userId);

            if (usuario.PasswordHash != currentPassword)
                throw new UnauthorizedBusinessException("AUTH_006", "Contraseña actual incorrecta");

            usuario.PasswordHash = newPassword;
            usuario.RequiereCambioPassword = false;
            usuario.FechaActualizacion = DateTime.Now;

            var result = await _unitOfWork.Usuarios.UpdateAsync(usuario, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<(int userId, string username, List<string> roles)> GetCurrentUserAsync(string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedBusinessException("AUTH_007", "Token no proporcionado");

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecret = _configuration["Jwt:Secret"] ?? "MiClaveSuperSecretaParaJWT1234567890!";
                var key = Encoding.ASCII.GetBytes(jwtSecret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                var rolesClaims = jwtToken.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();

                return (int.Parse(userIdClaim), usernameClaim ?? string.Empty, rolesClaims);
            }
            catch
            {
                throw new UnauthorizedBusinessException("AUTH_009", "Token inválido");
            }
        }

        private string GenerarTokenJWT(UsuarioAppEntity usuario, List<string> roles)
        {
            var jwtSecret = _configuration["Jwt:Secret"] ?? "MiClaveSuperSecretaParaJWT1234567890!";
            var expirationMinutesStr = _configuration["Jwt:ExpirationMinutes"];
            var expirationMinutes = string.IsNullOrEmpty(expirationMinutesStr) ? 60 : int.Parse(expirationMinutesStr);
            var issuer = _configuration["Jwt:Issuer"] ?? "Microservicio.Clientes";
            var audience = _configuration["Jwt:Audience"] ?? "Microservicio.Clientes.API";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim("nombres", usuario.Nombres ?? ""),
        new Claim("apellidos", usuario.Apellidos ?? "")
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                // 🔥 AQUÍ ESTÁ LA CORRECCIÓN
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),

                Issuer = issuer,
                Audience = audience,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}