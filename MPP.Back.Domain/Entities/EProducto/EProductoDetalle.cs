using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP.Back.Domain.Entities
{
    public class EProductoDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductoDetalleId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [StringLength(150)]
        public string? Proveedor { get; set; }

        [StringLength(100)]
        public string? Lote { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        [ForeignKey(nameof(ProductoId))]
        public virtual EProducto Producto { get; set; } = null!;
    }
}
