using System.ComponentModel.DataAnnotations;

namespace RoyalVilla_API.Models
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreateData { get; set; }
        public DateTime? UpdateData { get; set; }

        // in each villa there can be multiple Amenities associate to that villa id
        public ICollection<VillaAmenities>? Amenities { get; set; }

    }
}
