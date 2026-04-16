using Microservicio.Clientes.Business.DTOs.Cliente;
using Microservicio.Clientes.Business.Exceptions;
using System;

namespace Microservicio.Clientes.Business.Validators
{
    public static class ClienteValidator
    {
        public static void ValidateCrear(CrearClienteRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.CedulaRuc))
                throw new ValidationException("CedulaRuc", "La cédula/RUC es obligatoria");

            if (request.CedulaRuc.Length < 10 || request.CedulaRuc.Length > 13)
                throw new ValidationException("CedulaRuc", "La cédula/RUC debe tener entre 10 y 13 dígitos");

            foreach (char c in request.CedulaRuc)
                if (!char.IsDigit(c))
                    throw new ValidationException("CedulaRuc", "La cédula/RUC debe contener solo números");

            if (string.IsNullOrWhiteSpace(request.RazonSocial))
                throw new ValidationException("RazonSocial", "La razón social es obligatoria");

            if (request.RazonSocial.Length > 200)
                throw new ValidationException("RazonSocial", "La razón social no puede exceder 200 caracteres");

            if (request.Direccion?.Length > 300)
                throw new ValidationException("Direccion", "La dirección no puede exceder 300 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Correo))
            {
                if (request.Correo.Length > 150)
                    throw new ValidationException("Correo", "El correo no puede exceder 150 caracteres");
                try
                {
                    var addr = new System.Net.Mail.MailAddress(request.Correo);
                    if (addr.Address != request.Correo)
                        throw new Exception();
                }
                catch
                {
                    throw new ValidationException("Correo", "Formato de correo electrónico inválido");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Celular))
            {
                if (request.Celular.Length > 20)
                    throw new ValidationException("Celular", "El celular no puede exceder 20 caracteres");
                foreach (char c in request.Celular)
                    if (!char.IsDigit(c))
                        throw new ValidationException("Celular", "El celular debe contener solo números");
            }
        }

        public static void ValidateActualizar(ActualizarClienteRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.IdCliente <= 0)
                throw new ValidationException("IdCliente", "El ID del cliente debe ser mayor que cero");

            ValidateCrear(new CrearClienteRequest
            {
                CedulaRuc = request.CedulaRuc,
                RazonSocial = request.RazonSocial,
                Direccion = request.Direccion,
                Correo = request.Correo,
                Celular = request.Celular,
                Estado = request.Estado
            });
        }

        public static void ValidateFiltro(ClienteFiltroRequest filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.PageNumber < 1)
                throw new ValidationException("PageNumber", "El número de página debe ser mayor o igual a 1");

            if (filtro.PageSize < 1 || filtro.PageSize > 100)
                throw new ValidationException("PageSize", "El tamaño de página debe estar entre 1 y 100");

            if (!string.IsNullOrEmpty(filtro.SortBy))
            {
                var allowed = new[] { "IdCliente", "RazonSocial", "CedulaRuc", "CreatedAt", "Estado" };
                if (!allowed.Contains(filtro.SortBy, StringComparer.OrdinalIgnoreCase))
                    throw new ValidationException("SortBy", "Campo de ordenamiento no válido");
            }
        }
    }
}