using System.ComponentModel.DataAnnotations;

namespace Realtors_Portal.Model.Locations
{
    public class Cities
    {
        [Key]
        public int CityId { get; set; }
        [Required]
        public string? CityName { get; set; }
        [Required]
        public int ProvinceId { get; set; }
        [Required]
        public string? IsActive { get; set; }
        [Required]
        public string? IsDeleted { get; set; }

        public virtual Provinces Provinces { get; set; }
    }
}
