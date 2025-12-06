using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarberShop.Data;
using BarberShop.Entity;
using booking.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreRequestController : ControllerBase
    {
        private readonly BarberShopContext _context;

        public StoreRequestController(BarberShopContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/StoreRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreRequestDto>>> GetAll()
        {
            var storeRequests = await _context.StoreRequests
                .Select(sr => new StoreRequestDto
                {
                    Id = sr.Id,
                    WorkingHourID = sr.WorkingHourID,
                    WarehouseID = sr.WarehouseID,
                    AddressID = sr.AddressID,
                    StoreID = sr.StoreID,
                    Status = sr.Status,
                    UserID = sr.UserID // 🔹 đảm bảo UserID được trả về
                })
                .ToListAsync();

            return Ok(storeRequests);
        }

        // 🔹 GET: api/StoreRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoreRequestDto>> GetById(int id)
        {
            var sr = await _context.StoreRequests
                .Where(s => s.Id == id)
                .Select(s => new StoreRequestDto
                {
                    Id = s.Id,
                    WorkingHourID = s.WorkingHourID,
                    WarehouseID = s.WarehouseID,
                    AddressID = s.AddressID,
                    StoreID = s.StoreID,
                    Status = s.Status,
                    UserID = s.UserID
                })
                .FirstOrDefaultAsync();

            if (sr == null) return NotFound();

            return Ok(sr);
        }

        // 🔹 POST: api/StoreRequest
        [HttpPost]
        public async Task<ActionResult<StoreRequestDto>> Create(StoreRequestDto dto)
        {
            var storeRequest = new StoreRequest
            {
                WorkingHourID = dto.WorkingHourID,
                WarehouseID = dto.WarehouseID,
                AddressID = dto.AddressID,
                StoreID = dto.StoreID,
                UserID = dto.UserID,
                Status = dto.Status
            };

            _context.StoreRequests.Add(storeRequest);
            await _context.SaveChangesAsync();

            // Trả về DTO có UserID
            var resultDto = new StoreRequestDto
            {
                Id = storeRequest.Id,
                WorkingHourID = storeRequest.WorkingHourID,
                WarehouseID = storeRequest.WarehouseID,
                AddressID = storeRequest.AddressID,
                StoreID = storeRequest.StoreID,
                Status = storeRequest.Status,
                UserID = storeRequest.UserID
            };

            return CreatedAtAction(nameof(GetById), new { id = storeRequest.Id }, resultDto);
        }

        // 🔹 PUT: api/StoreRequest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StoreRequestDto dto)
        {
            var storeRequest = await _context.StoreRequests.FindAsync(id);
            if (storeRequest == null) return NotFound();

            storeRequest.WorkingHourID = dto.WorkingHourID;
            storeRequest.WarehouseID = dto.WarehouseID;
            storeRequest.AddressID = dto.AddressID;
            storeRequest.StoreID = dto.StoreID;
            storeRequest.UserID = dto.UserID;
            storeRequest.Status = dto.Status;

            _context.Entry(storeRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔹 DELETE: api/StoreRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var storeRequest = await _context.StoreRequests.FindAsync(id);
            if (storeRequest == null) return NotFound();

            _context.StoreRequests.Remove(storeRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
