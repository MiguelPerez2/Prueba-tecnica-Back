using MPP.Back.Application.Dtos.Producto;

namespace MPP.Back.Application.Interfaces.Services
{
    public interface IProductoService
    {
        Task<IReadOnlyCollection<ProductoDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ProductoDto> GetByIdAsync(int productoId, CancellationToken cancellationToken);
        Task<ProductoDto> CreateAsync(ProductoDto request, CancellationToken cancellationToken);
        Task<ProductoDto> UpdateAsync(int productoId, ProductoDto request, CancellationToken cancellationToken);
        Task DeleteAsync(int productoId, CancellationToken cancellationToken);
    }
}
