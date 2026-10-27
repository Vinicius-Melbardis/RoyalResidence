using RoyalResidence.DTO;

namespace RoyalResidenceWeb.Services.IServices
{
    public interface IAuthService
    {
        Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO);
        Task<T?> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO);
    }
}
