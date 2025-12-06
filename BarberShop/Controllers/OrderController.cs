using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetOrder()
        {
            var order = _unitOfWork.OrderRepository.GetAll<OrderDto>();
            return Ok(order);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _unitOfWork.OrderRepository.GetById<OrderDto>(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
        /*
        [HttpPost]
        public IActionResult CreateOrder(OrderDto orderModel)
        {
            if (orderModel == null)
                return BadRequest();

            var orderEntity = _unitOfWork.Mapper.Map<Order>(orderModel);
            _unitOfWork.OrderRepository.Add(orderEntity);
            _unitOfWork.Commit();

            var orderDto = _unitOfWork.Mapper.Map<OrderDto>(orderEntity);

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.orderID }, orderDto);
        }
        */
        [HttpPost]
        public IActionResult CreateOrder(OrderDto orderModel)
        {
            if (orderModel == null)
                return BadRequest();

            // Chuyển orderDate và deliveryDate sang giờ Việt Nam (UTC+7)
            // Chuyển orderDate và deliveryDate sang giờ Việt Nam (UTC+7)
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // múi giờ VN
            orderModel.orderDate = TimeZoneInfo.ConvertTimeFromUtc(orderModel.orderDate.ToUniversalTime(), vnTimeZone);
            orderModel.deliveryDate = TimeZoneInfo.ConvertTimeFromUtc(orderModel.deliveryDate.ToUniversalTime(), vnTimeZone);

            var orderEntity = _unitOfWork.Mapper.Map<Order>(orderModel);
            _unitOfWork.OrderRepository.Add(orderEntity);
            _unitOfWork.Commit();

            var orderDto = _unitOfWork.Mapper.Map<OrderDto>(orderEntity);

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.orderID }, orderDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, OrderDto updatedOrderModel)
        {
            var existingOrderEntity = _unitOfWork.OrderRepository.GetById<OrderDto>(id);
            if (existingOrderEntity == null)
                return NotFound();

            // Nếu có customerID => kiểm tra hợp lệ
            if (updatedOrderModel.customerID != 0)
            {
                var customer = _unitOfWork.CustomerRepository.GetById<Customer>(updatedOrderModel.customerID);
                if (customer == null)
                    return BadRequest(new { message = "Customer không tồn tại" });
            }

            _unitOfWork.OrderRepository.UpdateProperties(id, entity =>
            {
                entity.orderDate = updatedOrderModel.orderDate;
                entity.orderStatus = updatedOrderModel.orderStatus;
                entity.payID = updatedOrderModel.payID;
                if (updatedOrderModel.customerID > 0)
                    entity.customerID = updatedOrderModel.customerID;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật đơn hàng thành công" });
        }
        [HttpPatch("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] string status)
        {
            var order = _unitOfWork.OrderRepository.GetById<Order>(id);
            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng" });

            _unitOfWork.OrderRepository.UpdateProperties(id, entity =>
            {
                entity.orderStatus = status;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật trạng thái đơn hàng thành công" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var orderEntity = _unitOfWork.OrderRepository.GetById<OrderDto>(id);

            if (orderEntity == null)
                return NotFound();

            _unitOfWork.OrderRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }


        [HttpGet("search")]
        public IActionResult SearchOrders([FromQuery] string orderkey)
        {
            var orderes = _unitOfWork.OrderRepository.Search<OrderDto>(order =>
                            order.orderDate.ToString().Contains(orderkey));
            return Ok(orderes);
        }
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                if (orderDto == null)
                    return BadRequest(new { Message = "Dữ liệu đơn hàng không hợp lệ" });

                // Ngày hiện tại
                DateTime currentDate = DateTime.Now;

                // Tạo ngày giao hàng ngẫu nhiên 3-5 ngày
                Random random = new Random();
                int daysToAdd = random.Next(3, 5);
                DateTime deliveryDate = currentDate.AddDays(daysToAdd);

                // Tạo entity Order
                var order = new Order
                {
                    orderDate = currentDate,
                    deliveryDate = deliveryDate,
                    totalInvoice = orderDto.totalInvoice,
                    orderStatus = "Chưa xác nhận",
                    customerID = orderDto.customerID,
                    payID = orderDto.payID,
                    addressID = orderDto.addressID  // ← thêm trường addressID
                };

                await _unitOfWork.OrderRepository.Add(order);
                await _unitOfWork.CommitAsync();

                return Ok(new { Message = "Đặt hàng thành công!", DeliveryDate = deliveryDate });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Đặt hàng thất bại.", Error = ex.Message });
            }
        }

        /*
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                DateTime currentDate = DateTime.Now;

                Random random = new Random();
                int daysToAdd = random.Next(3, 5);
                DateTime deliveryDate = currentDate.AddDays(daysToAdd);

                var order = new Order
                {
                    orderDate = DateTime.Now,
                    deliveryDate = deliveryDate,
                    totalInvoice = orderDto.totalInvoice,
                    orderStatus = "Chưa xác nhận",
                    customerID = orderDto.customerID,
                    payID = orderDto.payID,
                };

                await _unitOfWork.OrderRepository.Add(order);
                await _unitOfWork.CommitAsync();

                return Ok(new { Message = "Đặt hàng thành công!", DeliveryDate = deliveryDate });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return BadRequest(new { Message = "Đặt hàng thất bại.", Error = ex.Message });
            }
        }*/

    }
}


