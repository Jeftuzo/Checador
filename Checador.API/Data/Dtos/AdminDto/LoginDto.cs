using System.ComponentModel.DataAnnotations;

namespace Checador.API.Data.Dtos.AdminDto
{
    public class LoginDto
    {
        [Required]
        public string HuellaCapturada { get; set; } = "";
    }
}
