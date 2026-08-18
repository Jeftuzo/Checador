namespace Checador.Web.Client.Dtos.PermisosDtos
{
    public class PerfilPermisoDto
    {
        public string NumeroEmpleado { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string Rol { get; set; } = "";
        public string Departamento { get; set; } = "";
        public bool EsAdmin { get; set; }
    }
}