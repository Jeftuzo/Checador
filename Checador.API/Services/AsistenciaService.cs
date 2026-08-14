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

        public async Task<(bool Exito, string mensaje)> RegistrarAsistenciaAsync(string numeroEmpleado, string foto)
        {
            var empleado = await _context.Empleados.FindAsync(numeroEmpleado);
            if (empleado == null)
            {
                return (false, "El número de empleado no está registrado en el sistema."); 
            }
            
            DateTime ahora = DateTime.Now;
            DateTime hoy = DateTime.Today;


            var asistencia = await _context.Asistencias
                .Where(a => a.NumeroEmpleado == numeroEmpleado &&
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
                    Foto = foto
                };

                _context.Asistencias.Add(nuevaAsistencia);
                await _context.SaveChangesAsync();

                await OptimizarAlmacenamientoAsync();
                return (true, $"Entrada de {empleado.Nombre} {empleado.ApellidoPa} registrada correctamente");
            }
            else
            {
                asistencia.HoraSalida = ahora;

                _context.Asistencias.Update(asistencia);
                await _context.SaveChangesAsync();

                return (true, $"Salida de {empleado.Nombre} {empleado.ApellidoPa} registrada correctamente");
            }
        }


        //Boorra las imagenes a partir de los 30 dias de antiguedad.
        private async Task OptimizarAlmacenamientoAsync()
        {
            DateTime limite = DateTime.Today.AddDays(-30);

            var registrosViejos = await _context.Asistencias
                .Where(a => a.HoraEntrada < limite && a.Foto != "")
                .ToListAsync();

            if (registrosViejos.Any())
            {
                foreach (var asistenciaVieja in registrosViejos)
                {
                    asistenciaVieja.Foto = "";
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
                query = query.Where(a => a.HoraEntrada >= fechaActual && a.HoraEntrada < fechaActual.AddDays(1));
            }
            else if (tipo == "semana")
            {
                int diferencia = (int)fechaActual.DayOfWeek - 1;
                if (diferencia < 0) diferencia = 6;
                var inicioSemana = fechaActual.AddDays(-diferencia);
                var finSemana = inicioSemana.AddDays(7);

                query = query.Where(a => a.HoraEntrada >= inicioSemana && a.HoraEntrada < finSemana);
            }
            else if (tipo == "mes")
            {
                query = query.Where(a => a.HoraEntrada.Value.Year == fechaActual.Year && a.HoraEntrada.Value.Month == fechaActual.Month);
            }
            else if (tipo == "ultimos30")
            {
                var treintaDiasAtras = fechaActual.AddDays(-30);
                query = query.Where(a => a.HoraEntrada >= treintaDiasAtras);
            }

            var registros = await query.ToListAsync();

            var reporteAgrupado = registros
                .GroupBy(a => a.NumeroEmpleado)
                .Select(g =>
                {
                    var primerRegistro = g.First();
                    var ultimoRegistro = g.OrderByDescending(a => a.HoraEntrada).First();

                    double totalMinutos = g.Where(a => a.HoraSalida.HasValue)
                        .Sum(a => (a.HoraSalida.Value - a.HoraEntrada).Value.TotalMinutes);

                    int horas = (int)(totalMinutos / 60);
                    int minutos = (int)(totalMinutos % 60);

                    return new ReporteAsistenciaDto
                    {
                        NumeroEmpleado = g.Key,
                        NombreCompleto = $"{primerRegistro.Nombre} {primerRegistro.ApellidoPa} {primerRegistro.ApellidoMa}".Trim(),
                        Puesto = _context.Empleados.Find(g.Key)?.Puesto ?? "Sin Puesto",
                        DetalleTiempo = $"{horas} hrs {minutos} mins ({g.Count()} registros)",
                        FotoBase64 = ultimoRegistro.Foto ?? string.Empty
                    };
                })
                .ToList();

            return reporteAgrupado;
        }
    }
}
