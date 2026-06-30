using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalVilla_API.Models
{
    public class VillaAmenities
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreateDate { get; set; }
        
        public DateTime UpdateDate { get; set; }

        [Required]
        [ForeignKey(nameof(Villa))]
        public int VillaId { get; set; }
        public Villa? Villa { get; set; }
    }
}
