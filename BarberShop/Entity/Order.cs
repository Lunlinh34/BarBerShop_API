using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    public class Order
    {
        [Key]
        public int orderID { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public string? orderStatus { get; set; }
        public int totalInvoice { get; set; }

        #region Quan hệ

        // Customer
        public int customerID { get; set; }
        [ForeignKey("customerID")]
        public Customer Customer { get; set; } = null!;

        // Payment
        public int payID { get; set; }
        [ForeignKey("payID")]
        public Payment Payment { get; set; } = null!;

        // Address
        public int? addressID { get; set; }
        [ForeignKey("addressID")]
        public Address? Address { get; set; } 

        // ProductOrders
        public ICollection<ProductOrder> ProductOrders { get; set; } = new HashSet<ProductOrder>();

        #endregion
    }
}
