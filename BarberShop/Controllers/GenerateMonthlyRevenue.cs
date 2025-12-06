using BarberShop.Data;
using BarberShop.Entity;
using booking.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateMonthlyRevenueController : ControllerBase
    {
        private readonly BarberShopContext _context;

        public GenerateMonthlyRevenueController(BarberShopContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllRevenue")]
        public async Task<IActionResult> GetAllRevenue()
        {
            var list = await _context.RevenueStatistics
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return Ok(list);
        }

        /// <summary>
        /// Tạo hoặc cập nhật thống kê doanh thu theo tháng
        /// </summary>
        [HttpPost("GenerateMonthlyRevenue")]
        public async Task<IActionResult> GenerateMonthlyRevenue(int month, int year)
        {
            var ordersInMonth = await _context.Orders
                .Where(o => o.orderDate.Month == month && o.orderDate.Year == year)
                .ToListAsync();

            if (ordersInMonth.Count == 0)
            {
                return NotFound(new { message = "❌ Không có đơn hàng nào trong tháng này." });
            }

            // Lấy danh sách order có sản phẩm
            var productOrderIds = await _context.ProductOrder
                .Select(p => p.orderID)
                .Distinct()
                .ToListAsync();

            int orderCount = ordersInMonth.Count;
            int productOrderCount = ordersInMonth.Count(o => productOrderIds.Contains(o.orderID));
            int serviceOrderCount = orderCount - productOrderCount;
            decimal totalRevenue = ordersInMonth.Sum(o => o.totalInvoice);

            // Kiểm tra đã có dữ liệu thống kê chưa
            var existing = await _context.RevenueStatistics
                .FirstOrDefaultAsync(r => r.Month == month && r.Year == year);

            if (existing != null)
            {
                existing.TotalRevenue = totalRevenue;
                existing.OrderCount = orderCount;
                existing.ProductOrderCount = productOrderCount;
                existing.ServiceOrderCount = serviceOrderCount;
                existing.CreatedAt = DateTime.Now;

                _context.RevenueStatistics.Update(existing);
            }
            else
            {
                var statistic = new RevenueStatistic
                {
                    Month = month,
                    Year = year,
                    TotalRevenue = totalRevenue,
                    OrderCount = orderCount,
                    ProductOrderCount = productOrderCount,
                    ServiceOrderCount = serviceOrderCount,
                    CreatedAt = DateTime.Now
                };

                await _context.RevenueStatistics.AddAsync(statistic);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "✅ Thống kê doanh thu tháng được cập nhật thành công.",
                month,
                year,
                totalRevenue,
                orderCount,
                productOrderCount,
                serviceOrderCount
            });
        }
    }
}
