using Venta.API.Data.Dtos.VentasConcepto;

namespace Venta.API.Data.Dtos.Ventas
{
    public class VentaNuevaDto
    {
        public int ClienteId { get; set; }
        public List<VentaConceptoNuevoDto> Conceptos { get; set; } = new();
        public decimal Total { get; set; }
    }
}
