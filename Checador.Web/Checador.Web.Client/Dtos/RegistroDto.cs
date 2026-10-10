using System.ComponentModel.DataAnnotations;

namespace Checador.Web.Client.Dtos
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El número de empleado es obligatorio.")]
        public string NumeroEmpleado { get; set; } = "";

        public string Foto { get; set; } = "";

        public bool ConfirmarSalidaAnticipada { get; set; } = false;
    }
}