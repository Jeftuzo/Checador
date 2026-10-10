using Checador.API.Data;
using Checador.API.Data.Dtos.AdminDto;
using Checador.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var hash = SecurityHelper.EncriptarPassword(request.Password);

            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.NumeroEmpleado == request.NumeroEmpleado && a.PasswordHash == hash);

            if (admin != null)
            {
                return Ok(new
                {
                    admin.NumeroEmpleado,
                    NombreCompleto = admin.NombreCompleto,
                    admin.Rol,
                    Departamento = "Administración",
                    EsAdmin = true
                });
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.NumeroEmpleado == request.NumeroEmpleado && e.PasswordHash == hash);

            if (empleado != null && (empleado.Rol == "DirectorArea" || empleado.Rol == "RRHH" || empleado.Rol == "RRHHAdmin"))
            {
                return Ok(new
                {
                    empleado.NumeroEmpleado,
                    NombreCompleto = $"{empleado.Nombre} {empleado.ApellidoPa} {empleado.ApellidoMa}".Trim(),
                    empleado.Rol,
                    empleado.Departamento,
                    EsAdmin = false
                });
            }

            return Unauthorized(new { Mensaje = "Credenciales incorrectas o el usuario no tiene privilegios administrativos." });
        }
    }
}