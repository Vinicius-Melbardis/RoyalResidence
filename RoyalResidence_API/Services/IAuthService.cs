using RoyalResidence_API.Models.DTO;

namespace RoyalResidence_API.Services
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)

        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)

        Task<bool> IsEmailExistsAsync(string email)
    }
}
