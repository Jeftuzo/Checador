using Checador.API.Data.Dtos.AdminDto;

namespace Checador.API.Services
{
    public interface IAdminService
    {
        Task<bool> RequiereSetupAsync();
        Task<bool> RegistrarPrimerAdminAsync(RegistroAdminDto dto);
    }
}
