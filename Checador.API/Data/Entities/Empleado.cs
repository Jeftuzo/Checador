using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Checador.API.Data.Entities
{
    public class Empleado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string NumeroEmpleado { get; set; } = null!;

        [Required]
        public string Nombre { get; set; } = null!;
        [Required]
        public string ApellidoPa { get; set; } = null!;
        public string ApellidoMa { get; set; } = null!;
        [Required]
        public string Puesto { get; set; } = null!;
        [Required]
        public string Departamento { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
    }
}
