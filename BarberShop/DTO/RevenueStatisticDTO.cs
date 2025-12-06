using System;
using System.Collections.Generic;

namespace booking.DTO
{
    public class RevenueRecordDTO
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Time { get; set; }
        public int RelatedID { get; set; }
    }

    public class RevenueStatisticDTO
    {
        public int RevenueID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
        public int ProductOrderCount { get; set; }
        public int ServiceOrderCount { get; set; }
        public DateTime CreatedAt { get; set; }

        // Danh sách chi tiết từng record
        public List<RevenueRecordDTO> Records { get; set; } = new List<RevenueRecordDTO>();
    }
}
