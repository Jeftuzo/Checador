namespace Checador.Web.Client.Dtos
{
    public class ReporteAsistenciaDto
    {
        public string NumeroEmpleado { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string Puesto { get; set; } = "";
        public string DetalleTiempo { get; set; } = "";
        public string FotoBase64 { get; set; } = "";
    }
}
