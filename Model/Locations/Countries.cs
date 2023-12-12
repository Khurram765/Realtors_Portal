using System.ComponentModel.DataAnnotations;

namespace Realtors_Portal.Model.Locations
{
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }
        [Required]
        public string? CountryName { get; set; }
        [Required]
        public string? IsActive { get; set; }
        [Required]
        public string? IsDeleted { get; set; }

        public virtual ICollection<Provinces> Provinces { get; set; }
    }
}
