namespace booking.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json;

[Table("RevenueStatistic")]

public class RevenueStatistic
{
    [Key]
    public int RevenueID { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalRevenue { get; set; }
    public int OrderCount { get; set; }
    public int ProductOrderCount { get; set; }
    public int ServiceOrderCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string RecordsJson { get; set; } = JsonSerializer.Serialize(new List<RevenueRecord>());
}
public class RevenueRecord
{
    public string Type { get; set; } // "Booking" hoặc "Order"
    public string Name { get; set; } // Tên dịch vụ / sản phẩm
    public decimal Price { get; set; } // Đơn giá
    public DateTime Time { get; set; } // Thời gian
    public int RelatedID { get; set; } // bookingID hoặc orderID
}