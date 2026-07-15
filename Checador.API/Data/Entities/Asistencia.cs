using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Checador.API.Data.Entities
{
    [Table("Asistencia")]
    public class Asistencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NumeroEmpleado { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string ApellidoPa { get; set; } = null!;
        public string ApellidoMa { get; set; } = null!;
        public DateTime? HoraEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }

        [Required]
        public string Foto { get; set;} = null!;

        [ForeignKey("NumeroEmpleado")]
        public Empleado? Empleado { get; set; }
    }
}
