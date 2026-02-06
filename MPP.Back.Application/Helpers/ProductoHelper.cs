using MPP.Back.Application.Dtos.Producto;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Application.Helpers
{
    internal static class ProductoHelper
    {
        internal static ProductoDto MapToDto(EProducto producto)
        {
            return new ProductoDto
            {
                ProductoId = producto.ProductoId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Detalles = producto.Detalles
                    .OrderBy(x => x.ProductoDetalleId)
                    .Select(MapToDto)
                    .ToList()
            };
        }
        internal static ProductoDetalleDto MapToDto(EProductoDetalle detalle)
        {
            return new ProductoDetalleDto
            {
                ProductoDetalleId = detalle.ProductoDetalleId,
                Proveedor = detalle.Proveedor,
                Lote = detalle.Lote,
                Precio = detalle.Precio,
                Stock = detalle.Stock
            };
        }

        internal static EProductoDetalle MapToEntity(ProductoDetalleDto detalle)
        {
            return new EProductoDetalle
            {
                Proveedor = NormalizarTextoOpcional(detalle.Proveedor),
                Lote = NormalizarTextoOpcional(detalle.Lote),
                Precio = detalle.Precio,
                Stock = detalle.Stock
            };
        }

        internal static string? NormalizarTextoOpcional(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        internal static bool NoTenerDetallesDuplicados(IEnumerable<ProductoDetalleDto>? detalles)
        {
            if (detalles is null)
            {
                return true;
            }

            var combinaciones = detalles
                .Select(detalle => new
                {
                    Proveedor = Normalizar(detalle.Proveedor),
                    Lote = Normalizar(detalle.Lote)
                })
                .ToList();

            return combinaciones
                .Distinct()
                .Count() == combinaciones.Count;
        }

        internal static string Normalizar(string? valor)
        {
            return string.IsNullOrWhiteSpace(valor)
                ? string.Empty
                : valor.Trim().ToUpperInvariant();
        }
    }
}
