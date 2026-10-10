using Checador.Web.Client.Dtos.PermisosDtos;

namespace Checador.Web.Client.Services
{
    public class AuthState
    {
        public PerfilPermisoDto? PerfilActual { get; private set; }

        public bool EstaAutenticado => PerfilActual != null;

        public event Action? OnChange;

        public void IniciarSesion(PerfilPermisoDto perfil)
        {
            PerfilActual = perfil;
            NotificarEstadoCambiado();
        }

        public void CerrarSesion()
        {
            PerfilActual = null;
            NotificarEstadoCambiado();
        }

        private void NotificarEstadoCambiado() => OnChange?.Invoke();
    }
}