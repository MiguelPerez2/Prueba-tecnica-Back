using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Contexts.Configurations.Relations
{
    public class RelationalRol : IEntityTypeConfiguration<Rol>
    {
        private const string TableName = "Roles";

        public void Configure(EntityTypeBuilder<Rol> entity)
        {
            entity.ToTable(TableName);
            entity.HasKey(r => r.RolId);

            entity.Property(r => r.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(r => r.FechaCreacion)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(r => r.FechaActualizacion)
                .HasColumnType("datetime2");

            entity.HasOne(r => r.Estado)
                .WithMany(e => e.Roles)
                .HasForeignKey(r => r.EstadoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(r => r.RolesUsuario)
                .WithOne(ur => ur.Rol)
                .HasForeignKey(ur => ur.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
