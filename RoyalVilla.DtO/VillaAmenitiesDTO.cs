using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalVilla.DTO
{
    public class VillaAmenitiesDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        
        [Required]
       
        public int VillaId { get; set; }

        // we could get the whole villa but i want to have complexity in auto mapper when you converting VillaAmenities to VillaAmenitiesDTO then you have to populate VillaName 
        public string? VillaName { get; set; }
    }
}
