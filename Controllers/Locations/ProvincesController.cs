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
    public class ProvincesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProvincesController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProvince([FromBody]ProvincesDTO provinceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var province = _mapper.Map<Provinces>(provinceDto);

            await _context.Provinces.AddAsync(province);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProvincesDTO>>> GetProvinces()
        {
            var provinces = await _context.Provinces
                .Include(p => p.Countries).Where(p => p.IsActive == "Y" && p.IsDeleted == "N")
                .ToListAsync();

            return Ok(provinces.Select(p => new Provinces
            {
                ProvinceId = p.ProvinceId,
                ProvinceName = p.ProvinceName,
                IsActive = p.IsActive,
                IsDeleted = p.IsDeleted,
                CountryId = p.CountryId,
                Countries = new Countries
                {
                    CountryId = p.Countries.CountryId,
                    CountryName = p.Countries.CountryName,
                    IsActive = p.Countries.IsActive,
                    IsDeleted = p.Countries.IsDeleted
                }
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProvincesDTO>> GetProvince(int id)
        {
            var province = await _context.Provinces
                .Include(p => p.Countries).Where(p => p.IsActive == "Y" && p.IsDeleted == "N")
                .FirstOrDefaultAsync(p => p.ProvinceId == id);

            if (province == null)
            {
                return NotFound();
            }

            return Ok(new Provinces
            {
                ProvinceId = province.ProvinceId,
                ProvinceName = province.ProvinceName,
                CountryId = province.CountryId,
                IsActive = province.IsActive,
                IsDeleted = province.IsDeleted,
                Countries = new Countries
                {
                    CountryId = province.Countries.CountryId,
                    CountryName = province.Countries.CountryName,
                    IsActive = province.Countries.IsActive,
                    IsDeleted = province.Countries.IsDeleted
                }
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProvince([FromBody]ProvincesDTO provinceDto)
        {

            var province = await _context.Provinces.FindAsync(provinceDto.ProvinceId);

            if (province == null)
            {
                return NotFound();
            }

            province.ProvinceName = provinceDto.ProvinceName;
            province.CountryId = provinceDto.CountryId;
            province.IsActive = provinceDto.IsActive;
            province.IsDeleted = provinceDto.IsDeleted;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteProvince(int id)
        {

            var province = await _context.Provinces.FindAsync(id);

            if (province == null)
            {
                return NotFound();
            }

            
            province.IsDeleted = "Y";

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
