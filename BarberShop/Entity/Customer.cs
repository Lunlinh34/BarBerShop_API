using BarberShop.Support;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class Customer
    {
        [Key]
        public int customerID { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? picture { get; set; }
        public string? email { get; set; }
        public string? numberphone { get; set; }
        public DateTime dateOfBirth { get; set; }


        #region QH

        // MM 
            // Address
        public ICollection<CustomerAddress> addresses { get; set; } = new HashSet<CustomerAddress>();
        // OO
            // User
        public int userID { get; set; }
        [ForeignKey("userID")]
        public User User { get; set; } = null!;
        // OneMany
        // Order
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        // Booking
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        // Evaluate
        public ICollection<Evaluate> Evaluations { get; set; } = new HashSet<Evaluate>();
        // CusNoti
        public ICollection<CustomerNotification> CustomerNotifications { get; set; } = new HashSet<CustomerNotification>();
        #endregion
    }
}
