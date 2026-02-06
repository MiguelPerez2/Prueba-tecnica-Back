using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP.Back.Domain.Entities
{
    public class Estado 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EstadoId { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
        public virtual ICollection<Rol> Roles { get; set; } = new HashSet<Rol>();
        public virtual ICollection<UsuarioRol> UsuariosRoles { get; set; } = new HashSet<UsuarioRol>();
    }
}
