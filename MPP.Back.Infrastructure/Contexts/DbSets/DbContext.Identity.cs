using Microsoft.EntityFrameworkCore;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Contexts
{
    public partial class DbContext
    {
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
    }
}
