using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MPP.Back.API.Configurations.Services;
using MPP.Back.API.Middlewares;
using Serilog;
using MPP.Back.API.Extentions;
using MPP.Back.Application.Dtos.Auth.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.Configure<AuditLoggingOptions>(
    builder.Configuration.GetSection(AuditLoggingOptions.SectionName));

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.ConfigurationJwtBearer(builder.Configuration);

builder.Services.RegisterDependencies(builder.Configuration);

var app = builder.Build();

var corsPolicyName = app.Configuration["Cors:PolicyName"]
    ?? throw new InvalidOperationException("No se configuro Cors:PolicyName.");

var swaggerRoutePrefix = app.Configuration["Swagger:RoutePrefix"]
    ?? throw new InvalidOperationException("No se configuro Swagger:RoutePrefix.");

var swaggerRouteTemplate = app.Configuration["Swagger:RouteTemplate"]
    ?? throw new InvalidOperationException("No se configuro Swagger:RouteTemplate.");
var auditExcludedPaths = app.Configuration
    .GetSection($"{AuditLoggingOptions.SectionName}:ExcludedPaths")
    .Get<string[]>()
    ?? Array.Empty<string>();

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.UseSwagger(options =>
{
    options.RouteTemplate = swaggerRouteTemplate;
});

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = swaggerRoutePrefix;

    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});

app.UseWhen(context => !IsExcludedPath(context.Request.Path, auditExcludedPaths), branch =>
{
    branch.UseSerilogRequestLogging();
    branch.UseMiddleware<RequestResponseLoggingMiddleware>();
});
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

try
{
    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "La API finalizo de forma inesperada.");
}
finally
{
    Log.CloseAndFlush();
}

static bool IsExcludedPath(PathString requestPath, IEnumerable<string> excludedPaths)
{
    return excludedPaths.Any(path =>
        !string.IsNullOrWhiteSpace(path) &&
        requestPath.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase));
}
