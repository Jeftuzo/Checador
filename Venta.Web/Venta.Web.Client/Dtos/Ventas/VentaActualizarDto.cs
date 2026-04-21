using Venta.Web.Client.Dtos.VentasConcepto;

namespace Venta.Web.Client.Dtos.Ventas
{
    public class VentaActualizarDto
    {
        public int ClienteId { get; set; }
        public decimal Total { get; set; }
        public List<VentaConceptoActualizarDto> Conceptos { get; set; } = new();
    }
}
