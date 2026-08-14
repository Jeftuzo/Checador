using Checador.API.Data.Dtos;
using Checador.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checador.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AsistenciaController : ControllerBase
    {
        private readonly AsistenciaService _asistenciaService;

        public AsistenciaController(AsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarAsistencia([FromBody] RegistroDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (exito, mensaje) = await _asistenciaService.RegistrarAsistenciaAsync(request.NumeroEmpleado, request.Foto);

            var respuestaJson = new RespuestaDto { Mensaje = mensaje };

            if (!exito)
            {
                return BadRequest(respuestaJson); 
            }

            return Ok(respuestaJson); 
        }

        [HttpGet("reporte")]
        public async Task<ActionResult<List<ReporteAsistenciaDto>>> GetReporte([FromQuery] string tipo = "dia")
        {
            try
            {
                if (string.IsNullOrEmpty(tipo))
                {
                    tipo = "dia";
                }

                var reporte = await _asistenciaService.ObtenerReporteAsync(tipo.ToLower());
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al generar el reporte: {ex.Message}");
            }
        }
    }
}
