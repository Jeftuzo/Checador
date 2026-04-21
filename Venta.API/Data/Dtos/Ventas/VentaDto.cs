using Venta.API.Data.Dtos.VentasConcepto;

namespace Venta.API.Data.Dtos.Ventas
{
    public class VentaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<VentaConceptoDto> Conceptos { get; set; } = new();
    }
}
