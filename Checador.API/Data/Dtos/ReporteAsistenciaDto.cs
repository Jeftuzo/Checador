namespace Checador.API.Data.Dtos
{
    public class ReporteAsistenciaDto
    {
        public string NumeroEmpleado { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Puesto { get; set; } = string.Empty;
        public string DetalleTiempo { get; set; } = string.Empty;
        public string FotoBase64 { get; set; } = string.Empty;
    }
}
