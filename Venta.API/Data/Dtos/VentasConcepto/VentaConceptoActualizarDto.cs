namespace Venta.API.Data.Dtos.VentasConcepto
{
    public class VentaConceptoActualizarDto
    {
        public int Id { get; set; } //Si es 0, se considera un nuevo concepto, si es diferente de 0, se actualiza el concepto existente
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
