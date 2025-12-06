using BarberShop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNDbController : ControllerBase
    {
        private readonly VNDbContext _context;

        public VNDbController(VNDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProvinces()
        {
            var provinces = await _context.Provinces
                .OrderBy(p => p.Name)
                .Select(p => new { p.Code, p.Name })
                .ToListAsync();

            return Ok(provinces);
        }

        [HttpGet("{provinceCode}/districts")]
        public async Task<IActionResult> GetDistricts(string provinceCode)
        {
            var districts = await _context.Wards
                .Where(w => w.ProvinceCode == provinceCode)
                .OrderBy(w => w.Name)
                .Select(w => new { w.Code, w.Name })
                .ToListAsync();

            return Ok(districts);
        }
    }
}
