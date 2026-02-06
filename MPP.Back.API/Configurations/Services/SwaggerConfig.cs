using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MPP.Back.API.Configurations.Services
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                var securitySchemeName = configuration["Swagger:SecuritySchemeName"]
                    ?? throw new InvalidOperationException("No se configuro Swagger:SecuritySchemeName.");
                var securityDescription = configuration["Swagger:SecurityDescription"]
                    ?? throw new InvalidOperationException("No se configuro Swagger:SecurityDescription.");

           
                options.AddSecurityDefinition(securitySchemeName, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = securityDescription
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = securitySchemeName
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            return services;
        }

        private sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
        {
            private readonly IApiVersionDescriptionProvider _provider;
            private readonly IConfiguration _configuration;

            public ConfigureSwaggerOptions(
                IApiVersionDescriptionProvider provider,
                IConfiguration configuration)
            {
                _provider = provider;
                _configuration = configuration;
            }

            public void Configure(SwaggerGenOptions options)
            {
                var title = _configuration["Swagger:Title"]
                    ?? throw new InvalidOperationException("No se configuro Swagger:Title.");
                var description = _configuration["Swagger:Description"]
                    ?? throw new InvalidOperationException("No se configuro Swagger:Description.");

                foreach (var apiVersionDescription in _provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(apiVersionDescription.GroupName, new OpenApiInfo
                    {
                        Title = title,
                        Version = apiVersionDescription.ApiVersion.ToString(),
                        Description = description
                    });
                }
            }
        }
    }
}


