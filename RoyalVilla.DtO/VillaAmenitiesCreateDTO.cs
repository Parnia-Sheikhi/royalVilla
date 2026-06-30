using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalVilla.DTO
{
    public class VillaAmenitiesCreateDTO
    {
        
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        // DTOs do not need the foreign relation
        [Required]
        public int VillaId { get; set; }
    }
}
