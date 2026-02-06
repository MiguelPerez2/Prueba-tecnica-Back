using FluentValidation;
using MPP.Back.Application.Dtos.Producto;
using MPP.Back.Application.Helpers;

namespace MPP.Back.Application.Validators.Producto
{
    public class ProductoValidator : AbstractValidator<ProductoDto>
    {
        public ProductoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre del producto no puede exceder 150 caracteres.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(500).WithMessage("La descripcion del producto no puede exceder 500 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Descripcion));

            RuleFor(x => x.Detalles)
                .NotEmpty().WithMessage("Debe registrar al menos un detalle de inventario.")
                .Must(ProductoHelper.NoTenerDetallesDuplicados)
                .WithMessage("No se permiten detalles repetidos por proveedor y lote.");

            RuleForEach(x => x.Detalles).SetValidator(new ProductoDetalleValidator());
        }

        private sealed class ProductoDetalleValidator : AbstractValidator<ProductoDetalleDto>
        {
            public ProductoDetalleValidator()
            {
                RuleFor(x => x.Proveedor)
                    .MaximumLength(150).WithMessage("El proveedor no puede exceder 150 caracteres.")
                    .When(x => !string.IsNullOrWhiteSpace(x.Proveedor));

                RuleFor(x => x.Lote)
                    .MaximumLength(100).WithMessage("El lote no puede exceder 100 caracteres.")
                    .When(x => !string.IsNullOrWhiteSpace(x.Lote));

                RuleFor(x => x)
                    .Must(x => !string.IsNullOrWhiteSpace(x.Proveedor) || !string.IsNullOrWhiteSpace(x.Lote))
                    .WithMessage("Cada detalle debe incluir proveedor o lote.");

                RuleFor(x => x.Precio)
                    .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");

                RuleFor(x => x.Stock)
                    .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
            }
        }
    }
}
