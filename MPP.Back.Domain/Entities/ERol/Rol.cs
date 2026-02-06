using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP.Back.Domain.Entities
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolId { get; set; }
        [Required, StringLength(150)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public int EstadoId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
        [ForeignKey(nameof(EstadoId))]
        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<UsuarioRol> RolesUsuario { get; set; } = new HashSet<UsuarioRol>();
    }
}
