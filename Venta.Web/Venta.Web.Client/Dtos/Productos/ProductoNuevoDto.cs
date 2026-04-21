using System.ComponentModel.DataAnnotations;

namespace Venta.Web.Client.Dtos.Productos
{
    public class ProductoNuevoDto
    {
        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo nombre no debe de ser de mas de 50 caracteres.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El campo precio es obligatorio.")]
        [Range(0.00, double.MaxValue, ErrorMessage = "El precio debe ser mayor a \"0.00\"")]
        public decimal Precio { get; set; }
    }
}
