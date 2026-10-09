using System.ComponentModel.DataAnnotations;

namespace RoyalResidence_API.Models.DTO
{
    public class UserDTO
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Role { get; set; } = default!;

    }
}
