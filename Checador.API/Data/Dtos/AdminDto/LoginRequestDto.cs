using System.ComponentModel.DataAnnotations;

namespace Checador.API.Data.Dtos.AdminDto
{
    public class LoginRequestDto
    {
        [Required]
        public string NumeroEmpleado { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}