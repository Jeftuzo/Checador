using System.ComponentModel.DataAnnotations;

namespace Checador.API.Data.Dtos
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El número de empleado es requerido")]
        public string NumeroEmpleado { get; set; } = null!;
        [Required]
        public string Foto { get; set; } = "";

        public bool ConfirmarSalidaAnticipada { get; set; } = false;
    }
}
