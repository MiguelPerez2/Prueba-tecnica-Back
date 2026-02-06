namespace MPP.Back.API.Configurations.Services
{
    public static class CorsConfig
    {
        public static IServiceCollection ConfigurationCors( this IServiceCollection services, IConfiguration configuration)
        {
            var corsPolicyName = configuration["Cors:PolicyName"]
               ?? throw new InvalidOperationException("No se configuro Cors:PolicyName.");
            var corsOrigins = configuration.GetSection("Cors:Origins").Get<string[]>()
                ?? throw new InvalidOperationException("No se configuro Cors:Origins.");

            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, policyBuilder =>
                {
                    policyBuilder.WithOrigins(corsOrigins);
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
