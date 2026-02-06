namespace MPP.Back.Application.Dtos.Producto
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public List<ProductoDetalleDto> Detalles { get; set; } = new();
    }
}
