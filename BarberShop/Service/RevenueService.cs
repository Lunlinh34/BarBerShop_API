using booking.DTO;
using booking.Entity;
using BarberShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BarberShop.Entity;

namespace booking.Service
{
    public class RevenueService
    {
        private readonly IRepository<Booking> _bookingRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<ProductOrder> _productOrderRepo;
        private readonly IRepository<BarberShop.Entity.Service> _serviceRepo;
        private readonly IRepository<RevenueStatistic> _revenueRepo;

        public RevenueService(
            IRepository<Booking> bookingRepo,
            IRepository<Order> orderRepo,
            IRepository<ProductOrder> productOrderRepo,
            IRepository<BarberShop.Entity.Service> serviceRepo,
            IRepository<RevenueStatistic> revenueRepo)
        {
            _bookingRepo = bookingRepo;
            _orderRepo = orderRepo;
            _productOrderRepo = productOrderRepo;
            _serviceRepo = serviceRepo;
            _revenueRepo = revenueRepo;
        }

        public async Task<RevenueStatisticDTO> CalculateMonthlyRevenue(int month, int year)
        {
            var bookings = _bookingRepo.GetAll<Booking>()
                .Where(b => DateTime.TryParse($"{b.startDate}T{b.startTime}", out DateTime dt) &&
                            dt.Month == month && dt.Year == year)
                .ToList();

            var orders = _orderRepo.GetAll<Order>()
                .Where(o => o.orderDate.Month == month && o.orderDate.Year == year)
                .ToList();

            decimal totalRevenue = 0;
            int totalProducts = 0;
            int totalServices = bookings.Count;

            var records = new List<RevenueRecord>();

            // Tính revenue từ booking
            foreach (var b in bookings)
            {
                var service = _serviceRepo.GetById<BarberShop.Entity.Service>(b.serID);
                decimal price = (decimal?)(service?.serPrice) ?? 0m;
                totalRevenue += price;

                records.Add(new RevenueRecord
                {
                    Type = "Booking",
                    Name = service?.serName ?? "Dịch vụ không xác định",
                    Price = price,
                    Time = DateTime.Parse($"{b.startDate}T{b.startTime}"),
                    RelatedID = b.bookingID
                });
            }

            // Tính revenue từ orders
            foreach (var o in orders)
            {
                var productOrders = _productOrderRepo.GetAll<ProductOrder>()
                                    .Where(po => po.orderID == o.orderID)
                                    .ToList();

                totalProducts += productOrders.Sum(po => po.proOrderQuantity);
                totalRevenue += o.totalInvoice;

                foreach (var po in productOrders)
                {
                    records.Add(new RevenueRecord
                    {
                        Type = "Order",
                        Name = po.proID.ToString(), // có thể map sang tên sản phẩm nếu muốn
                        Price = o.totalInvoice, // có thể chia theo từng sản phẩm
                        Time = o.orderDate,
                        RelatedID = o.orderID
                    });
                }
            }

            var revenueEntity = new RevenueStatistic
            {
                Month = month,
                Year = year,
                TotalRevenue = totalRevenue,
                OrderCount = orders.Count,
                ProductOrderCount = totalProducts,
                ServiceOrderCount = totalServices,
                RecordsJson = JsonSerializer.Serialize(records),
                CreatedAt = DateTime.Now
            };

            await _revenueRepo.AddAsync(revenueEntity);

            // Trả về DTO
            var dto = new RevenueStatisticDTO
            {
                RevenueID = revenueEntity.RevenueID,
                Month = revenueEntity.Month,
                Year = revenueEntity.Year,
                TotalRevenue = revenueEntity.TotalRevenue,
                OrderCount = revenueEntity.OrderCount,
                ProductOrderCount = revenueEntity.ProductOrderCount,
                ServiceOrderCount = revenueEntity.ServiceOrderCount,
                CreatedAt = revenueEntity.CreatedAt,
                Records = records.Select(r => new RevenueRecordDTO
                {
                    Type = r.Type,
                    Name = r.Name,
                    Price = r.Price,
                    Time = r.Time,
                    RelatedID = r.RelatedID
                }).ToList()
            };

            return dto;
        }
    }
}
