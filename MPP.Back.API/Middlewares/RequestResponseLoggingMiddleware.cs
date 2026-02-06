using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Options;
using MPP.Back.API.Configurations.Services;

namespace MPP.Back.API.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private static readonly string[] AllowedContentTypes =
        [
            "application/json",
            "application/xml",
            "text/plain",
            "text/html",
            "application/x-www-form-urlencoded"
        ];

        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly AuditLoggingOptions _options;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger,
            IOptions<AuditLoggingOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_options.Enabled || IsExcludedPath(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var requestBody = _options.LogRequestBody
                ? await ReadRequestBodyAsync(context.Request)
                : "Request body logging disabled.";

            var originalResponseBodyStream = context.Response.Body;
            await using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var responseBody = _options.LogResponseBody
                ? await ReadResponseBodyAsync(context.Response, responseBodyStream)
                : "Response body logging disabled.";

            _logger.LogInformation(
                "AUDIT {Method} {Path} {StatusCode} in {ElapsedMilliseconds}ms | TraceId {TraceId} | Request: {RequestBody} | Response: {ResponseBody}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                context.TraceIdentifier,
                requestBody,
                responseBody);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            context.Response.Body = originalResponseBodyStream;
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            if (!HasSupportedContentType(request.ContentType) || request.ContentLength is 0)
            {
                return "Request body omitted.";
            }

            request.EnableBuffering();

            using var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return Truncate(body);
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response, Stream responseBodyStream)
        {
            if (!HasSupportedContentType(response.ContentType))
            {
                return "Response body omitted.";
            }

            responseBodyStream.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(responseBodyStream, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            return Truncate(body);
        }

        private static bool HasSupportedContentType(string? contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return false;
            }

            return AllowedContentTypes.Any(contentType.Contains);
        }

        private string Truncate(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "Body empty.";
            }

            if (text.Length <= _options.BodySizeLimit)
            {
                return text;
            }

            return $"{text[.._options.BodySizeLimit]}...(truncated)";
        }

        private bool IsExcludedPath(PathString requestPath)
        {
            return _options.ExcludedPaths.Any(path =>
                !string.IsNullOrWhiteSpace(path) &&
                requestPath.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase));
        }
    }
}
