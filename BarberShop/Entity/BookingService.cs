using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class BookingService
    {
        [Key]
        public int bookingServiceID { get; set; }


        #region QH

        // ManytoOne
            // Booking
        public int bookingID { get; set; }
        [ForeignKey("bookingID")]
        public Booking booking { get; set; } = null!;
            // Service
        public int serviceID { get; set; }
        [ForeignKey("serviceID")]
        public Service Service { get; set; } = null!;
        #endregion
    }
}
