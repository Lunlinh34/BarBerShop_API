using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Repository;
using booking.DTO;
using booking.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRepository<Booking> _bookingRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<ProductOrder> _productOrderRepo;
        private readonly IRepository<RevenueStatistic> _revenueRepo;
        private readonly IRepository<BarberShop.Entity.Service> _serviceRepo;

        public RevenueController(
            IRepository<Booking> bookingRepo,
            IRepository<Order> orderRepo,
            IRepository<ProductOrder> productOrderRepo,
            IRepository<RevenueStatistic> revenueRepo,
            IRepository<BarberShop.Entity.Service> serviceRepo)
        {
            _bookingRepo = bookingRepo;
            _orderRepo = orderRepo;
            _productOrderRepo = productOrderRepo;
            _revenueRepo = revenueRepo;
            _serviceRepo = serviceRepo;
        }

        // 📊 Tạo thống kê cho tháng được chọn
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMonthlyRevenue(int month, int year)
        {
            // Lấy tất cả Booking trong tháng
            var bookings = _bookingRepo.GetAll<Booking>()
                .Where(b =>
                {
                    if (DateTime.TryParse($"{b.startDate}T{b.startTime}", out DateTime bookingDate))
                        return bookingDate.Month == month && bookingDate.Year == year;
                    return false;
                })
                .ToList();

            // Lấy tất cả Order trong tháng
            var orders = _orderRepo.GetAll<Order>()
                .Where(o => o.orderDate.Month == month && o.orderDate.Year == year)
                .ToList();

            // Tính tổng sản phẩm được đặt
            int totalProductCount = 0;
            foreach (var order in orders)
            {
                var productOrders = _productOrderRepo.GetAll<ProductOrder>()
                    .Where(po => po.orderID == order.orderID)
                    .ToList();
                totalProductCount += productOrders.Sum(po => po.proOrderQuantity);
            }

            // Tính tổng dịch vụ
            int totalServiceCount = bookings.Count;

            // Tính tổng doanh thu từ Booking (lấy giá từ Service)
            decimal totalBookingRevenue = 0;
            foreach (var booking in bookings)
            {
                try
                {
                    var service = _serviceRepo.GetById<ServicesDto>(booking.serID);
                    if (service != null)
                        totalBookingRevenue += (decimal)service.serPrice;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi lấy giá dịch vụ bookingID {booking.bookingID}: {ex.Message}");
                }
            }

            // Tổng doanh thu = Booking + Order
            decimal totalRevenue = totalBookingRevenue + orders.Sum(o => o.totalInvoice);

            // Tạo bản ghi thống kê
            var statistic = new RevenueStatistic
            {
                Month = month,
                Year = year,
                TotalRevenue = totalRevenue,
                OrderCount = orders.Count,
                ProductOrderCount = totalProductCount,
                ServiceOrderCount = totalServiceCount,
                CreatedAt = DateTime.Now
            };

            await _revenueRepo.AddAsync(statistic);

            // Trả về DTO
            var dto = new RevenueStatisticDTO
            {
                RevenueID = statistic.RevenueID,
                Month = statistic.Month,
                Year = statistic.Year,
                TotalRevenue = statistic.TotalRevenue,
                OrderCount = statistic.OrderCount,
                ProductOrderCount = statistic.ProductOrderCount,
                ServiceOrderCount = statistic.ServiceOrderCount,
                CreatedAt = statistic.CreatedAt
            };

            return Ok(new { message = $"✅ Đã tạo thống kê cho tháng {month}/{year}", data = dto });
        }

        // 📋 Lấy tất cả thống kê, mới nhất → cũ nhất
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var data = _revenueRepo.GetAll<RevenueStatistic>()
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return Ok(data);
        }

        // ❌ Xoá một thống kê
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _revenueRepo.GetByIdAsync(id);
            if (item == null)
                return NotFound("❌ Không tìm thấy bản thống kê.");

            await _revenueRepo.DeleteAsync(id);
            return Ok("🗑️ Đã xoá bản thống kê thành công.");
        }
    }
}
