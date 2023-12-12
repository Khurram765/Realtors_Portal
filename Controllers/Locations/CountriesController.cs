using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Realtors_Portal.Model;
using Realtors_Portal.Model.DTO.Locations;
using Realtors_Portal.Model.Locations;
using System;
using System.Diagnostics.Metrics;

namespace Realtors_Portal.Controllers.Locations
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountriesController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody]CountriesDTO countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var country = new Countries
            {
                CountryName = countryDto.CountryName,
                IsActive = countryDto.IsActive,
                IsDeleted = countryDto.IsDeleted
            };

            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountriesDTO>>> GetCountries()
        {
            var countries = await _context.Countries.Where(c => c.IsActive == "Y" && c.IsDeleted == "N").ToListAsync();
            return Ok(countries.Select(c => new CountriesDTO
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName,
                IsActive = c.IsActive,
                IsDeleted = c.IsDeleted
            }));
        }

        // GET api/countries/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CountriesDTO>> GetCountry(int id)
        {
            var country = await _context.Countries.Where(c => c.CountryId == id && c.IsActive == "Y" && c.IsDeleted == "N").FirstOrDefaultAsync();

            if (country == null)
            {
                return NotFound();
            }

            return Ok(new CountriesDTO
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName,
                IsActive = country.IsActive,
                IsDeleted = country.IsDeleted
            });
        }

        // PUT api/countries/{id}
        [HttpPut]
        public async Task<IActionResult> UpdateCountry([FromBody] CountriesDTO countryDto)
        {
            var country = await _context.Countries.FindAsync(countryDto.CountryId);

            if (country == null)
            {
                return NotFound();
            }

            country.CountryName = countryDto.CountryName;
            country.IsActive = countryDto.IsActive;
            country.IsDeleted = countryDto.IsDeleted;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE api/countries/{id}
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            country.IsDeleted = "Y";
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
