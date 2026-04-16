using Microservicio.Clientes.Business.DTOs.Cliente;
using Microservicio.Clientes.DataManagement.Models;

namespace Microservicio.Clientes.Business.Mappers;

/// <summary>
/// Mapper para convertir entre DTOs de Business y DataManagement Models
/// </summary>
public static class ClienteBusinessMapper
{
    // ============================================================
    // Request → DataModel (para operaciones de escritura)
    // ============================================================

    /// <summary>
    /// Convierte CrearClienteRequest a ClienteDataModel
    /// </summary>
    public static ClienteDataModel ToDataModel(CrearClienteRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return new ClienteDataModel
        {
            IdCliente = 0,
            CedulaRuc = request.CedulaRuc,
            RazonSocial = request.RazonSocial,
            Direccion = request.Direccion,
            Correo = request.Correo,
            Celular = request.Celular,
            Estado = request.Estado,
            Eliminado = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            DeletedAt = null,
            CreatedBy = request.CreatedBy ?? string.Empty,
            UpdatedBy = string.Empty,
            DeletedBy = null,
            CreatedIp = request.CreatedIp,
            UpdatedIp = null,
            DeletedIp = null
        };
    }

    /// <summary>
    /// Convierte ActualizarClienteRequest a ClienteDataModel
    /// </summary>
    public static ClienteDataModel ToDataModel(ActualizarClienteRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return new ClienteDataModel
        {
            IdCliente = request.IdCliente,
            CedulaRuc = request.CedulaRuc,
            RazonSocial = request.RazonSocial,
            Direccion = request.Direccion,
            Correo = request.Correo,
            Celular = request.Celular,
            Estado = request.Estado,
            Eliminado = false, // No se modifica en actualización normal
            CreatedAt = DateTime.Now, // Temporal, se conservará el original
            UpdatedAt = DateTime.Now,
            DeletedAt = null,
            CreatedBy = string.Empty, // Temporal, se conservará el original
            UpdatedBy = request.UpdatedBy ?? string.Empty,
            DeletedBy = null,
            CreatedIp = null, // Temporal, se conservará el original
            UpdatedIp = request.UpdatedIp,
            DeletedIp = null
        };
    }

    // ============================================================
    // DataModel → Response (COMPLETO con auditoría)
    // ============================================================

    /// <summary>
    /// Convierte ClienteDataModel a ClienteResponse
    /// </summary>
    public static ClienteResponse ToResponse(ClienteDataModel model)
    {
        if (model == null)
            return null!;

        return new ClienteResponse
        {
            IdCliente = model.IdCliente,
            CedulaRuc = model.CedulaRuc,
            RazonSocial = model.RazonSocial,
            Direccion = model.Direccion,
            Correo = model.Correo,
            Celular = model.Celular,
            Estado = model.Estado,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy
        };
    }

    // ============================================================
    // Lista DataModel → Lista Response
    // ============================================================

    /// <summary>
    /// Convierte una lista de ClienteDataModel a lista de ClienteResponse
    /// </summary>
    public static IEnumerable<ClienteResponse> ToResponseList(IEnumerable<ClienteDataModel> models)
    {
        if (models == null)
            return Enumerable.Empty<ClienteResponse>();

        return models.Select(ToResponse).Where(r => r != null).ToList()!;
    }

    // ============================================================
    // Filtro Request → Filtro DataModel
    // ============================================================

    /// <summary>
    /// Convierte ClienteFiltroRequest a ClienteFiltroDataModel
    /// </summary>
    public static ClienteFiltroDataModel ToDataModel(ClienteFiltroRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        return new ClienteFiltroDataModel
        {
            RazonSocial = request.RazonSocial,
            CedulaRuc = request.CedulaRuc,
            Correo = request.Correo,
            Estado = request.Estado,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending
        };
    }

    // ============================================================
    // Actualización parcial (para Patch)
    // ============================================================

    /// <summary>
    /// Actualiza un ClienteDataModel existente con los valores de ActualizarClienteRequest
    /// Solo actualiza los campos que no son null en el request
    /// </summary>
    public static void UpdateDataModel(ActualizarClienteRequest request, ClienteDataModel existingModel)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (existingModel == null)
            throw new ArgumentNullException(nameof(existingModel));

        // Actualizar solo los campos que tienen valor
        if (!string.IsNullOrWhiteSpace(request.CedulaRuc))
            existingModel.CedulaRuc = request.CedulaRuc;

        if (!string.IsNullOrWhiteSpace(request.RazonSocial))
            existingModel.RazonSocial = request.RazonSocial;

        if (request.Direccion != null)
            existingModel.Direccion = request.Direccion;

        if (request.Correo != null)
            existingModel.Correo = request.Correo;

        if (request.Celular != null)
            existingModel.Celular = request.Celular;

        // Estado siempre se actualiza
        existingModel.Estado = request.Estado;

        // Auditoría
        existingModel.UpdatedAt = DateTime.Now;
        existingModel.UpdatedBy = request.UpdatedBy ?? existingModel.UpdatedBy;
        existingModel.UpdatedIp = request.UpdatedIp ?? existingModel.UpdatedIp;
    }

    // ============================================================
    // Para Soft Delete
    // ============================================================

    /// <summary>
    /// Marca un ClienteDataModel como eliminado (soft delete)
    /// </summary>
    public static void MarkAsDeleted(ClienteDataModel model, string? deletedBy, string? deletedIp)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        model.Eliminado = true;
        model.DeletedAt = DateTime.Now;
        model.DeletedBy = deletedBy;
        model.DeletedIp = deletedIp;
    }
}