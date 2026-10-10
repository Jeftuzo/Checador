using Checador.API.Data;
using Checador.API.Data.Dtos;
using Checador.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checador.API.Services
{
    public class AsistenciaService
    {
        private readonly ApplicationDbContext _context;

        public AsistenciaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Exito, string mensaje, bool RequiereConfirmacion)>
           RegistrarAsistenciaAsync(
               string numeroEmpleado,
               string foto,
               bool confirmarSalidaAnticipada)
        {
            var empleado = await _context.Empleados.FindAsync(numeroEmpleado);

            if (empleado == null)
            {
                return (
                    false,
                    "El número de empleado no está registrado en el sistema.",
                    false
                );
            }


            DateTime ahora = DateTime.Now;
            DateTime hoy = DateTime.Today;

            // Buscar una entrada de hoy que todavía no tenga salida
            var asistencia = await _context.Asistencias
                .Where(a =>
                    a.NumeroEmpleado == numeroEmpleado &&
                    a.HoraEntrada >= hoy &&
                    a.HoraEntrada < hoy.AddDays(1) &&
                    a.HoraSalida == null)
                .FirstOrDefaultAsync();


            if (asistencia == null)
            {
                var nuevaAsistencia = new Asistencia
                {
                    NumeroEmpleado = numeroEmpleado,
                    Nombre = empleado.Nombre,
                    ApellidoPa = empleado.ApellidoPa,
                    ApellidoMa = empleado.ApellidoMa,

                    HoraEntrada = ahora,
                    HoraSalida = null,

                    FotoEntrada = foto,
                    FotoSalida = null,

                    SalidaAnticipada = false,
                    PermisoId = null
                };

                _context.Asistencias.Add(nuevaAsistencia);
                await _context.SaveChangesAsync();

                await OptimizarAlmacenamientoAsync();

                return (
                     true,
                     $"Entrada de {empleado.Nombre} {empleado.ApellidoPa} registrada correctamente",
                     false
              );
            }

            var horaActual = TimeOnly.FromDateTime(ahora);
            var horaSalidaNormal = new TimeOnly(15, 0);

            bool salidaAnticipada = false;
            string mensajeConfirmacion = "";

            var permisoAutorizado = await _context.Permisos
                .Where(p =>
                    p.NumeroEmpleado == numeroEmpleado &&
                    p.Fecha == DateOnly.FromDateTime(hoy) &&
                    p.Estado == "Autorizado")
                .OrderBy(p => p.HoraInicio)
                .FirstOrDefaultAsync();

            if (permisoAutorizado != null)
            {
                if (horaActual < permisoAutorizado.HoraInicio)
                {
                    salidaAnticipada = true;

                    mensajeConfirmacion =
      $"Tiene un permiso autorizado de " +
      $"{permisoAutorizado.HoraInicio.ToString("HH:mm")} a " +
      $"{permisoAutorizado.HoraFin.ToString("HH:mm")}, " +
      $"pero está intentando registrar salida antes de la hora autorizada. " +
      $"¿Desea continuar?";
                }
            }
            else
            {
                if (horaActual < horaSalidaNormal)
                {
                    salidaAnticipada = true;

                    mensajeConfirmacion =
                        "Está intentando registrar su salida antes de las 3:00 PM. " +
                        "¿Está seguro de que desea continuar?";
                }
            }

            if (salidaAnticipada && !confirmarSalidaAnticipada)
            {
                return (
                    false,
                    mensajeConfirmacion,
                    true
                );
            }

            asistencia.HoraSalida = ahora;
            asistencia.FotoSalida = foto;
            asistencia.SalidaAnticipada = salidaAnticipada;

            if (permisoAutorizado != null)
            {
                asistencia.PermisoId = permisoAutorizado.Id;
            }

            _context.Asistencias.Update(asistencia);
            await _context.SaveChangesAsync();

            return (
                true,
                $"Salida de {empleado.Nombre} {empleado.ApellidoPa} registrada correctamente",
                false
            );

        }


        // Borra las imágenes después de 30 días de antigüedad
        private async Task OptimizarAlmacenamientoAsync()
        {
            DateTime limite = DateTime.Today.AddDays(-30);

            var registrosViejos = await _context.Asistencias
                .Where(a =>
                    a.HoraEntrada < limite &&
                    (a.FotoEntrada != null || a.FotoSalida != null))
                .ToListAsync();

            if (registrosViejos.Any())
            {
                foreach (var asistenciaVieja in registrosViejos)
                {
                    asistenciaVieja.FotoEntrada = null;
                    asistenciaVieja.FotoSalida = null;
                }

                _context.Asistencias.UpdateRange(registrosViejos);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<ReporteAsistenciaDto>> ObtenerReporteAsync(string tipo)
        {
            var fechaActual = DateTime.Today;
            var query = _context.Asistencias.AsQueryable();

            if (tipo == "dia")
            {
                query = query.Where(a =>
                    a.HoraEntrada >= fechaActual &&
                    a.HoraEntrada < fechaActual.AddDays(1));
            }
            else if (tipo == "semana")
            {
                int diferencia = (int)fechaActual.DayOfWeek - 1;

                if (diferencia < 0)
                    diferencia = 6;

                var inicioSemana = fechaActual.AddDays(-diferencia);
                var finSemana = inicioSemana.AddDays(7);

                query = query.Where(a =>
                    a.HoraEntrada >= inicioSemana &&
                    a.HoraEntrada < finSemana);
            }
            else if (tipo == "mes")
            {
                query = query.Where(a =>
                    a.HoraEntrada.HasValue &&
                    a.HoraEntrada.Value.Year == fechaActual.Year &&
                    a.HoraEntrada.Value.Month == fechaActual.Month);
            }
            else if (tipo == "ultimos30")
            {
                var treintaDiasAtras = fechaActual.AddDays(-30);

                query = query.Where(a =>
                    a.HoraEntrada >= treintaDiasAtras);
            }

            var registros = await query.ToListAsync();

            var empleados = await _context.Empleados.ToListAsync();

            var reporteAgrupado = registros
                .GroupBy(a => a.NumeroEmpleado)
                .Select(g =>
                {
                    var primerRegistro = g.First();

                    var ultimoRegistro = g
                        .OrderByDescending(a => a.HoraEntrada)
                        .First();

                    double totalMinutos = g
                        .Where(a =>
                            a.HoraSalida.HasValue &&
                            a.HoraEntrada.HasValue)
                        .Sum(a =>
                            (a.HoraSalida!.Value -
                             a.HoraEntrada!.Value).TotalMinutes);

                    int horas = (int)(totalMinutos / 60);
                    int minutos = (int)(totalMinutos % 60);

                    var empleado = empleados
                        .FirstOrDefault(e =>
                            e.NumeroEmpleado == g.Key);

                    return new ReporteAsistenciaDto
                    {
                        NumeroEmpleado = g.Key,

                        NombreCompleto =
                            $"{primerRegistro.Nombre} " +
                            $"{primerRegistro.ApellidoPa} " +
                            $"{primerRegistro.ApellidoMa}".Trim(),

                        Puesto = empleado?.Puesto ?? "Sin Puesto",

                        Departamento = empleado?.Departamento ?? "Sin Departamento",

                        DetalleTiempo =
                            $"{horas} hrs {minutos} mins " +
                            $"({g.Count()} registros)",

                        FotoEntradaBase64 =
                            ultimoRegistro.FotoEntrada ?? string.Empty,

                        FotoSalidaBase64 =
                            ultimoRegistro.FotoSalida ?? string.Empty
                    };
                })
                .ToList();

            return reporteAgrupado;
        }


        public async Task<(bool Exito, string Mensaje, List<ReporteAsistenciaDto>Reporte)>ObtenerReportePorRolAsync(string tipo, string numeroSolicitante)
        {


            var empleadoSolicitante = await _context.Empleados
                .FirstOrDefaultAsync(e =>
                    e.NumeroEmpleado == numeroSolicitante);

            if (empleadoSolicitante != null)
            {

                if (empleadoSolicitante.Rol == "DirectorArea")
                {
                    var reporteCompleto =
                        await ObtenerReporteAsync(tipo);

                    var empleadosArea = await _context.Empleados
                        .Where(e =>
                            e.Departamento ==
                            empleadoSolicitante.Departamento)
                        .Select(e => e.NumeroEmpleado)
                        .ToListAsync();

                    var reporteArea = reporteCompleto
                        .Where(r =>
                            empleadosArea.Contains(r.NumeroEmpleado))
                        .ToList();

                    return (
                        true,
                        "",
                        reporteArea
                    );
                }

                if (empleadoSolicitante.Rol == "RRHH")
                {
                    var reporte =
                        await ObtenerReporteAsync(tipo);

                    return (
                        true,
                        "",
                        reporte
                    );
                }


                if (empleadoSolicitante.Rol == "RRHHAdmin")
                {
                    var reporte =
                        await ObtenerReporteAsync(tipo);

                    return (
                        true,
                        "",
                        reporte
                    );
                }

                return (
                    false,
                    "El empleado no tiene permisos para consultar reportes.",
                    new List<ReporteAsistenciaDto>()
                );
            }

            var administrador = await _context.Administradores
                .FirstOrDefaultAsync(a =>
                    a.NumeroEmpleado == numeroSolicitante);

            if (administrador != null &&
                administrador.Rol == "Admin")
            {
                var reporte =
                    await ObtenerReporteAsync(tipo);

                return (
                    true,
                    "",
                    reporte
                );
            }


            return (
                false,
                "El usuario no existe o no tiene permisos para consultar reportes.",
                new List<ReporteAsistenciaDto>()
            );
        }

        public async Task<(string FotoEntrada, string FotoSalida)> ObtenerFotosPorFechaAsync(string numeroEmpleado, DateTime fecha)
        {
            var registro = await _context.Asistencias
                .Where(a => a.NumeroEmpleado == numeroEmpleado &&
                            a.HoraEntrada.HasValue &&
                            a.HoraEntrada.Value.Date == fecha.Date)
                .OrderByDescending(a => a.HoraEntrada)
                .FirstOrDefaultAsync();

            if (registro != null)
            {
                return (registro.FotoEntrada ?? string.Empty, registro.FotoSalida ?? string.Empty);
            }

            return (string.Empty, string.Empty);
        }
    }
}