using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Contexts.Configurations.Relations
{
    public class RelationalProductoDetalle : IEntityTypeConfiguration<EProductoDetalle>
    {
        private const string TableName = "ProductosDetalles";

        public void Configure(EntityTypeBuilder<EProductoDetalle> entity)
        {
            entity.ToTable(TableName);
            entity.HasKey(x => x.ProductoDetalleId);

            entity.Property(x => x.Proveedor)
                .HasMaxLength(150);

            entity.Property(x => x.Lote)
                .HasMaxLength(100);

            entity.Property(x => x.Precio)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.Stock)
                .IsRequired();

            entity.Property(x => x.FechaCreacion)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(x => x.FechaActualizacion)
                .HasColumnType("datetime2");

            entity.HasOne(x => x.Producto)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
