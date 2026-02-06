using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MPP.Back.Infrastructure.Extentions;
using MPP.Back.Shared.Extensions;
using System.Reflection;

namespace MPP.Back.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.ConfigureDatabase(configuration);

            #region References automatic to all Repositories Or Services
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());
            #endregion

            return services;
        }
    }
}


