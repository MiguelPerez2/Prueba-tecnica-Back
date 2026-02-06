using Microsoft.EntityFrameworkCore;
using MPP.Back.Application.Interfaces.Repository;
using MPP.Back.Domain.Entities;
using InfrastructureDbContext = MPP.Back.Infrastructure.Contexts.DbContext;

namespace MPP.Back.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository, IServicesScoped
    {
        private readonly InfrastructureDbContext _dbContext;

        public UsuarioRepository(InfrastructureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Usuario?> GetByCredentialsUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return _dbContext.Usuarios
                .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Rol)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }
    }
}
