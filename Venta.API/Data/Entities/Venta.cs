namespace Venta.API.Data.Entities
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        //Cliente
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        //VentaConcepto
        public List<VentaConcepto> Conceptos { get; set; } = new();

        public decimal Total { get; set; }
    }
}
