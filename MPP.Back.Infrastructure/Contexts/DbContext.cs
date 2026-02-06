using Microsoft.EntityFrameworkCore;

namespace MPP.Back.Infrastructure.Contexts
{
    /// <summary>
    /// Contexto principal de Entity Framework Core.
    /// Representa la sesion de trabajo con la base de datos SQL Server.
    /// </summary>
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext, IServicesScoped
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}


