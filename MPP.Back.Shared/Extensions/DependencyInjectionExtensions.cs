using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MPP.Back.Shared.Extensions
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Registra automáticamente los servicios de un ensamblado específico.
        /// </summary>
        /// <param name="services">Colección de servicios de Dependency Injections.</param>
        /// <param name="assembly">El ensamblado donde se buscarán los servicios.</param>
        public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.Scan(scan => scan
                    .FromAssemblies(assembly)
                    .AddClasses(classes => classes.AssignableTo<IServicesScoped>())
                        .AsImplementedInterfaces(i => i != typeof(IServicesScoped))
                        .WithScopedLifetime()
                    .AddClasses(classes => classes.AssignableTo<IServicesSingleton>())
                        .AsImplementedInterfaces(i => i != typeof(IServicesSingleton))
                        .WithSingletonLifetime()
                    .AddClasses(classes => classes.AssignableTo<IServicesTransient>())
                        .AsImplementedInterfaces(i => i != typeof(IServicesTransient))
                        .WithTransientLifetime()
            );
            return services;
        }
    }
}
