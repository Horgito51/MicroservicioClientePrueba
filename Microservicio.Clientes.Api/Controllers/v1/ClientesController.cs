using Microservicio.Clientes.Api.Models.Common;
using Microservicio.Clientes.Business.DTOs.Cliente;
using Microservicio.Clientes.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Clientes.Api.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClienteResponse>>>> GetAll()
        {
            var result = await _clienteService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<ClienteResponse>>.Ok(result));
        }

        [HttpGet("active")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClienteResponse>>>> GetAllActive()
        {
            var result = await _clienteService.GetAllActiveAsync();
            return Ok(ApiResponse<IEnumerable<ClienteResponse>>.Ok(result));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ClienteResponse>>> GetById(int id)
        {
            var result = await _clienteService.GetByIdAsync(id);
            return Ok(ApiResponse<ClienteResponse>.Ok(result));
        }

        [HttpGet("cedula/{cedulaRuc}")]
        public async Task<ActionResult<ApiResponse<ClienteResponse>>> GetByCedula(string cedulaRuc)
        {
            var result = await _clienteService.GetByCedulaAsync(cedulaRuc);
            return Ok(ApiResponse<ClienteResponse>.Ok(result));
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClienteResponse>>>> Search([FromQuery] string term)
        {
            var result = await _clienteService.SearchAsync(term);
            return Ok(ApiResponse<IEnumerable<ClienteResponse>>.Ok(result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ClienteResponse>>> Create([FromBody] CrearClienteRequest request)
        {
            request.CreatedBy = User.Identity?.Name ?? "api";
            request.CreatedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _clienteService.CreateAsync(request);
            return Ok(ApiResponse<ClienteResponse>.Ok(result, "Cliente creado exitosamente"));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> Update([FromBody] ActualizarClienteRequest request)
        {
            request.UpdatedBy = User.Identity?.Name ?? "api";
            request.UpdatedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _clienteService.UpdateAsync(request);
            return Ok(ApiResponse<bool>.Ok(result, "Cliente actualizado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var deletedBy = User.Identity?.Name ?? "api";
            var deletedIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _clienteService.DeleteAsync(id, deletedBy, deletedIp);
            return Ok(ApiResponse<bool>.Ok(result, "Cliente eliminado exitosamente"));
        }
    }
}