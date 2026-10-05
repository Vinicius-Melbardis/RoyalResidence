using System.ComponentModel.DataAnnotations;

namespace RoyalResidence_API.Models.DTO
{
    public class ResidenceUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public int Sqm { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
    }
}
