using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checador.API.Data.Entities
{
    [Table("Asistencia")]
    public class Asistencia
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("numeroEmpleado")]
        public string NumeroEmpleado { get; set; } = null!;

        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        [Column("apellidoPa")]
        public string ApellidoPa { get; set; } = null!;

        [Column("apellidoMa")]
        public string ApellidoMa { get; set; } = null!;

        [Column("horaEntrada")]
        public DateTime? HoraEntrada { get; set; }

        [Column("horaSalida")]
        public DateTime? HoraSalida { get; set; }

        [Column("fotoEntrada")]
        public string? FotoEntrada { get; set; }

        [Column("fotoSalida")]
        public string? FotoSalida { get; set; }

        [Column("salidaAnticipada")]
        public bool SalidaAnticipada { get; set; } = false;

        [Column("permisoId")]
        public int? PermisoId { get; set; }

        [ForeignKey(nameof(NumeroEmpleado))]
        public Empleado? Empleado { get; set; }

        [ForeignKey(nameof(PermisoId))]
        public Permiso? Permiso { get; set; }
    }
}