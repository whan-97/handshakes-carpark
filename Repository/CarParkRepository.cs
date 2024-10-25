using Handshakes_Carpark.Data;
using Handshakes_Carpark.Models;
using Microsoft.EntityFrameworkCore;

namespace Handshakes_Carpark.Repository
{
    public class CarParkRepository : ICarParkRepository
    {
        private readonly ApplicationDbContext _context;
        public CarParkRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CarPark>> GetCarParksWithFreeParking()
        {
            return await _context.CarParks.Where(x => x.FreeParking != "NO").ToListAsync();
        }
        public async Task<List<CarPark>> GetCarParksWithNightParking()
        {
            return await _context.CarParks.Where(x => x.NightParking == true).ToListAsync();
        }
        public async Task<List<CarPark>> GetCarParksWithHeightRequirement(double heightReq)
        {
            return await _context.CarParks.Where(x => x.GantryHeight > heightReq).ToListAsync();
        }
        public async Task<CarPark?> AddFavoriteCarParkAsync(string carParkNo)
        {
            var carPark = await _context.CarParks.FindAsync(carParkNo);

            if (carPark == null)
            {
                return null;
            }

            carPark.FavoriteCarPark = true;

            await _context.SaveChangesAsync();

            return carPark;
        }
    }
}
