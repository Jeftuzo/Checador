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
        public string NombreCompleto { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string NumeroEmpleado { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Puesto { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Rol { get; set; } = "Admin";

        
        public string? Huella { get; set; }
        // El campo se conserva para una futura integración con un lector biométrico.
        // Actualmente NO es obligatorio ni se utiliza
        // para autenticar al administrador.
    }
}