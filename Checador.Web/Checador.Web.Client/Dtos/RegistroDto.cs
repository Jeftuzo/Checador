using System.ComponentModel.DataAnnotations;

namespace Checador.Web.Client.Dtos
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El número de empleado es obligatorio.")]
        public string NumeroEmpleado { get; set; } = "";

        // La fotografía se captura después de enviar el formulario,
        // dentro de ProcesarRegistro().
        public string Foto { get; set; } = "";

        public bool ConfirmarSalidaAnticipada { get; set; } = false;
    }
}