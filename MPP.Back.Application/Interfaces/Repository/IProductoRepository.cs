using MPP.Back.Domain.Entities;

namespace MPP.Back.Application.Interfaces.Repository
{
    public interface IProductoRepository
    {
        Task<IReadOnlyCollection<EProducto>> GetAllAsync(CancellationToken cancellationToken);
        Task<EProducto?> GetByIdAsync(int productoId, CancellationToken cancellationToken);
        Task<EProducto?> GetByIdForUpdateAsync(int productoId, CancellationToken cancellationToken);
        Task<EProducto> CreateAsync(EProducto producto, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task DeleteAsync(EProducto producto, CancellationToken cancellationToken);
    }
}
