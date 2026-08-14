using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checador.API.Data.Entities
{
    [Table("Admins")]
    public class Administrador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        [MaxLength(50)]
        public string NumeroEmpleado { get; set; }

        [Required]
        [MaxLength(50)]
        public string Puesto { get; set; }

        [Required]
        public string Huella { get; set; }
    }
}
