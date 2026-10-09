using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoyalResidence_API.Controllers.Data;
using RoyalResidence_API.Models.DTO;

namespace RoyalResidence_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDbContext db,IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
