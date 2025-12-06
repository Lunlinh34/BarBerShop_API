using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.DTO
{
    public class BookingDto
    {
        public int bookingID { get; set; }
        public string? startDate { get; set; }
        public string? startTime { get; set; }
        public string? note { get; set; }
        public int customerID { get; set; }
        public int storeID { get; set; }
        public int serID { get; set; }
        public int employeID { get; set; }
    }
}
