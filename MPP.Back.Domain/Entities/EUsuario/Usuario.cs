using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP.Back.Domain.Entities
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }
        [Required]
        public int EstadoId { get; set; }
        [Required, StringLength(100)]
        public string UserName { get; set; }
        [Required, StringLength(20)]
        public string Password { get; set; } 
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
        [ForeignKey(nameof(EstadoId))]
        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<UsuarioRol> Roles { get; set; } = new HashSet<UsuarioRol>();
    }
}
