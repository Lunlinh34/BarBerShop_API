/*  using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Repository;
using booking.DTO;
using booking.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueServiceController : ControllerBase
    {
        private readonly IRepository<Booking> _bookingRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<ProductOrder> _productOrderRepo;
        private readonly IRepository<Services> _serviceRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<RevenueStatistic> _revenueRepo;

        public RevenueServiceController(
            IRepository<Booking> bookingRepo,
            IRepository<Order> orderRepo,
            IRepository<ProductOrder> productOrderRepo,
            IRepository<Services> serviceRepo,
            IRepository<Product> productRepo,
            IRepository<RevenueStatistic> revenueRepo)
        {
            _bookingRepo = bookingRepo;
            _orderRepo = orderRepo;
            _productOrderRepo = productOrderRepo;
            _serviceRepo = serviceRepo;
            _productRepo = productRepo;
            _revenueRepo = revenueRepo;
        }

        // POST: api/revenue/generate?month=11&year=2025
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMonthlyRevenue(int month, int year)
        {
            // Lấy booking theo tháng
            var bookings = _bookingRepo.GetAll<Booking>()
                .Where(b =>
                {
                    if (DateTime.TryParse($"{b.startDate}T{b.startTime}", out DateTime bookingDate))
                        return bookingDate.Month == month && bookingDate.Year == year;
                    return false;
                }).ToList();

            // Lấy order theo tháng
            var orders = _orderRepo.GetAll<Order>()
                .Where(o => o.orderDate.Month == month && o.orderDate.Year == year)
                .ToList();

            var records = new List<RevenueRecord>();

            // Booking
            foreach (var b in bookings)
            {
                var service = _serviceRepo.GetById<Services>(b.serID);
                decimal price = (decimal?)(service?.serPrice) ?? 0m;

                records.Add(new RevenueRecord
                {
                    Type = "Booking",
                    Name = service?.serName ?? "Dịch vụ không xác định",
                    Price = price,
                    Time = DateTime.Parse($"{b.startDate}T{b.startTime}"),
                    RelatedID = b.bookingID
                });
            }

            // Order
            foreach (var o in orders)
            {
                var productOrders = _productOrderRepo.GetAll<ProductOrder>()
                    .Where(po => po.orderID == o.orderID)
                    .ToList();

                foreach (var po in productOrders)
                {
                    decimal price = 0m;
                    string productName = "Sản phẩm không xác định";

                    if (po.proID != 0)
                    {
                        var product = _productRepo.GetById<Product>(po.proID);
                        if (product != null)
                        {
                            productName = product.proName ?? "Sản phẩm không xác định";
                            price = (decimal)product.price;
                        }
                    }

                    records.Add(new RevenueRecord
                    {
                        Type = "Order",
                        Name = productName,
                        Price = price,
                        Time = o.orderDate,
                        RelatedID = o.orderID
                    });
                }
            }

            decimal totalRevenue = records.Sum(r => r.Price);

            var statistic = new RevenueStatistic
            {
                Month = month,
                Year = year,
                TotalRevenue = totalRevenue,
                OrderCount = orders.Count,
                ProductOrderCount = orders.Sum(o => _productOrderRepo.GetAll<ProductOrder>().Count(po => po.orderID == o.orderID)),
                ServiceOrderCount = bookings.Count,
                RecordsJson = JsonSerializer.Serialize(records),
                CreatedAt = DateTime.Now
            };

            await _revenueRepo.AddAsync(statistic);

            // Tạo DTO trả về FE
            var dto = new RevenueStatisticDTO
            {
                RevenueID = statistic.RevenueID,
                Month = statistic.Month,
                Year = statistic.Year,
                TotalRevenue = statistic.TotalRevenue,
                OrderCount = statistic.OrderCount,
                ProductOrderCount = statistic.ProductOrderCount,
                ServiceOrderCount = statistic.ServiceOrderCount,
                CreatedAt = statistic.CreatedAt,
                Records = JsonSerializer.Deserialize<List<RevenueRecordDTO>>(statistic.RecordsJson) ?? new List<RevenueRecordDTO>()
            };

            return Ok(new { message = $"✅ Đã tạo thống kê cho tháng {month}/{year}", data = dto });
        }

        // GET: api/revenue/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var data = _revenueRepo.GetAll<RevenueStatistic>()
                .OrderByDescending(r => r.CreatedAt)
                .Select(s => new RevenueStatisticDTO
                {
                    RevenueID = s.RevenueID,
                    Month = s.Month,
                    Year = s.Year,
                    TotalRevenue = s.TotalRevenue,
                    OrderCount = s.OrderCount,
                    ProductOrderCount = s.ProductOrderCount,
                    ServiceOrderCount = s.ServiceOrderCount,
                    CreatedAt = s.CreatedAt,
                    Records = JsonSerializer.Deserialize<List<RevenueRecordDTO>>(s.RecordsJson) ?? new List<RevenueRecordDTO>()
                })
                .ToList();

            return Ok(data);
        }

        // DELETE: api/revenue/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var statistic = await _revenueRepo.GetByIdAsync(id);
            if (statistic == null) return NotFound("Không tìm thấy bản thống kê");

            await _revenueRepo.DeleteAsync(id);
            return Ok("Đã xoá bản thống kê thành công");
        }
    }
}
*/using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Repository;
using booking.DTO;
using booking.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueServiceController : ControllerBase
    {
        private readonly IRepository<Booking> _bookingRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<ProductOrder> _productOrderRepo;
        private readonly IRepository<BarberShop.Entity.Service> _serviceRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<RevenueStatistic> _revenueRepo;

        public RevenueServiceController(
            IRepository<Booking> bookingRepo,
            IRepository<Order> orderRepo,
            IRepository<ProductOrder> productOrderRepo,
            IRepository<BarberShop.Entity.Service> serviceRepo,
            IRepository<Product> productRepo,
            IRepository<RevenueStatistic> revenueRepo)
        {
            _bookingRepo = bookingRepo;
            _orderRepo = orderRepo;
            _productOrderRepo = productOrderRepo;
            _serviceRepo = serviceRepo;
            _productRepo = productRepo;
            _revenueRepo = revenueRepo;
        }
        /*
        // POST: api/revenue/generate?month=11&year=2025
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMonthlyRevenue(int month, int year)
        {
            var bookings = _bookingRepo.GetAll<Booking>()
                .Where(b => DateTime.TryParse($"{b.startDate}T{b.startTime}", out DateTime bookingDate)
                            && bookingDate.Month == month && bookingDate.Year == year)
                .ToList();

            var orders = _orderRepo.GetAll<Order>()
                .Where(o => o.orderDate.Month == month && o.orderDate.Year == year)
                .ToList();

            var records = new List<RevenueRecord>();

            // Booking records
            foreach (var b in bookings)
            {
                var service = _serviceRepo.GetById<Services>(b.serID);
                records.Add(new RevenueRecord
                {
                    Type = "Booking",
                    Name = service?.serName ?? "Dịch vụ không xác định",
                    Price = (decimal?)(service?.serPrice) ?? 0m,
                    Time = DateTime.Parse($"{b.startDate}T{b.startTime}"),
                    RelatedID = b.bookingID
                });
            }

            // Order records
            foreach (var o in orders)
            {
                var productOrders = _productOrderRepo.GetAll<ProductOrder>()
                    .Where(po => po.orderID == o.orderID)
                    .ToList();
                if (productOrders.Any())
                {
                    // Lấy danh sách tên sản phẩm và tổng giá
                    var productNames = productOrders
                        .Select(po => _productRepo.GetById<Product>(po.proID)?.proName ?? "Sản phẩm không xác định")
                        .ToList();

                    var totalPrice = productOrders
                        .Select(po => (decimal?)(_productRepo.GetById<Product>(po.proID)?.price) ?? 0m)
                        .Sum();

                    records.Add(new RevenueRecord
                    {
                        Type = "Order",
                        Name = string.Join(" | ", productNames), // nối các tên sản phẩm bằng |
                        Price = totalPrice,
                        Time = o.orderDate,
                        RelatedID = o.orderID
                    });
                }

            }

            var statistic = new RevenueStatistic
            {
                Month = month,
                Year = year,
                TotalRevenue = records.Sum(r => r.Price),
                OrderCount = orders.Count,
                ProductOrderCount = orders.Sum(o => _productOrderRepo.GetAll<ProductOrder>().Count(po => po.orderID == o.orderID)),
                ServiceOrderCount = bookings.Count,
                RecordsJson = JsonSerializer.Serialize(records),
                CreatedAt = DateTime.Now
            };

            await _revenueRepo.AddAsync(statistic);

            // Tạo DTO trả về frontend
            var dto = new RevenueStatisticDTO
            {
                RevenueID = statistic.RevenueID,
                Month = statistic.Month,
                Year = statistic.Year,
                TotalRevenue = statistic.TotalRevenue,
                OrderCount = statistic.OrderCount,
                ProductOrderCount = statistic.ProductOrderCount,
                ServiceOrderCount = statistic.ServiceOrderCount,
                CreatedAt = statistic.CreatedAt,
                Records = JsonSerializer.Deserialize<List<RevenueRecordDTO>>(statistic.RecordsJson) ?? new List<RevenueRecordDTO>()
            };

            return Ok(new { message = $"✅ Đã tạo thống kê cho tháng {month}/{year}", data = dto });
        }
        */
        // POST: api/revenue/generate?month=11&year=2025
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateMonthlyRevenue(int month, int year)
        {
            // Lấy booking theo tháng
            var bookings = _bookingRepo.GetAll<Booking>()
                .Where(b => DateTime.TryParse($"{b.startDate}T{b.startTime}", out DateTime bookingDate)
                            && bookingDate.Month == month && bookingDate.Year == year)
                .ToList();

            // Lấy order theo tháng
            var orders = _orderRepo.GetAll<Order>()
                .Where(o => o.orderDate.Month == month && o.orderDate.Year == year)
                .ToList();

            var records = new List<RevenueRecord>();

            // --- Booking records ---
            foreach (var b in bookings)
            {
                var service = _serviceRepo.GetById<BarberShop.Entity.Service>(b.serID);

                records.Add(new RevenueRecord
                {
                    Type = "Booking",
                    Name = service != null ? service.serName : "Dịch vụ đã bị xoá",
                    Price = service != null ? (decimal)service.serPrice : 0m,
                    Time = DateTime.Parse($"{b.startDate}T{b.startTime}"),
                    RelatedID = b.bookingID
                });
            }

            // --- Order records ---
            foreach (var o in orders)
            {
                var productOrders = _productOrderRepo.GetAll<ProductOrder>()
                    .Where(po => po.orderID == o.orderID)
                    .ToList();

                // Nếu order có sản phẩm
                if (productOrders.Any())
                {
                    var productNames = productOrders
                        .Select(po => _productRepo.GetById<Product>(po.proID)?.proName ?? "Sản phẩm đã bị xoá")
                        .ToList();

                    var totalPrice = productOrders
                        .Select(po => (decimal?)(_productRepo.GetById<Product>(po.proID)?.price) ?? 0m)
                        .Sum();

                    records.Add(new RevenueRecord
                    {
                        Type = "Order",
                        Name = string.Join(" | ", productNames),
                        Price = totalPrice,
                        Time = o.orderDate,
                        RelatedID = o.orderID
                    });
                }
                else
                {
                    // Nếu order không còn sản phẩm nào (bị xóa hết) vẫn tạo record
                    records.Add(new RevenueRecord
                    {
                        Type = "Order",
                        Name = "Sản phẩm đã bị xoá",
                        Price = 0m,
                        Time = o.orderDate,
                        RelatedID = o.orderID
                    });
                }
            }

            // Tạo thống kê
            var statistic = new RevenueStatistic
            {
                Month = month,
                Year = year,
                TotalRevenue = records.Sum(r => r.Price),
                OrderCount = orders.Count,
                ProductOrderCount = orders.Sum(o => _productOrderRepo.GetAll<ProductOrder>().Count(po => po.orderID == o.orderID)),
                ServiceOrderCount = bookings.Count,
                RecordsJson = JsonSerializer.Serialize(records),
                CreatedAt = DateTime.Now
            };

            await _revenueRepo.AddAsync(statistic);

            // Tạo DTO trả về FE
            var dto = new RevenueStatisticDTO
            {
                RevenueID = statistic.RevenueID,
                Month = statistic.Month,
                Year = statistic.Year,
                TotalRevenue = statistic.TotalRevenue,
                OrderCount = statistic.OrderCount,
                ProductOrderCount = statistic.ProductOrderCount,
                ServiceOrderCount = statistic.ServiceOrderCount,
                CreatedAt = statistic.CreatedAt,
                Records = JsonSerializer.Deserialize<List<RevenueRecordDTO>>(statistic.RecordsJson) ?? new List<RevenueRecordDTO>()
            };

            return Ok(new { message = $"✅ Đã tạo thống kê cho tháng {month}/{year}", data = dto });
        }

        // GET: api/revenue/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var data = _revenueRepo.GetAll<RevenueStatistic>()
                .OrderByDescending(r => r.CreatedAt)
                .Select(s => new RevenueStatisticDTO
                {
                    RevenueID = s.RevenueID,
                    Month = s.Month,
                    Year = s.Year,
                    TotalRevenue = s.TotalRevenue,
                    OrderCount = s.OrderCount,
                    ProductOrderCount = s.ProductOrderCount,
                    ServiceOrderCount = s.ServiceOrderCount,
                    CreatedAt = s.CreatedAt,
                    Records = JsonSerializer.Deserialize<List<RevenueRecordDTO>>(s.RecordsJson) ?? new List<RevenueRecordDTO>()
                })
                .ToList();

            return Ok(data);
        }

        // DELETE: api/revenue/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var statistic = await _revenueRepo.GetByIdAsync(id);
            if (statistic == null) return NotFound("Không tìm thấy bản thống kê");

            await _revenueRepo.DeleteAsync(id);
            return Ok("Đã xoá bản thống kê thành công");
        }
    }
}
