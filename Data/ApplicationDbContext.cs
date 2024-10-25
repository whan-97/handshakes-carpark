using Handshakes_Carpark.Models;
using Microsoft.EntityFrameworkCore;

namespace Handshakes_Carpark.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CarPark> CarParks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set CarParkNo as Primary Key
            modelBuilder.Entity<CarPark>()
            .HasKey(u => u.CarParkNo);

            // set default value of FavoriteCarPark to be false for each CarPark
            modelBuilder.Entity<CarPark>()
            .Property(e => e.FavoriteCarPark)
            .HasDefaultValue(false);

            modelBuilder.Entity<CarPark>().ToTable("CarParks");
        }
    }
}