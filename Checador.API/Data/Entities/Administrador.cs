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

        [Column("PasswordHash")]
        public string? PasswordHash { get; set; }


        //no se esta utilizando de momento, se mantiene para un futuro
        public string? Huella { get; set; }
    }
}