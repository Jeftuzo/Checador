using System.ComponentModel.DataAnnotations;

namespace Checador.Web.Client.Dtos.AdminsDtos
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El número de empleado es requerido.")]
        public string NumeroEmpleado { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; } = "";
    }
}