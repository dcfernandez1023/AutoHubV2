﻿namespace AutoHub.Models
{
    public class Vehicle
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Name { get; set; }
        public required long Mileage { get; set; }
        public required int Year { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required string LicensePlate { get; set; }
        public required string Vin { get; set; }
        public required string Notes { get; set; }
        public required long DateCreated { get; set; }
        public required IList<Guid> SharedWith { get; set; }
        public required string Base64Image { get; set; }
    }
}
