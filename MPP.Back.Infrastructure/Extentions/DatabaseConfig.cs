using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InfrastructureDbContext = MPP.Back.Infrastructure.Contexts.DbContext;

namespace MPP.Back.Infrastructure.Extentions
{
    public static class DatabaseConfig
    {
        private const string DatabaseConnectionNamePath = "Database:ConnectionStringName";
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringName = configuration[DatabaseConnectionNamePath]
                ?? throw new InvalidOperationException(
                    $"No se configuro '{DatabaseConnectionNamePath}' en appsettings.");

            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException(
                    $"No se encontro la cadena de conexion '{connectionStringName}'.");

            services.AddDbContext<InfrastructureDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure();
                });
            });
            return services;
        }
    }
}