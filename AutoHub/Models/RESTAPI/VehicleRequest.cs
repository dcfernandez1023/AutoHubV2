namespace AutoHub.Models.RESTAPI
{
    public class VehicleRequest
    {
        public required string Name { get; set; }
        public required long Mileage { get; set; }
        public required int Year { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required string LicensePlate { get; set; }
        public required string Vin { get; set; }
        public required string Notes { get; set; }
        public required string Base64Image { get; set; }
    }
}
