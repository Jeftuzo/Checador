using Checador.API.Data;
using Checador.API.Data.Dtos.AdminDto;
using Checador.API.Data.Entities;
using Checador.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("requiere-setup")]
        public async Task<IActionResult> GetSetupStatus()
        {
            try
            {
                bool requiereSetup = await _adminService.RequiereSetupAsync();
                return Ok(requiereSetup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al verificar la base de datos: " + ex.Message);
            }
        }

        [HttpPost("registrar-inicial")]
        public async Task<IActionResult> RegistrarAdminInicial([FromBody] RegistroAdminDto dto)
        {
            try
            {
                bool exito = await _adminService.RegistrarPrimerAdminAsync(dto);

                if (!exito)
                {
                    return BadRequest(new { Mensaje = "Ya existe un administrador registrado. Operación bloqueada." });
                }

                return Ok(new { Mensaje = "Administrador registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al guardar el administrador: " + ex.Message);
            }
        }

    }
}
