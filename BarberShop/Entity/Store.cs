using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class Store
    {
        [Key]
        public int storeID { get; set; }
        public string? storeName { get; set; }
        public string? numberphone { get; set; }

        public int workingHourID { get; set; }
        [ForeignKey("workingHourID")]
        public WorkingHour workingHour { get; set; } = null!;

        public int? AddressID { get; set; }
        [ForeignKey("AddressID")]
        public Address? Address { get; set; }

        // Thêm trường ảnh
        public string? ImageUrl { get; set; }

        #region QH
        public ICollection<Warehouse> warehouse { get; set; } = new HashSet<Warehouse>();
        public ICollection<ServiceManagement> serviceManagement { get; set; } = new HashSet<ServiceManagement>();
        public ICollection<LocationStore> locationStore { get; set; } = new HashSet<LocationStore>();
        public ICollection<Evaluate> evaluates { get; set; } = new HashSet<Evaluate>();
        public ICollection<Employee> employees { get; set; } = new HashSet<Employee>();
        public virtual ICollection<Booking>? Bookings { get; set; }
        #endregion
    }
}
