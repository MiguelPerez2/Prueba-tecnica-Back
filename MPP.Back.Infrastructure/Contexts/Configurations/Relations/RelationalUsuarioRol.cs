using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Contexts.Configurations.Relations
{
    public class RelationalUsuarioRol : IEntityTypeConfiguration<UsuarioRol>
    {
        private const string TableName = "UsuariosRoles";

        public void Configure(EntityTypeBuilder<UsuarioRol> entity)
        {
            entity.ToTable(TableName);
            entity.HasKey(ur => new { ur.UsuarioId, ur.RolId });

            entity.Property(ur => ur.FechaCreacion)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(ur => ur.FechaActualizacion)
                .HasColumnType("datetime2");

            entity.HasOne(ur => ur.Usuario)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ur => ur.Rol)
                .WithMany(r => r.RolesUsuario)
                .HasForeignKey(ur => ur.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ur => ur.Estado)
                .WithMany(e => e.UsuariosRoles)
                .HasForeignKey(ur => ur.EstadoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
