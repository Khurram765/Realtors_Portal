using Realtors_Portal.Model.Locations;
using System.ComponentModel.DataAnnotations;

namespace Realtors_Portal.Model.DTO.Locations
{
    public class CitiesDTO
    {
        public int CityId { get; set; }
        public string? CityName { get; set; }

        public string? IsActive { get; set; }

        public string? IsDeleted { get; set; }
        public int ProvinceId { get; set; }

        //public ProvincesDTO Provinces { get; set; }
    }
}
