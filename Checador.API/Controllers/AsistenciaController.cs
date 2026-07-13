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
    }
}
