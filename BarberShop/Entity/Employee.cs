using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class Employee
    {
        [Key]
        public int employeID {get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? picture { get; set; }
        public string? email { get; set; }
        public string? numberphone { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime wordDay { get; set; }



        #region QH

        // OneOne User
        public int userID { get; set; }
        [ForeignKey("userID")]
        public User user { get; set; } = null!;
        // ManyOne 
        // Store
        public int storeID { get; set; }
        [ForeignKey("storeID")]
        public Store Store { get; set; } = null!;
        // Address
        public int addressID { get; set; }
        [ForeignKey("addressID")]
        public Address Address { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; }
        #endregion
    }
}
