using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Realtors_Portal.Model.Locations
{
    public class Provinces
    {
        [Key]
        public int ProvinceId { get; set; }
        [Required]
        public string? ProvinceName { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public string? IsActive { get; set; }
        [Required]
        public string? IsDeleted { get; set; }

        public virtual Countries Countries { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }


    }
}
