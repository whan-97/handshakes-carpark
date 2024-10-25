namespace Handshakes_Carpark.Models
{
    public class CarPark
    {
        public string? CarParkNo { get; set; }
        public string? Address {  get; set; }
        public double XCoord { get; set; }
        public double YCoord { get; set; }
        public string? CarParkType { get; set; }
        public string? ParkingSystemType { get; set; }
        public string? ShortTermParking { get; set; }
        public string? FreeParking { get; set; }
        public bool NightParking { get; set; }
        public int CarParkDecks { get; set; }
        public double GantryHeight { get; set; }
        public bool CarParkBasement { get; set; }
        public bool FavoriteCarPark { get; set; }


    }
}
