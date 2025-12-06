namespace BarberShop.DTO
{
    public class StoreDto
    {
        public int storeID { get; set; }
        public string? storeName { get; set; }
        public string? numberphone { get; set; }
        public int workingHourID { get; set; }
        public int? addressID { get; set; }

        // Thêm trường ảnh
        public string? ImageUrl { get; set; }
    }
}
