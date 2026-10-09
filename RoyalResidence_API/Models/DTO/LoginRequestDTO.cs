using System.ComponentModel.DataAnnotations;

namespace RoyalResidence_API.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
