// StoreRequest.cs (Entity)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberShop.Entity
{
    [Table("StoreRequest")]
    public class StoreRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int WorkingHourID { get; set; }

        public int WarehouseID { get; set; }

        public int AddressID { get; set; }

        public int StoreID { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // 🔹 Thêm UserID
        [Required]
        public int UserID { get; set; }
    }
}
