using MPP.Back.API.Configurations.Services;
using MPP.Back.Application.DI;
using MPP.Back.Infrastructure.DI;

namespace MPP.Back.API.Extentions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization();
            services.AddControllers();
            services.ConfigureApiVersioning(configuration);
            services.AddSwaggerConfiguration(configuration);
        

            services.AddApplication();
            services.AddInfrastructure(configuration);


            services.ConfigurationCors(configuration);

            services.ConfigureRateLimiterGlobal(configuration);
            services.AddMemoryCache();

            return services;
        }
    }
}


