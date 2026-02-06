using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Contexts.Configurations.Relations
{
    public class RelationalUsuario : IEntityTypeConfiguration<Usuario>
    {
        private const string TableName = "Usuarios";

        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.ToTable(TableName);
            entity.HasKey(u => u.UsuarioId);

            entity.Property(u => u.UserName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.Password)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(u => u.FechaCreacion)
                .HasColumnType("datetime2")
                .IsRequired();

            entity.Property(u => u.FechaActualizacion)
                .HasColumnType("datetime2");

            entity.HasMany(u => u.Roles)
                .WithOne(ur => ur.Usuario)
                .HasForeignKey(ur => ur.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
