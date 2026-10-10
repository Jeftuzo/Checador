using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Checador.API.Data.Entities
{
    public class Empleado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("numeroEmpleado")]
        public string NumeroEmpleado { get; set; } = null!;

        [Required]
        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column("apellidoPa")]
        public string ApellidoPa { get; set; } = null!;

        [Column("apellidoMa")]
        public string ApellidoMa { get; set; } = null!;

        [Required]
        [Column("Puesto")]
        public string Puesto { get; set; } = null!;

        [Required]
        [Column("Departamento")]
        public string Departamento { get; set; } = null!;

        [Required]
        [Column("Rol")]
        public string Rol { get; set; } = "Empleado";

        [Column("PasswordHash")]
        public string? PasswordHash { get; set; }

        [JsonIgnore]
        public ICollection<Asistencia> Asistencias { get; set; }
            = new List<Asistencia>();

        [JsonIgnore]
        public ICollection<Permiso> Permisos { get; set; }
            = new List<Permiso>();
    }
}