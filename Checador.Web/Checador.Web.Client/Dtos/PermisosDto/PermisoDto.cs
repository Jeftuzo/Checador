namespace Checador.Web.Client.Dtos.PermisosDtos
{
    public class PermisoDto
    {
        public int Id { get; set; }

        public string NumeroEmpleado { get; set; } = "";

        public DateOnly Fecha { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFin { get; set; }

        public string? Motivo { get; set; }

        public string Estado { get; set; } = "";

        public string? AutorizadoPor { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public DateTime? FechaAutorizacion { get; set; }

        public EmpleadoPermisoDto? Empleado { get; set; }
    }

    public class EmpleadoPermisoDto
    {
        public string NumeroEmpleado { get; set; } = "";

        public string Nombre { get; set; } = "";

        public string ApellidoPa { get; set; } = "";

        public string ApellidoMa { get; set; } = "";

        public string Departamento { get; set; } = "";

        public string NombreCompleto =>
            $"{Nombre} {ApellidoPa} {ApellidoMa}".Trim();
    }
}