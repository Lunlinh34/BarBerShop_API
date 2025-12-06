using BarberShop.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.DTO
{
    public class CustomerAddressDto
    {
        public int cusAddressId { get; set; }

        public int customerID { get; set; }
        public int addressID { get; set; } 
    }
}
