using System.ComponentModel.DataAnnotations;

namespace Checador.API.Data.Dtos.AdminDto
{
    public class RegistroAdminDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreCompleto { get; set; } = "";

        [Required(ErrorMessage = "El número de empleado es obligatorio")]
        public string NumeroEmpleado { get; set; } = "";

        [Required(ErrorMessage = "El puesto es obligatorio")]
        public string Puesto { get; set; } = "";

        // Mejora futura: autenticación mediante lector biométrico.
        public string? Huella { get; set; }
    }
}