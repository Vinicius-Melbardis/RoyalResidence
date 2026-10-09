using Microsoft.EntityFrameworkCore;
using RoyalResidence_API.Models;

namespace RoyalResidence_API.Controllers.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Residence> Residences { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Residence>().HasData(
                new Residence
                {
                    Id = 1,
                    Name = "Royal Residence",
                    Details = "Luxurious residence with stunning ocean views and private beach access.",
                    Rate = 500.0,
                    Sqm = 230,
                    Occupancy = 6,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg",
                    CreatedDate = new DateTime(2026, 7, 1),
                    UpdatedDate = new DateTime(2026, 7, 1)
                },
                new Residence
                {
                    Id = 2,
                    Name = "Diamond Residence",
                    Details = "Elegant residence with marble interiors and panoramic mountain views.",
                    Rate = 750.0,
                    Sqm = 300,
                    Occupancy = 8,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg",
                    CreatedDate = new DateTime(2026, 8, 1),
                    UpdatedDate = new DateTime(2026, 8, 1)
                },
                new Residence
                {
                    Id = 3,
                    Name = "Pool Residence",
                    Details = "Modern residence featuring an infinity pool and outdoor entertainment area.",
                    Rate = 350.0,
                    Sqm = 170,
                    Occupancy = 4,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg",
                    CreatedDate = new DateTime(2026, 8, 16),
                    UpdatedDate = new DateTime(2026, 8, 15)
                },
                new Residence
                {
                    Id = 4,
                    Name = "Luxury Residence",
                    Details = "Premium residence with spa facilities and concierge services.",
                    Rate = 900.0,
                    Sqm = 370,
                    Occupancy = 10,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg",
                    CreatedDate = new DateTime(2026, 9, 1),
                    UpdatedDate = new DateTime(2026, 9, 1)
                },
                new Residence
                {
                    Id = 5,
                    Name = "Garden Residence",
                    Details = "Charming residence surrounded by tropical gardens and nature trails.",
                    Rate = 275.0,
                    Sqm = 140,
                    Occupancy = 3,
                    ImageUrl = "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg",
                    CreatedDate = new DateTime(2026, 10, 3),
                    UpdatedDate = new DateTime(2026, 10, 3)
                }
            );
        }
    }
}
