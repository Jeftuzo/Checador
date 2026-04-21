using Venta.Web.Client.Dtos.VentasConcepto;

namespace Venta.Web.Client.Dtos.Ventas
{
    public class VentaNuevaDto
    {
        public int ClienteId { get; set; }
        public List<VentaConceptoNuevoDto> Conceptos { get; set; } = new();
        public decimal Total { get; set; }
    }
}
