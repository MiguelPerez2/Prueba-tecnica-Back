using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MPP.Back.Shared.Extensions;
using System.Reflection;

namespace MPP.Back.Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            #region References automatic to all Repositories Or Services
            services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());
            #endregion

            return services;
        }
    }
}
