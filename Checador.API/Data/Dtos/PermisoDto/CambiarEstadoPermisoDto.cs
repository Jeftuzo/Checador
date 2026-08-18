using System.ComponentModel.DataAnnotations;

namespace Checador.API.Data.Dtos.PermisoDto
{
    public class CambiarEstadoPermisoDto
    {
        [Required]
        public string NumeroEmpleadoAutoriza { get; set; } = null!;
    }
}
