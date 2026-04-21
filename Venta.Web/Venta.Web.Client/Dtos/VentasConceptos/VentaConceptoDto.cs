namespace Venta.Web.Client.Dtos.VentasConcepto
{
    public class VentaConceptoDto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe => Cantidad * ValorUnitario;
    }
}
