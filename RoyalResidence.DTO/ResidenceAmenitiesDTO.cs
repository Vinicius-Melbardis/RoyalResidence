using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalResidence.DTO
{
    public class ResidenceAmenitiesDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int ResidenceId { get; set; }

        public string? ResidenceName { get; set; }
    }
}
