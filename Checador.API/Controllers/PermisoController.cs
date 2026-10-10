using Checador.API.Data;
using Checador.API.Data.Dtos.PermisoDto;
using Checador.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Checador.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PermisosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPermiso(
            [FromBody] CrearPermisoDto dto)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e =>
                    e.NumeroEmpleado == dto.NumeroEmpleado);

            if (empleado == null)
            {
                return NotFound(new
                {
                    Mensaje = "El empleado no existe."
                });
            }

            var horaMinima = new TimeOnly(8, 0);
            var horaMaxima = new TimeOnly(15, 0);

            if (dto.HoraInicio < horaMinima ||
                dto.HoraFin > horaMaxima)
            {
                return BadRequest(new
                {
                    Mensaje =
                        "El permiso debe estar dentro del horario de " +
                        "8:00 AM a 3:00 PM."
                });
            }

            if (dto.HoraFin <= dto.HoraInicio)
            {
                return BadRequest(new
                {
                    Mensaje =
                        "La hora final debe ser mayor que la hora inicial."
                });
            }

            var duracion =
                dto.HoraFin.ToTimeSpan() -
                dto.HoraInicio.ToTimeSpan();

            if (duracion > TimeSpan.FromHours(2))
            {
                return BadRequest(new
                {
                    Mensaje =
                        "El permiso no puede ser mayor a 2 horas."
                });
            }

            var inicioMes =
                new DateOnly(dto.Fecha.Year, dto.Fecha.Month, 1);

            var inicioMesSiguiente =
                inicioMes.AddMonths(1);

            var cantidadPermisos = await _context.Permisos
                .CountAsync(p =>
                    p.NumeroEmpleado == dto.NumeroEmpleado &&
                    p.Fecha >= inicioMes &&
                    p.Fecha < inicioMesSiguiente &&
                    p.Estado != "Rechazado" &&
                    p.Estado != "Cancelado");

            if (cantidadPermisos >= 3)
            {
                return BadRequest(new
                {
                    Mensaje =
                        "El empleado ya alcanzó el límite de " +
                        "3 permisos durante este mes."
                });
            }

            var permiso = new Permiso
            {
                NumeroEmpleado = dto.NumeroEmpleado,
                Fecha = dto.Fecha,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                Motivo = dto.Motivo,
                Estado = "Pendiente",
                FechaSolicitud = DateTime.UtcNow,
                FechaAutorizacion = null,
                AutorizadoPor = null
            };

            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Permiso solicitado correctamente.",
                Permiso = permiso
            });
        }

        [HttpGet("mis-permisos/{numeroEmpleado}")]
        public async Task<IActionResult> ObtenerMisPermisos(
            string numeroEmpleado)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e =>
                    e.NumeroEmpleado == numeroEmpleado);

            if (empleado == null)
            {
                return NotFound(new
                {
                    Mensaje = "El empleado no existe."
                });
            }

            var permisos = await _context.Permisos
                .Where(p =>
                    p.NumeroEmpleado == numeroEmpleado)
                .OrderByDescending(p => p.Fecha)
                .ThenByDescending(p => p.HoraInicio)
                .ToListAsync();

            return Ok(permisos);
        }

        [HttpGet("area/{numeroDirector}")]
        public async Task<IActionResult> ObtenerPermisosArea(
            string numeroDirector)
        {
            var director = await _context.Empleados
                .FirstOrDefaultAsync(e =>
                    e.NumeroEmpleado == numeroDirector);

            if (director == null)
            {
                return NotFound(new
                {
                    Mensaje = "El empleado no existe."
                });
            }

            if (director.Rol != "DirectorArea")
            {
                return StatusCode(403, new
                {
                    Mensaje =
                        "El empleado no tiene permisos de Director de Área."
                });
            }

            var permisos = await _context.Permisos
                .Include(p => p.Empleado)
                .Where(p =>
                    p.Empleado.Departamento ==
                    director.Departamento)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(permisos);
        }

        [HttpGet("todos/{numeroSolicitante}")]
        public async Task<IActionResult> ObtenerTodos(
            string numeroSolicitante)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e =>
                    e.NumeroEmpleado == numeroSolicitante);

            if (empleado != null)
            {
                if (empleado.Rol != "RRHH" &&
                    empleado.Rol != "RRHHAdmin")
                {
                    return StatusCode(403, new
                    {
                        Mensaje =
                            "No tiene permisos para consultar " +
                            "todos los permisos."
                    });
                }

                var permisosRRHH = await _context.Permisos
                    .Include(p => p.Empleado)
                    .OrderByDescending(p => p.Fecha)
                    .ToListAsync();

                return Ok(permisosRRHH);
            }

            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a =>
                    a.NumeroEmpleado == numeroSolicitante);

            if (admin == null || admin.Rol != "Admin")
            {
                return StatusCode(403, new
                {
                    Mensaje =
                        "No tiene permisos para realizar esta operación."
                });
            }

            var permisosAdmin = await _context.Permisos
                .Include(p => p.Empleado)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(permisosAdmin);
        }


        [HttpPut("{id}/autorizar")]
        public async Task<IActionResult> AutorizarPermiso(
                    int id,
                    [FromBody] CambiarEstadoPermisoDto dto)
        {
            var tienePermiso =
                await PuedeAutorizar(dto.NumeroEmpleadoAutoriza, id);

            if (!tienePermiso)
            {
                return StatusCode(403, new
                {
                    Mensaje =
                        "No tiene permisos para autorizar solicitudes de esta área."
                });
            }

            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permiso == null)
            {
                return NotFound(new { Mensaje = "El permiso no existe." });
            }

            if (permiso.Estado != "Pendiente")
            {
                return BadRequest(new { Mensaje = $"El permiso ya se encuentra en estado {permiso.Estado}." });
            }

            permiso.Estado = "Autorizado";
            permiso.AutorizadoPor = dto.NumeroEmpleadoAutoriza;
            permiso.FechaAutorizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "Permiso autorizado correctamente." });
        }


        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> RechazarPermiso(
                    int id,
                    [FromBody] CambiarEstadoPermisoDto dto)
        {
            var tienePermiso =
                await PuedeAutorizar(dto.NumeroEmpleadoAutoriza, id);

            if (!tienePermiso)
            {
                return StatusCode(403, new
                {
                    Mensaje =
                        "No tiene permisos para rechazar solicitudes de esta área."
                });
            }

            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permiso == null)
            {
                return NotFound(new { Mensaje = "El permiso no existe." });
            }

            if (permiso.Estado != "Pendiente")
            {
                return BadRequest(new { Mensaje = $"El permiso ya se encuentra en estado {permiso.Estado}." });
            }

            permiso.Estado = "Rechazado";
            permiso.AutorizadoPor = dto.NumeroEmpleadoAutoriza;
            permiso.FechaAutorizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "Permiso rechazado correctamente." });
        }

        [HttpPut("{id}/cancelar/{numeroEmpleado}")]
        public async Task<IActionResult> CancelarPermiso(
            int id,
            string numeroEmpleado)
        {
            var permiso = await _context.Permisos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permiso == null)
            {
                return NotFound(new
                {
                    Mensaje = "El permiso no existe."
                });
            }

            if (permiso.NumeroEmpleado != numeroEmpleado)
            {
                return StatusCode(403, new
                {
                    Mensaje =
                        "No puede cancelar un permiso de otro empleado."
                });
            }

            if (permiso.Estado != "Pendiente")
            {
                return BadRequest(new
                {
                    Mensaje =
                        "Solamente se pueden cancelar permisos pendientes."
                });
            }

            permiso.Estado = "Cancelado";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Permiso cancelado correctamente."
            });
        }

        private async Task<bool> PuedeAutorizar(string numeroEmpleadoAutoriza, int permisoId)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.NumeroEmpleado == numeroEmpleadoAutoriza);

            if (empleado != null)
            {
                if (empleado.Rol == "RRHHAdmin")
                {
                    return true;
                }

                if (empleado.Rol == "DirectorArea")
                {
                    var permiso = await _context.Permisos
                        .Include(p => p.Empleado)
                        .FirstOrDefaultAsync(p => p.Id == permisoId);

                    if (permiso != null && permiso.Empleado.Departamento == empleado.Departamento)
                    {
                        return true;
                    }
                }
            }

            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.NumeroEmpleado == numeroEmpleadoAutoriza);

            if (admin != null && admin.Rol == "Admin")
            {
                return true;
            }

            return false;
        }

        [HttpGet("perfil/{numeroEmpleado}")]
        public async Task<IActionResult> ObtenerPerfil(string numeroEmpleado)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.NumeroEmpleado == numeroEmpleado);

            if (empleado != null)
            {
                return Ok(new
                {
                    empleado.NumeroEmpleado,
                    NombreCompleto =
                        $"{empleado.Nombre} {empleado.ApellidoPa} {empleado.ApellidoMa}".Trim(),
                    empleado.Rol,
                    empleado.Departamento,
                    EsAdmin = false
                });
            }

            var admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.NumeroEmpleado == numeroEmpleado);

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

            return NotFound(new
            {
                Mensaje = "El número de empleado no existe."
            });
        }
    }
}