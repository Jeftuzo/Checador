using System.ComponentModel.DataAnnotations;

namespace Checador.Web.Client.Dtos.AdminsDtos
{
    public class LoginDto
    {
        [Required]
        public string HuellaCapturada { get; set; } = "";
    }
}
