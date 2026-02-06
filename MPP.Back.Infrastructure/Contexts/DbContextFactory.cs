using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MPP.Back.Infrastructure.Contexts
{
    /// <summary>
    /// Clase para crear el contexto en tiempo de diseno (migraciones EF Core).
    /// </summary>
    public class DbContextFactory : IDesignTimeDbContextFactory<DbContext>, IServicesSingleton
    {
        private const string ApiProjectPath = "../MPP.Back.API";
        private const string DatabaseSectionConnectionNamePath = "Database:ConnectionStringName";

        public DbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), ApiProjectPath);
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionStringName = configuration[DatabaseSectionConnectionNamePath]
                ?? throw new InvalidOperationException(
                    $"No se configuro '{DatabaseSectionConnectionNamePath}' en appsettings.");

            var connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException(
                    $"No se encontro la cadena de conexion '{connectionStringName}' para el entorno '{environment}'.");

            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure();
            });

            return new DbContext(optionsBuilder.Options);
        }
    }
}


