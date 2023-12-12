using System.ComponentModel.DataAnnotations;

namespace Realtors_Portal.Model.DTO.Locations
{
    public class CountriesDTO
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }

        public string? IsActive { get; set; }

        public string? IsDeleted { get; set; }
    }
}
