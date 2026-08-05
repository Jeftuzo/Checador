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
        [Required]
        [Column("Foto")]
        public string Foto { get; set;} = null!;

        [ForeignKey("NumeroEmpleado")]
        public Empleado? Empleado { get; set; }
    }
}
