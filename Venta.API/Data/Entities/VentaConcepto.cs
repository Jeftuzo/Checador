namespace Venta.API.Data.Entities
{
    public class VentaConcepto
    {
        public int Id { get; set; }

        //Venta
        public int VentaId { get; set; }
        public Venta? Venta { get; set; }

        //Producto
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe => Cantidad * ValorUnitario;
    }
}
