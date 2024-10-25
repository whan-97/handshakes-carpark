using Handshakes_Carpark.Data;
using Handshakes_Carpark.Models;
using Handshakes_Carpark.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class CarParkRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public CarParkRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CarParkTestDatabase")
            .Options;

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.CarParks.AddRange(new List<CarPark>
            {
                new CarPark
                {
                    CarParkNo = "AAA",
                    Address = "BLK 270/271 ALBERT CENTRE BASEMENT CAR PARK",
                    XCoord = 30314.7936,
                    YCoord = 31490.4942,
                    CarParkType = "BASEMENT CAR PARK",
                    ParkingSystemType = "ELECTRONIC PARKING",
                    ShortTermParking = "WHOLE DAY",
                    FreeParking = "NO",
                    NightParking = true,
                    CarParkDecks = 1,
                    GantryHeight = 1.8,
                    CarParkBasement = true
                },
                new CarPark
                {
                    CarParkNo = "BBB",
                    Address = "BLK 123 MAIN STREET",
                    XCoord = 30315.1234,
                    YCoord = 31491.1234,
                    CarParkType = "OUTDOOR PARKING",
                    ParkingSystemType = "MANUAL PARKING",
                    ShortTermParking = "2 HOURS",
                    FreeParking = "SUN & PH FR 7AM-10.30PM",
                    NightParking = false,
                    CarParkDecks = 0,
                    GantryHeight = 2.0,
                    CarParkBasement = false
                },
                new CarPark
                {
                    CarParkNo = "CCC",
                    Address = "BLK 456 ELM STREET",
                    XCoord = 30316.5678,
                    YCoord = 31492.5678,
                    CarParkType = "BASEMENT CAR PARK",
                    ParkingSystemType = "ELECTRONIC PARKING",
                    ShortTermParking = "WHOLE DAY",
                    FreeParking = "SUN & PH FR 7AM-10.30PM",
                    NightParking = true,
                    CarParkDecks = 2,
                    GantryHeight = 3.0,
                    CarParkBasement = true
                }
            });
            context.SaveChanges();
        }
    }

    [Fact]
    public async Task GetCarParksWithFreeParking_ReturnsParksWithFreeParking()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var repository = new CarParkRepository(context);

            // Act
            var result = await repository.GetCarParksWithFreeParking();

            // Assert
            Assert.Equal(2, result.Count); 
            Assert.All(result, carPark => Assert.NotEqual("NO", carPark.FreeParking));
        }
    }

    [Fact]
    public async Task GetCarParksWithNightParking_ReturnsNightParks()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var repository = new CarParkRepository(context);

            // Act
            var result = await repository.GetCarParksWithNightParking();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, carPark => Assert.True(carPark.NightParking));
        }
    }

    [Fact]
    public async Task GetCarParksWithHeightRequirement_ReturnsParksAboveHeight()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var repository = new CarParkRepository(context);

            // Act
            var result = await repository.GetCarParksWithHeightRequirement(2.0);

            // Assert
            Assert.Single(result); 
            Assert.All(result, carPark => Assert.True(carPark.GantryHeight > 2.0));
        }
    }

    [Fact]
    public async Task AddFavoriteCarParkAsync_ValidCarPark_MarksAsFavorite()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var repository = new CarParkRepository(context);

            // Act
            var result = await repository.AddFavoriteCarParkAsync("AAA");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.FavoriteCarPark);
        }
    }

    [Fact]
    public async Task AddFavoriteCarParkAsync_InvalidCarPark_ReturnsNull()
    {
        using (var context = new ApplicationDbContext(_options))
        {
            var repository = new CarParkRepository(context);

            // Act
            var result = await repository.AddFavoriteCarParkAsync("XYZ"); 

            // Assert
            Assert.Null(result);
        }
    }
}
