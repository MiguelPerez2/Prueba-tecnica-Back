using Microsoft.Extensions.Options;
using MPP.Back.API.Configurations.Services;
using MPP.Back.Application.Common.Exceptions;
using MPP.Back.Application.Common.Responses;

namespace MPP.Back.API.Middlewares
{
    /// <summary>
    /// Middleware global para capturar excepciones no controladas
    /// y devolver respuestas estandarizadas.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly AuditLoggingOptions _auditLoggingOptions;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IOptions<AuditLoggingOptions> auditLoggingOptions)
        {
            _next = next;
            _logger = logger;
            _auditLoggingOptions = auditLoggingOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (!IsExcludedPath(context.Request.Path))
                {
                    _logger.LogError(
                        exception,
                        "Unhandled exception | TraceId: {TraceId} | {Method} {Path}",
                        context.TraceIdentifier,
                        context.Request.Method,
                        context.Request.Path);
                }

                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                ExceptionsError knownException => (knownException.Error.StatusCode, knownException.Error.Message),
                _ => (StatusCodes.Status500InternalServerError, "Ocurrio un error interno en el servidor.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse(statusCode, message);
            return context.Response.WriteAsJsonAsync(response);
        }

        private bool IsExcludedPath(PathString requestPath)
        {
            return _auditLoggingOptions.ExcludedPaths.Any(path =>
                !string.IsNullOrWhiteSpace(path) &&
                requestPath.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase));
        }
    }
}
