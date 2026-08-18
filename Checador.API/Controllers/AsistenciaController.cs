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
        public async Task<IActionResult> RegistrarAsistencia(
            [FromBody] RegistroDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _asistenciaService
                .RegistrarAsistenciaAsync(
                    request.NumeroEmpleado,
                    request.Foto,
                    request.ConfirmarSalidaAnticipada
                );

            if (resultado.RequiereConfirmacion)
            {
                return Ok(new
                {
                    Exito = false,
                    RequiereConfirmacion = true,
                    Mensaje = resultado.mensaje
                });
            }

            if (!resultado.Exito)
            {
                return BadRequest(new
                {
                    Exito = false,
                    RequiereConfirmacion = false,
                    Mensaje = resultado.mensaje
                });
            }

            return Ok(new
            {
                Exito = true,
                RequiereConfirmacion = false,
                Mensaje = resultado.mensaje
            });
        }

        [HttpGet("reporte")]
        public async Task<IActionResult> GetReporte(
            [FromQuery] string tipo = "dia",
            [FromQuery] string numeroSolicitante = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numeroSolicitante))
                {
                    return BadRequest(new
                    {
                        Mensaje = "Debe indicar el número del usuario solicitante."
                    });
                }

                if (string.IsNullOrWhiteSpace(tipo))
                {
                    tipo = "dia";
                }

                var resultado = await _asistenciaService
                    .ObtenerReportePorRolAsync(
                        tipo.ToLower(),
                        numeroSolicitante);

                if (!resultado.Exito)
                {
                    return StatusCode(403, new
                    {
                        Mensaje = resultado.Mensaje
                    });
                }

                return Ok(resultado.Reporte);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensaje = $"Error interno al generar el reporte: {ex.Message}"
                });
            }
        }
    }
}