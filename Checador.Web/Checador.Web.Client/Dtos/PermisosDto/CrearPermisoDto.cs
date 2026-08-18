using System.ComponentModel.DataAnnotations;

namespace Checador.Web.Client.Dtos.PermisosDtos
{
    public class CrearPermisoDto
    {
        [Required]
        public string NumeroEmpleado { get; set; } = "";

        [Required]
        public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public TimeOnly HoraInicio { get; set; } = new TimeOnly(8, 0);

        [Required]
        public TimeOnly HoraFin { get; set; } = new TimeOnly(10, 0);

        [MaxLength(500)]
        public string? Motivo { get; set; }
    }
}