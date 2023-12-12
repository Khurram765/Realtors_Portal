using AutoMapper;
using Realtors_Portal.Model.DTO.Locations;
using Realtors_Portal.Model.Locations;
using System.Diagnostics.Metrics;

namespace Realtors_Portal.Model.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            // Create mappings between DTOs and models
            CreateMap<CountriesDTO, Countries>().ReverseMap();

            // Province mappings
            CreateMap<ProvincesDTO, Provinces>()
    .ForMember(p => p.CountryId, opt => opt.MapFrom(dto => dto.CountryId)) // Map countryId directly
    .ReverseMap();

            // City mappings
            CreateMap<CitiesDTO, Cities>()
                .ForMember(c => c.ProvinceId, opt => opt.MapFrom(dto => dto.ProvinceId))
                .ReverseMap();

            // Additional mappings as needed
        }
    }
}
