using BarberShop.DTO;
using BarberShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Linq;

namespace BarberShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _service;

        public VnPayController(IVnPayService service)
        {
            _service = service;
        }

        // POST: api/vnpay/create
        [HttpPost("create")]
        public IActionResult Create([FromBody] CreatePaymentDto dto)
        {
            // Lấy IP client
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            // Tạo URL thanh toán
            var paymentUrl = _service.CreatePaymentUrl(dto.Amount, dto.OrderInfo, ip);

            return Ok(new { paymentUrl });
        }

        // GET: api/vnpay/return
        [HttpGet("return")]
        public IActionResult Return()
        {
            // Chuyển query string sang dictionary
            var queryParams = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());

            // Chuyển sang NameValueCollection để validate chữ ký
            var nvc = new NameValueCollection();
            foreach (var kv in queryParams)
            {
                nvc.Add(kv.Key, kv.Value);
            }

            // Validate chữ ký
            bool valid = _service.ValidateSignature(nvc);
            if (!valid)
            {
                return BadRequest("Invalid payment result");
            }

            // Kiểm tra kết quả thanh toán
            if (queryParams.TryGetValue("vnp_ResponseCode", out var code) && code == "00")
            {
                // Thanh toán thành công
                // TODO: Cập nhật trạng thái đơn hàng trong DB
            }
            else
            {
                // Thanh toán thất bại
                // TODO: Cập nhật trạng thái đơn hàng trong DB
            }

            return Ok(queryParams); // trả về cho FE xem kết quả
        }
    }
}
