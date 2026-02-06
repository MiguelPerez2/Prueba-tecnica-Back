using System.Net;
using System.Threading.RateLimiting;

namespace MPP.Back.API.Configurations.Services
{
    public static class RateLimiterGlobalConfiguration
    {
        public static IServiceCollection ConfigureRateLimiterGlobal(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    double retryAfterSeconds = Convert.ToInt32(configuration["RateLimiter:retryAfterSeconds"]);
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        retryAfterSeconds = retryAfter.TotalSeconds;
                    }

                    var errorResponse = new
                    {
                        Code = HttpStatusCode.TooManyRequests,
                        Traceid = context.HttpContext.TraceIdentifier,
                        Message = "Se ha excedido el límite de peticiones permitido por el servidor."
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(errorResponse, token);
                };

                // Se aplica a todas las peticiones entrantes
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var clientIp = httpContext.Connection.RemoteIpAddress?.ToString()!;

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: clientIp,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = Convert.ToInt32(configuration["RateLimiter:PermitLimit"]),
                            Window = TimeSpan.FromSeconds(Convert.ToInt32(configuration["RateLimiter:PermitLimitGlobalBySeconds"])),
                            QueueLimit = 0,
                            AutoReplenishment = true
                        });
                });
            });
            return services;
        }
    }
}
