using Microservicio.Clientes.Business.DTOs.Cliente;
using Microservicio.Clientes.Business.Exceptions;
using Microservicio.Clientes.Business.Interfaces;
using Microservicio.Clientes.Business.Mappers;
using Microservicio.Clientes.DataManagement.Interfaces;
using Microservicio.Clientes.DataManagement.Mappers;
using Microservicio.Clientes.DataManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClienteResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                throw new NotFoundException("Cliente", id);

            var model = ClienteDataMapper.ToModel(entity);
            return ClienteBusinessMapper.ToResponse(model);
        }

        public async Task<ClienteResponse> GetByCedulaAsync(string cedulaRuc, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cedulaRuc))
                throw new ValidationException("CedulaRuc", "La cédula/RUC es obligatoria");

            var entity = await _unitOfWork.Clientes.GetByCedulaAsync(cedulaRuc, cancellationToken);
            if (entity == null)
                throw new NotFoundException("Cliente", "cédula/RUC", cedulaRuc);

            var model = ClienteDataMapper.ToModel(entity);
            return ClienteBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<ClienteResponse>> GetAllActiveAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.Clientes.GetAllActiveAsync(cancellationToken);
            var models = ClienteDataMapper.ToModelList(entities);
            return ClienteBusinessMapper.ToResponseList(models);
        }

        public async Task<IEnumerable<ClienteResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);
            var activeEntities = entities.Where(e => !e.Eliminado);
            var models = ClienteDataMapper.ToModelList(activeEntities);
            return ClienteBusinessMapper.ToResponseList(models);
        }

        public async Task<DataPagedResult<ClienteResponse>> GetPagedAsync(ClienteFiltroRequest filtro, CancellationToken cancellationToken = default)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            // Obtener todos los clientes no eliminados
            var entities = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);
            var query = entities.Where(e => !e.Eliminado).AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(filtro.RazonSocial))
                query = query.Where(c => c.RazonSocial.Contains(filtro.RazonSocial, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filtro.CedulaRuc))
                query = query.Where(c => c.CedulaRuc.Contains(filtro.CedulaRuc));

            if (!string.IsNullOrWhiteSpace(filtro.Correo) && filtro.Correo != "null")
                query = query.Where(c => c.Correo != null && c.Correo.Contains(filtro.Correo));

            if (filtro.Estado.HasValue)
                query = query.Where(c => c.EstadoCli == filtro.Estado.Value);

            var totalCount = query.Count();

            // Ordenamiento
            query = filtro.SortBy?.ToLower() switch
            {
                "razonsocial" => filtro.SortDescending ? query.OrderByDescending(c => c.RazonSocial) : query.OrderBy(c => c.RazonSocial),
                "cedularuc" => filtro.SortDescending ? query.OrderByDescending(c => c.CedulaRuc) : query.OrderBy(c => c.CedulaRuc),
                "createdat" => filtro.SortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                "estado" => filtro.SortDescending ? query.OrderByDescending(c => c.EstadoCli) : query.OrderBy(c => c.EstadoCli),
                _ => filtro.SortDescending ? query.OrderByDescending(c => c.IdCliente) : query.OrderBy(c => c.IdCliente)
            };

            // Paginación
            var pageSize = filtro.PageSize < 1 ? 10 : (filtro.PageSize > 100 ? 100 : filtro.PageSize);
            var pageNumber = filtro.PageNumber < 1 ? 1 : filtro.PageNumber;

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var models = ClienteDataMapper.ToModelList(items);
            var responses = ClienteBusinessMapper.ToResponseList(models);

            return new DataPagedResult<ClienteResponse>
            {
                Items = responses,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<ClienteResponse>> SearchAsync(string term, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<ClienteResponse>();

            var entities = await _unitOfWork.Clientes.SearchAsync(term, cancellationToken);
            var activeEntities = entities.Where(e => !e.Eliminado);
            var models = ClienteDataMapper.ToModelList(activeEntities);
            return ClienteBusinessMapper.ToResponseList(models);
        }

        public async Task<ClienteResponse> CreateAsync(CrearClienteRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (await _unitOfWork.Clientes.ExistsByCedulaAsync(request.CedulaRuc, null, cancellationToken))
                throw new ValidationException("CedulaRuc", $"Ya existe un cliente con la cédula/RUC: {request.CedulaRuc}");

            if (!string.IsNullOrWhiteSpace(request.Correo) && await _unitOfWork.Clientes.ExistsByEmailAsync(request.Correo, null, cancellationToken))
                throw new ValidationException("Correo", $"Ya existe un cliente con el correo: {request.Correo}");

            var model = ClienteBusinessMapper.ToDataModel(request);
            var entity = ClienteDataMapper.ToEntity(model);

            entity.CreatedBy = request.CreatedBy ?? "system";
            entity.CreatedIp = request.CreatedIp;
            entity.UpdatedAt = DateTime.Now;
            entity.Eliminado = false;

            var createdEntity = await _unitOfWork.Clientes.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdModel = ClienteDataMapper.ToModel(createdEntity);
            return ClienteBusinessMapper.ToResponse(createdModel);
        }

        public async Task<bool> UpdateAsync(ActualizarClienteRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existingEntity = await _unitOfWork.Clientes.ObtenerParaActualizarAsync(request.IdCliente, cancellationToken);
            if (existingEntity == null)
                throw new NotFoundException("Cliente", request.IdCliente);

            if (existingEntity.Eliminado)
                throw new BusinessException("CLI_002", "No se puede actualizar un cliente eliminado");

            if (await _unitOfWork.Clientes.ExistsByCedulaAsync(request.CedulaRuc, request.IdCliente, cancellationToken))
                throw new ValidationException("CedulaRuc", $"Ya existe otro cliente con la cédula/RUC: {request.CedulaRuc}");

            if (!string.IsNullOrWhiteSpace(request.Correo) && await _unitOfWork.Clientes.ExistsByEmailAsync(request.Correo, request.IdCliente, cancellationToken))
                throw new ValidationException("Correo", $"Ya existe otro cliente con el correo: {request.Correo}");

            existingEntity.CedulaRuc = request.CedulaRuc;
            existingEntity.RazonSocial = request.RazonSocial;
            existingEntity.Direccion = request.Direccion;
            existingEntity.Correo = request.Correo;
            existingEntity.Celular = request.Celular;
            existingEntity.EstadoCli = request.Estado;
            existingEntity.UpdatedBy = request.UpdatedBy ?? "system";
            existingEntity.UpdatedIp = request.UpdatedIp;
            existingEntity.UpdatedAt = DateTime.Now;

            var result = await _unitOfWork.Clientes.UpdateAsync(existingEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<bool> DeleteAsync(int id, string? deletedBy, string? deletedIp, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);
            if (existingEntity == null)
                throw new NotFoundException("Cliente", id);

            if (existingEntity.Eliminado)
                throw new BusinessException("CLI_003", "El cliente ya está eliminado");

            existingEntity.Eliminado = true;
            existingEntity.DeletedAt = DateTime.Now;
            existingEntity.DeletedBy = deletedBy ?? "system";
            existingEntity.DeletedIp = deletedIp;

            var result = await _unitOfWork.Clientes.UpdateAsync(existingEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<bool> ExistsByCedulaAsync(string cedulaRuc, int? excludeId = null, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Clientes.ExistsByCedulaAsync(cedulaRuc, excludeId, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string correo, int? excludeId = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            return await _unitOfWork.Clientes.ExistsByEmailAsync(correo, excludeId, cancellationToken);
        }
    }
}