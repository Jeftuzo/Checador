namespace Checador.API.Data.Dtos
{
    public class ReporteAsistenciaDto
    {
        public string NumeroEmpleado { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string Puesto { get; set; } = "";
        public string Departamento { get; set; } = "";
        public string DetalleTiempo { get; set; } = "";
        public string FotoEntradaBase64 { get; set; } = "";
        public string FotoSalidaBase64 { get; set; } = "";
    }
}