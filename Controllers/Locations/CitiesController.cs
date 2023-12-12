using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Realtors_Portal.Model;
using Realtors_Portal.Model.DTO.Locations;
using Realtors_Portal.Model.Locations;

namespace Realtors_Portal.Controllers.Locations
{
    [ApiController]
    [Route("[controller]")]
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CitiesController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CitiesDTO citiesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = _mapper.Map<Cities>(citiesDto);

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitiesDTO>>> GetCities()
        {
            var cities = await _context.Cities
                .Include(p => p.Provinces).ThenInclude(c=>c.Countries).Where(c => c.IsActive == "Y" && c.IsDeleted == "N")
                .ToListAsync();

            return Ok(cities.Select(c => new Cities
            {
                CityId = c.CityId,
                CityName = c.CityName,
                IsActive = c.IsActive,
                IsDeleted = c.IsDeleted,
                ProvinceId = c.ProvinceId,
                Provinces = new Provinces
                {
                    ProvinceId = c.ProvinceId,
                    ProvinceName = c.Provinces.ProvinceName,
                    CountryId = c.Provinces.CountryId,
                    IsActive = c.Provinces.IsActive,
                    IsDeleted = c.Provinces.IsDeleted,
                    Countries = new Countries
                    {
                        CountryId = c.Provinces.Countries.CountryId,
                        CountryName = c.Provinces.Countries.CountryName,
                        IsActive = c.Provinces.Countries.IsActive,
                        IsDeleted = c.Provinces.Countries.IsDeleted
                    }
                }

            })); ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CitiesDTO>> GetCity(int id)
        {
            var city = await _context.Cities
                .Include(p => p.Provinces).ThenInclude(c=>c.Countries).Where(p => p.IsActive == "Y" && p.IsDeleted == "N")
                .FirstOrDefaultAsync(p => p.CityId == id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(new Cities
            {
                CityId = city.CityId,
                CityName = city.CityName,
                IsActive = city.IsActive,
                IsDeleted = city.IsDeleted,
                ProvinceId = city.ProvinceId,
                Provinces = new Provinces
                {
                    ProvinceId = city.Provinces.ProvinceId,
                    ProvinceName = city.Provinces.ProvinceName,
                    CountryId = city.Provinces.CountryId,
                    IsActive = city.Provinces.IsActive,
                    IsDeleted = city.Provinces.IsDeleted,
                    Countries = new Countries
                    {
                        CountryId = city.Provinces.Countries.CountryId,
                        CountryName = city.Provinces.Countries.CountryName,
                        IsActive = city.Provinces.Countries.IsActive,
                        IsDeleted = city.Provinces.Countries.IsDeleted
                    }
                }
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCity([FromBody] CitiesDTO cityDto)
        {

            var city = await _context.Cities.FindAsync(cityDto.CityId);

            if (city == null)
            {
                return NotFound();
            }

            city.CityName = cityDto.CityName;
            city.ProvinceId = cityDto.ProvinceId;
            city.IsActive = cityDto.IsActive;
            city.IsDeleted = cityDto.IsDeleted;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {

            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            
            city.IsDeleted = "Y";

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
