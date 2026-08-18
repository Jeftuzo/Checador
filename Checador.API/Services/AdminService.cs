using Checador.API.Data;
using Checador.API.Data.Dtos.AdminDto;
using Checador.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Checador.API.Services
{
    public class AdminService : IAdminService
    {

        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RequiereSetupAsync()
        {
            return !await _context.Administradores.AnyAsync();
        }

        public async Task<bool> RegistrarPrimerAdminAsync(RegistroAdminDto dto)
        {
            if (await _context.Administradores.AnyAsync())
            {
                return false;
            }

            var nuevoAdmin = new Administrador
            {
                NombreCompleto = dto.NombreCompleto,
                Puesto = dto.Puesto,
                NumeroEmpleado = dto.NumeroEmpleado,
                Rol = "Admin",

                // Mejora futura: lector de huella biométrica.
                // Por ahora el sistema no depende de este campo.
                Huella = dto.Huella
            };

            _context.Administradores.Add(nuevoAdmin);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

