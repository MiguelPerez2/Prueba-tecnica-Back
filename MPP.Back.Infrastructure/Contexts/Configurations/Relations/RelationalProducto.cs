using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Contexts.Configurations.Relations
{
    public class RelationalProducto : IEntityTypeConfiguration<EProducto>
    {
        private const string TableName = "Productos";

        public void Configure(EntityTypeBuilder<EProducto> entity)
        {
            entity.ToTable(TableName);
            entity.HasKey(x => x.ProductoId);

            entity.Property(x => x.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Descripcion)
                .HasMaxLength(500);

            entity.Property(x => x.FechaCreacion)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(x => x.FechaActualizacion)
                .HasColumnType("datetime2");

            entity.HasMany(x => x.Detalles)
                .WithOne(x => x.Producto)
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
