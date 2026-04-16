using Microservicio.Clientes.DataManagement.Interfaces;
using Microservicio.Clientes.DataManagement.Models;
using Microservicio.Clientes.DataManagement.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservicio.Clientes.DataManagement.Services;

public class ClienteDataService : IClienteDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClienteDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    // -------------------------------
    // CONSULTAS
    // -------------------------------

    public async Task<ClienteDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : ClienteDataMapper.ToModel(entity);
    }

    public async Task<IEnumerable<ClienteDataModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);
        return ClienteDataMapper.ToModelList(entities);
    }

    public async Task<IEnumerable<ClienteDataModel>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Clientes.GetAllActiveAsync(cancellationToken);
        return ClienteDataMapper.ToModelList(entities);
    }

    public async Task<IEnumerable<ClienteDataModel>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Clientes.SearchAsync(term, cancellationToken);
        return ClienteDataMapper.ToModelList(entities);
    }

    public async Task<DataPagedResult<ClienteDataModel>> GetPagedAsync(
        ClienteFiltroDataModel filtro,
        CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);

        var query = entities.AsQueryable();

        // Filtros
        if (!string.IsNullOrEmpty(filtro.RazonSocial))
            query = query.Where(c => c.RazonSocial.Contains(filtro.RazonSocial));

        if (!string.IsNullOrEmpty(filtro.CedulaRuc))
            query = query.Where(c => c.CedulaRuc.Contains(filtro.CedulaRuc));

        if (filtro.Estado.HasValue)
            query = query.Where(c => c.EstadoCli == filtro.Estado.Value);

        var totalCount = query.Count();

        // Ordenamiento
        query = filtro.SortBy.ToLower() switch
        {
            "razonsocial" => filtro.SortDescending
                ? query.OrderByDescending(c => c.RazonSocial)
                : query.OrderBy(c => c.RazonSocial),

            "cedularuc" => filtro.SortDescending
                ? query.OrderByDescending(c => c.CedulaRuc)
                : query.OrderBy(c => c.CedulaRuc),

            _ => filtro.SortDescending
                ? query.OrderByDescending(c => c.IdCliente)
                : query.OrderBy(c => c.IdCliente)
        };

        var items = query
            .Skip((filtro.PageNumber - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .ToList();

        return new DataPagedResult<ClienteDataModel>
        {
            Items = items.Select(ClienteDataMapper.ToModel),
            TotalCount = totalCount,
            PageNumber = filtro.PageNumber,
            PageSize = filtro.PageSize
        };
    }

    // -------------------------------
    // ESCRITURA
    // -------------------------------

    public async Task<ClienteDataModel> CreateAsync(ClienteDataModel model, CancellationToken cancellationToken = default)
    {
        var entity = ClienteDataMapper.ToEntity(model);

        await _unitOfWork.Clientes.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ClienteDataMapper.ToModel(entity);
    }

    public async Task<bool> UpdateAsync(ClienteDataModel model, CancellationToken cancellationToken = default)
    {
        var entity = ClienteDataMapper.ToEntity(model);

        var result = await _unitOfWork.Clientes.UpdateAsync(entity, cancellationToken);

        if (result)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.Clientes.SoftDeleteAsync(id, cancellationToken);

        if (result)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }

    // -------------------------------
    // VALIDACIONES
    // -------------------------------

    public Task<bool> ExistsByCedulaAsync(string cedulaRuc, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return _unitOfWork.Clientes.ExistsByCedulaAsync(cedulaRuc, excludeId, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(string correo, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        return _unitOfWork.Clientes.ExistsByEmailAsync(correo, excludeId, cancellationToken);
    }
}