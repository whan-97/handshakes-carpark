using Handshakes_Carpark.Models;
using Handshakes_Carpark.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Handshakes_Carpark.Controllers
{
    [ApiController]
    [Route("carpark")] // Controller base route
    public class CarParkController : ControllerBase
    {
        private readonly ILogger<CarParkController> _logger;
        private readonly ICarParkRepository _carParkRepository;

        public CarParkController(ILogger<CarParkController> logger, ICarParkRepository carParkRepository)
        {
            _logger = logger;
            _carParkRepository = carParkRepository;
        }

        [HttpGet("free-parking")] 
        [ProducesResponseType(typeof(IEnumerable<CarPark>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CarPark>>> GetFreeParking()
        {
            // Get list of car parks where free parking != "NO".
            var freeParking = await _carParkRepository.GetCarParksWithFreeParking();

            if (!freeParking.Any())
            {
                return NotFound();
            }

            return Ok(freeParking);
        }

        [HttpGet("night-parking")]
        [ProducesResponseType(typeof(IEnumerable<CarPark>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CarPark>>> GetNightParking()
        {
            // Get list of car parks where night parking is true.
            var nightParking = await _carParkRepository.GetCarParksWithNightParking();

            if (!nightParking.Any())
            {
                return NotFound();
            }

            return Ok(nightParking);
        }

        [HttpGet("height-requirement")]
        [ProducesResponseType(typeof(IEnumerable<CarPark>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CarPark>>> GetCarParksWithHeight(double height)
        {
            // Get list of car parks that are > than input paramters.
            var availableCarParks = await _carParkRepository.GetCarParksWithHeightRequirement(height);

            if (!availableCarParks.Any())
            {
                return NotFound();
            }

            return Ok(availableCarParks);
        }


        [HttpPost("favorite-carpark")]
        [ProducesResponseType(typeof(CarPark), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarPark>> AddFavoriteCarPark(string carParkNo)
        {
            // Update the FavoriteCarPark field to true for existing records found with the same carParkNo
            var addedCarPark = await _carParkRepository.AddFavoriteCarParkAsync(carParkNo);

            // Return null if carParkNo record does not exist
            if (addedCarPark == null)
            {
                return NotFound(new { message = $"Car park number '{carParkNo}' not found." });
            }

            return Ok(addedCarPark);
        }

    }
}
