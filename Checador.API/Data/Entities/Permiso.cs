using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checador.API.Data.Entities
{
    [Table("Permisos")]
    public class Permiso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("numeroEmpleado")]
        public string NumeroEmpleado { get; set; } = null!;

        [ForeignKey(nameof(NumeroEmpleado))]
        public Empleado Empleado { get; set; } = null!;

        [Required]
        public DateOnly Fecha { get; set; }

        [Required]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        public TimeOnly HoraFin { get; set; }

        [MaxLength(500)]
        public string? Motivo { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "Pendiente";

        // Guardaremos quién autorizó/rechazó el permiso.
        // Después lo conectaremos con el sistema de usuarios/roles.
        [MaxLength(100)]
        public string? AutorizadoPor { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

        public DateTime? FechaAutorizacion { get; set; }
    }
}