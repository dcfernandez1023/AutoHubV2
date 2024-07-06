namespace AutoHub.Models
{
    public class Vehicle
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public required long Mileage { get; set; }
        public required int Year { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required string LicensePlate { get; set; }
        public required string Vin { get; set; }
        public required string Notes { get; set; }
        public required long DateCreated { get; set; }
        public required IList<string> SharedWith { get; set; }
        public required string ImageUrl { get; set; }
    }
}
