using System.ComponentModel.DataAnnotations;

namespace Realtors_Portal.Model.DTO.Locations
{
    public class ProvincesDTO
    {
        public int ProvinceId { get; set; }
        public string? ProvinceName { get; set; }

        public string? IsActive { get; set; }
 
        public string? IsDeleted { get; set; }
        public int CountryId { get; set; }

        //public CountriesDTO Countries { get; set; }
    }
}
