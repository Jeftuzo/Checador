using System.ComponentModel.DataAnnotations;

namespace Checador.API.Data.Dtos.PermisoDto
{
    public class CrearPermisoDto
    {
        [Required]
        public string NumeroEmpleado { get; set; } = null!;

        [Required]
        public DateOnly Fecha { get; set; }

        [Required]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        public TimeOnly HoraFin { get; set; }

        [MaxLength(500)]
        public string? Motivo { get; set; }
    }
}