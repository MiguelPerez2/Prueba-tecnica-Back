using Microsoft.EntityFrameworkCore;
using MPP.Back.Application.Interfaces.Repository;
using MPP.Back.Domain.Entities;
using InfrastructureDbContext = MPP.Back.Infrastructure.Contexts.DbContext;

namespace MPP.Back.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository, IServicesScoped
    {
        private readonly InfrastructureDbContext _dbContext;

        public ProductoRepository(InfrastructureDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<EProducto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await QueryProductoConDetalle()
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .ToListAsync(cancellationToken);
        }

        public Task<EProducto?> GetByIdAsync(int productoId, CancellationToken cancellationToken)
        {
            return QueryProductoConDetalle()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductoId == productoId, cancellationToken);
        }

        public Task<EProducto?> GetByIdForUpdateAsync(int productoId, CancellationToken cancellationToken)
        {
            return QueryProductoConDetalle()
                .FirstOrDefaultAsync(x => x.ProductoId == productoId, cancellationToken);
        }

        public async Task<EProducto> CreateAsync(EProducto producto, CancellationToken cancellationToken)
        {
            await _dbContext.Productos.AddAsync(producto, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return producto;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(EProducto producto, CancellationToken cancellationToken)
        {
            _dbContext.Productos.Remove(producto);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<EProducto> QueryProductoConDetalle()
        {
            return _dbContext.Productos
                .Include(x => x.Detalles);
        }
    }
}
