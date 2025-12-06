using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class Booking
    {
        [Key]
        public int bookingID { get; set; }
        public string? startDate { get; set; }
        public string? startTime { get; set; }
        public string? note { get; set; }

        #region QH

        // ManytoOne
        // Customer
        public int customerID { get; set; }
        [ForeignKey("customerID")]
        public Customer customer { get; set; } = null!;
        // Payment
        //public int payID { get; set; }
        //[ForeignKey("payID")]
        //public Payment payment { get; set; } = null!;

        public int storeID { get; set; }
        [ForeignKey("storeID")]
        public Store Store { get; set; } = null!;

        public int serID { get; set; }
        [ForeignKey("serID")]
        public Service service { get; set; } = null!;

        public int employeID { get; set; }
        [ForeignKey("employeID")]
        public Employee employee { get; set; } = null!;
        

        // OnetoOne BookingSateDescription
        public BookingStateDescription? BookingSateDescription { get; set; }
        #endregion

    }

}
