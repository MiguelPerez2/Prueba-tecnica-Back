namespace MPP.Back.Application.Dtos.Producto
{
    public class ProductoDetalleDto
    {
        public int ProductoDetalleId { get; set; }
        public string? Proveedor { get; set; }
        public string? Lote { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
