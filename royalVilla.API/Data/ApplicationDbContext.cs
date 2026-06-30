using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        // default configuration for db context 

        public DbSet<Villa> Vilas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VillaAmenities> VillaAmenities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data to the db
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "royal villa",
                    Details = "luxurious villa with stunning ocean views and private beach access",
                    Rate = 500.0,
                    Sqft = 2500,
                    Occupancy = 6,
                    ImageUrl = "C:\\Users\\lenovo\\Desktop\\images.jpg",
                    CreateData = new DateTime(2024, 1, 1),
                    UpdateData = new DateTime(2024, 1, 1)
                },
                new Villa
                {
                    Id = 2,
                    Name = "royal villa",
                    Details = "luxurious villa with stunning ocean views and private beach access",
                    Rate = 300.0,
                    Sqft = 20,
                    Occupancy = 4,
                    ImageUrl = "C:\\Users\\lenovo\\Desktop\\images.jpg",
                    CreateData = new DateTime(2024, 2, 1),
                    UpdateData = new DateTime(2024, 2, 1)
                },
                new Villa
                {
                    Id = 3,
                    Name = "royal villa",
                    Details = "luxurious villa with stunning ocean views and private beach access",
                    Rate = 800.0,
                    Sqft = 3500,
                    Occupancy = 2,
                    ImageUrl = "C:\\Users\\lenovo\\Desktop\\images.jpg",
                    CreateData = new DateTime(2024, 3, 1),
                    UpdateData = new DateTime(2024, 3, 1)
                },
                new Villa
                {
                    Id = 4,
                    Name = "royal villa",
                    Details = "luxurious villa with stunning ocean views and private beach access",
                    Rate = 5500.0,
                    Sqft = 2700,
                    Occupancy = 7,
                    ImageUrl = "C:\\Users\\lenovo\\Desktop\\images.jpg",
                    CreateData = new DateTime(2024, 4, 1),
                    UpdateData = new DateTime(2024, 4, 1)
                },
                new Villa
                {
                    Id = 5,
                    Name = "royal villa",
                    Details = "luxurious villa with stunning ocean views and private beach access",
                    Rate = 1500.0,
                    Sqft = 200,
                    Occupancy = 6,
                    ImageUrl = "C:\\Users\\lenovo\\Desktop\\images.jpg",
                    CreateData = new DateTime(2024, 5, 1),
                    UpdateData = new DateTime(2024, 5, 1)
                });
            
        }
    }
}
