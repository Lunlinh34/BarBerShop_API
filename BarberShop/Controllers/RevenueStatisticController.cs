using BarberShop.Entity;
using BarberShop.Unit;
using booking.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueStatisticController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public RevenueStatisticController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 🧮 Tạo thống kê cho tháng hiện tại
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMonthlyRevenue()
        {
            var now = DateTime.Now;
            var month = now.Month;
            var year = now.Year;

            // Lấy tất cả đơn hàng đã thanh toán trong tháng hiện tại
            var orders = await _unitOfWork.OrderRepository
                .GetQuery()
                .Where(o => o.orderDate.Month == month &&
                            o.orderDate.Year == year &&
                            o.orderStatus == "2")
                .ToListAsync();

            if (!orders.Any())
            {
                // Không có đơn hàng → trả về bản thống kê rỗng
                var emptyStat = new RevenueStatistic
                {
                    Month = month,
                    Year = year,
                    OrderCount = 0,
                    TotalRevenue = 0,
                    ProductOrderCount = 0,
                    ServiceOrderCount = 0,
                    CreatedAt = DateTime.Now
                };
                return Ok(new
                {
                    message = $"⚠️ Không có đơn hàng nào trong tháng {month}/{year}",
                    data = emptyStat
                });
            }

            // Lấy tất cả ProductOrder liên quan đến các đơn hàng này
            var productOrders = await _unitOfWork.ProductOrderRepository
                .GetQuery()
                .Where(po => orders.Select(o => o.orderID).Contains(po.orderID))
                .ToListAsync();

            // Phân loại đơn hàng
            int productCount = 0;
            int serviceCount = 0;

            foreach (var order in orders)
            {
                if (productOrders.Any(po => po.orderID == order.orderID))
                    productCount++; // Có sản phẩm → đơn sản phẩm
                else
                    serviceCount++; // Không có sản phẩm → đơn dịch vụ
            }

            // Tính tổng doanh thu
            var totalRevenue = orders.Sum(o => o.totalInvoice);

            // Tạo bản ghi thống kê mới
            var statistic = new RevenueStatistic
            {
                Month = month,
                Year = year,
                OrderCount = orders.Count,
                TotalRevenue = totalRevenue,
                ProductOrderCount = productCount,
                ServiceOrderCount = serviceCount,
                CreatedAt = DateTime.Now
            };

            // Lưu vào cơ sở dữ liệu
            await _unitOfWork.RevenueStatisticRepo.AddAsync(statistic);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                message = $"✅ Đã tạo thống kê doanh thu mới cho tháng {month}/{year}",
                data = statistic
            });
        }


        // 📋 Lấy tất cả thống kê theo thứ tự mới nhất → cũ nhất
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.RevenueStatisticRepo
                .GetQuery()
                .OrderByDescending(r => r.CreatedAt) // Sắp xếp theo thời gian tạo giảm dần
                .ToListAsync();

            return Ok(data);
        }

        // ❌ Xoá một thống kê
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _unitOfWork.RevenueStatisticRepo.GetByIdAsync(id);
            if (item == null)
                return NotFound("❌ Không tìm thấy bản thống kê.");

            await _unitOfWork.RevenueStatisticRepo.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return Ok("🗑️ Đã xoá bản thống kê thành công.");
        }
    }
}
