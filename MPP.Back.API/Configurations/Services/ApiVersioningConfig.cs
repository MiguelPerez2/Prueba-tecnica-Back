using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace MPP.Back.API.Configurations.Services
{
    public static class ApiVersioningConfig
    {
        public static IServiceCollection ConfigureApiVersioning(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var defaultMajorVersion = configuration.GetValue<int>("ApiVersioning:DefaultMajorVersion");
            var defaultMinorVersion = configuration.GetValue<int>("ApiVersioning:DefaultMinorVersion");
            var assumeDefaultWhenUnspecified = configuration.GetValue<bool>("ApiVersioning:AssumeDefaultVersionWhenUnspecified");
            var reportApiVersions = configuration.GetValue<bool>("ApiVersioning:ReportApiVersions");
            var groupNameFormat = configuration["ApiVersioning:GroupNameFormat"]
                ?? throw new InvalidOperationException("No se configuro ApiVersioning:GroupNameFormat.");
            var substituteApiVersionInUrl = configuration.GetValue<bool>("ApiVersioning:SubstituteApiVersionInUrl");

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(defaultMajorVersion, defaultMinorVersion);
                options.AssumeDefaultVersionWhenUnspecified = assumeDefaultWhenUnspecified;
                options.ReportApiVersions = reportApiVersions;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = groupNameFormat;
                options.SubstituteApiVersionInUrl = substituteApiVersionInUrl;
            });

            return services;
        }
    }
}


