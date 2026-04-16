using System.Text.Json;
using Microservicio.Clientes.Api.Models.Common;
using Microservicio.Clientes.Business.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Microservicio.Clientes.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var apiError = new ApiErrorResponse();

            switch (exception)
            {
                case ValidationException vex:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    apiError.Title = "Error de validación";
                    apiError.Status = 400;
                    apiError.Detail = vex.Message;
                    if (!string.IsNullOrEmpty(vex.Campo))
                    {
                        apiError.Errors = new Dictionary<string, string[]>
                        {
                            { vex.Campo, new[] { vex.Message } }
                        };
                    }
                    break;

                case NotFoundException nfex:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    apiError.Title = "Recurso no encontrado";
                    apiError.Status = 404;
                    apiError.Detail = nfex.Message;
                    break;

                case UnauthorizedBusinessException uaex:
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    apiError.Title = "No autorizado";
                    apiError.Status = 401;
                    apiError.Detail = uaex.Message;
                    break;

                case BusinessException bex:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    apiError.Title = "Error de negocio";
                    apiError.Status = 400;
                    apiError.Detail = bex.Message;
                    break;

                default:
                    _logger.LogError(exception, "Error no controlado");
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    apiError.Title = "Error interno del servidor";
                    apiError.Status = 500;
                    apiError.Detail = "Ocurrió un error inesperado. Por favor, intente más tarde.";
                    break;
            }

            apiError.Type = $"https://httpstatuses.com/{response.StatusCode}";
            apiError.Timestamp = DateTime.UtcNow;

            var json = JsonSerializer.Serialize(apiError);
            await response.WriteAsync(json);
        }
    }
}