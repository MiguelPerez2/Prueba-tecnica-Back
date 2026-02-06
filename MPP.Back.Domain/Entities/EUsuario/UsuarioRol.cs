using System.ComponentModel.DataAnnotations.Schema;

namespace MPP.Back.Domain.Entities
{
    public class UsuarioRol 
    {
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
        public int EstadoId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario Usuario { get; set; } = null!;
        [ForeignKey(nameof(RolId))]
        public virtual Rol Rol { get; set; } = null!;
        [ForeignKey(nameof(EstadoId))]
        public virtual Estado Estado { get; set; } = null!;

    }
}
