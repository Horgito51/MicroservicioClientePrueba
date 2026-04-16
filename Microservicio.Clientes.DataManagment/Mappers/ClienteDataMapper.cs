using Microservicio.Clientes.DataAccess.Entities;
using Microservicio.Clientes.DataManagement.Models;

namespace Microservicio.Clientes.DataManagement.Mappers;

public static class ClienteDataMapper
{
    // -------------------------------
    // Entity → DataModel (COMPLETO)
    // -------------------------------
    public static ClienteDataModel ToModel(ClienteEntity entity)
    {
        if (entity == null) return null!;

        return new ClienteDataModel
        {
            IdCliente = entity.IdCliente,
            CedulaRuc = entity.CedulaRuc,
            RazonSocial = entity.RazonSocial,
            Direccion = entity.Direccion,
            Correo = entity.Correo,
            Celular = entity.Celular,
            Estado = entity.EstadoCli,
            Eliminado = entity.Eliminado,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedBy = entity.UpdatedBy,
            DeletedBy = entity.DeletedBy,
            CreatedIp = entity.CreatedIp,
            UpdatedIp = entity.UpdatedIp,
            DeletedIp = entity.DeletedIp
        };
    }

    // -------------------------------
    // DataModel → Entity (COMPLETO)
    // -------------------------------
    public static ClienteEntity ToEntity(ClienteDataModel model)
    {
        if (model == null) return null!;

        return new ClienteEntity
        {
            IdCliente = model.IdCliente,
            CedulaRuc = model.CedulaRuc,
            RazonSocial = model.RazonSocial,
            Direccion = model.Direccion,
            Correo = model.Correo,
            Celular = model.Celular,
            EstadoCli = model.Estado,
            Eliminado = model.Eliminado,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            DeletedAt = model.DeletedAt,
            CreatedBy = model.CreatedBy,
            UpdatedBy = model.UpdatedBy,
            DeletedBy = model.DeletedBy,
            CreatedIp = model.CreatedIp,
            UpdatedIp = model.UpdatedIp,
            DeletedIp = model.DeletedIp
        };
    }

    // -------------------------------
    // Lista Entity → Lista Model
    // -------------------------------
    public static IEnumerable<ClienteDataModel> ToModelList(IEnumerable<ClienteEntity> entities)
    {
        return entities?.Select(ToModel) ?? Enumerable.Empty<ClienteDataModel>();
    }

    // -------------------------------
    // Lista Model → Lista Entity
    // -------------------------------
    public static IEnumerable<ClienteEntity> ToEntityList(IEnumerable<ClienteDataModel> models)
    {
        return models?.Select(ToEntity) ?? Enumerable.Empty<ClienteEntity>();
    }
}