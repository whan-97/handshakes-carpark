using Handshakes_Carpark.Models;

namespace Handshakes_Carpark.Repository
{
    public interface ICarParkRepository
    {
        Task<List<CarPark>> GetCarParksWithFreeParking();
        Task<List<CarPark>> GetCarParksWithNightParking();
        Task<List<CarPark>> GetCarParksWithHeightRequirement(double heightRequirement);
        Task<CarPark?> AddFavoriteCarParkAsync(string carParkNo);

    }
}
