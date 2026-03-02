using Partient.TestProject.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Partient.TestProject.Web.Middleware
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
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Запрос был отменен пользователем.");
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
            catch (FhirSearchException ex)
            {
                _logger.LogError(ex, "Ошибка поиска FHIR: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest, "invalid_date_format");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Непредвиденная ошибка сервера");
                await HandleExceptionAsync(context, "Внутренняя ошибка сервера", HttpStatusCode.InternalServerError, "server_error");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode code, string errorCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                code = errorCode,
                timestamp = DateTime.UtcNow
            });

            return context.Response.WriteAsync(result);
        }
    }
}
