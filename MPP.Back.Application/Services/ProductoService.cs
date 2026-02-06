using System.Net;
using FluentValidation;
using MPP.Back.Application.Common.Exceptions;
using MPP.Back.Application.Dtos.Producto;
using MPP.Back.Application.Helpers;
using MPP.Back.Application.Interfaces.Repository;
using MPP.Back.Application.Interfaces.Services;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Application.Services
{
    public class ProductoService : IProductoService, IServicesScoped
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IValidator<ProductoDto> _productoValidator;

        public ProductoService(
            IProductoRepository productoRepository,
            IValidator<ProductoDto> productoValidator)
        {
            _productoRepository = productoRepository;
            _productoValidator = productoValidator;
        }

        public async Task<IReadOnlyCollection<ProductoDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var productos = await _productoRepository.GetAllAsync(cancellationToken);
            return productos.Select(ProductoHelper.MapToDto).ToList();
        }

        public async Task<ProductoDto> GetByIdAsync(int productoId, CancellationToken cancellationToken)
        {
            var producto = await _productoRepository.GetByIdAsync(productoId, cancellationToken);
            if (producto is null)
            {
                throw new ExceptionsError((int)HttpStatusCode.NoContent, "Producto no encontrado.");
            }

            return ProductoHelper.MapToDto(producto);
        }

        public async Task<ProductoDto> CreateAsync(ProductoDto request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var producto = new EProducto
            {
                Nombre = request.Nombre.Trim(),
                Descripcion = ProductoHelper.NormalizarTextoOpcional(request.Descripcion),
                Detalles = request.Detalles.Select(ProductoHelper.MapToEntity).ToList()
            };

            var created = await _productoRepository.CreateAsync(producto, cancellationToken);
            return ProductoHelper.MapToDto(created);
        }

        public async Task<ProductoDto> UpdateAsync(int productoId, ProductoDto request, CancellationToken cancellationToken)
        {
            await ValidateRequestAsync(request, cancellationToken);

            var producto = await _productoRepository.GetByIdForUpdateAsync(productoId, cancellationToken);
            if (producto is null)
            {
                throw new ExceptionsError((int)HttpStatusCode.NoContent, "Producto no encontrado.");
            }

            producto.Nombre = request.Nombre.Trim();
            producto.Descripcion = ProductoHelper.NormalizarTextoOpcional(request.Descripcion);
            producto.FechaActualizacion = DateTime.Now;

            var detallesExistentesPorId = producto.Detalles.ToDictionary(x => x.ProductoDetalleId);
            var idsSolicitados = request.Detalles
                .Where(x => x.ProductoDetalleId > 0)
                .Select(x => x.ProductoDetalleId)
                .ToHashSet();

            foreach (var detalleEliminar in producto.Detalles
                         .Where(x => x.ProductoDetalleId > 0 && !idsSolicitados.Contains(x.ProductoDetalleId))
                         .ToList())
            {
                producto.Detalles.Remove(detalleEliminar);
            }

            foreach (var detalleDto in request.Detalles)
            {
                if (detalleDto.ProductoDetalleId == 0)
                {
                    producto.Detalles.Add(ProductoHelper.MapToEntity(detalleDto));
                    continue;
                }

                if (!detallesExistentesPorId.TryGetValue(detalleDto.ProductoDetalleId, out var detalleActual))
                {
                    throw new ExceptionsError((int)HttpStatusCode.BadRequest, "El detalle de producto enviado no existe.");
                }

                detalleActual.Proveedor = ProductoHelper.NormalizarTextoOpcional(detalleDto.Proveedor);
                detalleActual.Lote = ProductoHelper.NormalizarTextoOpcional(detalleDto.Lote);
                detalleActual.Precio = detalleDto.Precio;
                detalleActual.Stock = detalleDto.Stock;
                detalleActual.FechaActualizacion = DateTime.Now;
            }

            await _productoRepository.SaveChangesAsync(cancellationToken);

            var actualizado = await _productoRepository.GetByIdAsync(productoId, cancellationToken);
            if (actualizado is null)
            {
                throw new ExceptionsError((int)HttpStatusCode.NotFound, "Producto no encontrado.");
            }

            return ProductoHelper.MapToDto(actualizado);
        }

        public async Task DeleteAsync(int productoId, CancellationToken cancellationToken)
        {
            var producto = await _productoRepository.GetByIdForUpdateAsync(productoId, cancellationToken);
            if (producto is null)
            {
                throw new ExceptionsError((int)HttpStatusCode.NotFound, "Producto no encontrado.");
            }

            await _productoRepository.DeleteAsync(producto, cancellationToken);
        }

        private async Task ValidateRequestAsync(ProductoDto request, CancellationToken cancellationToken)
        {
            var validationResult = await _productoValidator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return;
            }

            var message = string.Join(" | ", validationResult.Errors
                .Select(x => x.ErrorMessage)
                .Distinct());

            throw new ExceptionsError((int)HttpStatusCode.BadRequest, message);
        }

        
    }
}
