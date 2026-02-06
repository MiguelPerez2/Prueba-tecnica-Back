using Microsoft.EntityFrameworkCore;
using MPP.Back.Domain.Entities;

namespace MPP.Back.Infrastructure.Contexts
{
    public partial class DbContext
    {
        public DbSet<EProducto> Productos { get; set; }
        public DbSet<EProductoDetalle> ProductosDetalles { get; set; }
    }
}
