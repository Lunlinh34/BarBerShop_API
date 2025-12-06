// StoreRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace booking.DTO
{
    public class StoreRequestDto
    {
        public int Id { get; set; }

        [Required]
        public int WorkingHourID { get; set; }

        [Required]
        public int WarehouseID { get; set; }

        [Required]
        public int AddressID { get; set; }

        [Required]
        public int StoreID { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // 🔹 Thêm UserID
        [Required]
        public int UserID { get; set; }
    }
}
