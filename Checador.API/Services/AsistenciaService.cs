using Checador.API.Data.Entities;
using Checador.API.Data;
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
                asistencia.Foto = foto;

                _context.Asistencias.Update(asistencia);
                await _context.SaveChangesAsync();

                return (true, $"Salida de {empleado.Nombre} {empleado.ApellidoPa} registrada correctamente");
            }
        }

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
    }
}
