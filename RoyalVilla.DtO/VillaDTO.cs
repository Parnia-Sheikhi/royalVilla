using System.ComponentModel.DataAnnotations;

namespace RoyalVilla.DTO
{
    public class VillaDTO        // we must not expose our entity of db because of that we make DTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
    }
}
