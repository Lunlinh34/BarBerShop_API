namespace BarberShop.DTO
{
    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; } = string.Empty;
        public string OrderType { get; set; } = "other"; // tùy chọn
    }
}
