using BarberShop.Entity;

namespace BarberShop.DTO
{
    public class WarehouseDto
    {
        public int warehouseID { get; set; }
        public string? warehouseName { get; set; }
        public float totalAsset { get; set; }
        public int capacity { get; set; }
        public int addressID { get; set; }
        public int storeID { get; set; }
    }
}
