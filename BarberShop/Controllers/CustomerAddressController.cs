using BarberShop.Entity;
using BarberShop.DTO;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerAddressController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetCustomerAddress()
        {
            var customerAddress = _unitOfWork.CustomerAddressRepository.GetAll<CustomerAddressDto>();
            return Ok(customerAddress);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerAddress(int id)
        {
            var customerAddress = _unitOfWork.CustomerAddressRepository.GetById<CustomerAddressDto>(id);

            if (customerAddress == null)
                return NotFound();

            return Ok(customerAddress);
        }

        [HttpPost]
        public IActionResult CreateCustomerAddress(CustomerAddressDto customerAddressModel)
        {
            /*
            if (customerAddressModel == null)
                return BadRequest();

            var customerAddressEntity = _unitOfWork.Mapper.Map<CustomerAddress>(customerAddressModel);
            _unitOfWork.CustomerAddressRepository.Add(customerAddressEntity);
            _unitOfWork.Commit();

            var customerAddressDto = _unitOfWork.Mapper.Map<CustomerAddressDto>(customerAddressEntity);

            return CreatedAtAction(nameof(GetCustomerAddress), new { id = customerAddressDto.cusAddressId }, customerAddressDto);
            */
            if (customerAddressModel == null)
                return BadRequest(new { message = "Dữ liệu gửi lên không hợp lệ!" });

            // 🧩 Kiểm tra ID hợp lệ
            if (customerAddressModel.addressID <= 0)
                return BadRequest(new { message = "Địa chỉ không hợp lệ!" });

            if (customerAddressModel.customerID <= 0)
                return BadRequest(new { message = "Khách hàng không hợp lệ!" });

            // 🧩 Kiểm tra Address có tồn tại không
            var address = _unitOfWork.AddressRepository.GetById<Address>(customerAddressModel.addressID);
            if (address == null)
                return BadRequest(new { message = "Địa chỉ đã chọn không tồn tại trong hệ thống!" });

            // 🧩 Kiểm tra Customer có tồn tại không
            var customer = _unitOfWork.CustomerRepository.GetById<Customer>(customerAddressModel.customerID);
            if (customer == null)
                return BadRequest(new { message = "Khách hàng không tồn tại!" });

            // 🧩 Map và lưu vào DB
            var customerAddressEntity = _unitOfWork.Mapper.Map<CustomerAddress>(customerAddressModel);
            _unitOfWork.CustomerAddressRepository.Add(customerAddressEntity);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lưu dữ liệu!", error = ex.Message });
            }

            var customerAddressDto = _unitOfWork.Mapper.Map<CustomerAddressDto>(customerAddressEntity);
            return CreatedAtAction(nameof(GetCustomerAddress),
                new { id = customerAddressDto.cusAddressId },
                customerAddressDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomerAddress(int id, CustomerAddressDto updatedCustomerAddressModel)
        {
            var existingCustomerAddressEntity = _unitOfWork.CustomerAddressRepository.GetById<CustomerAddressDto>(id);

            if (existingCustomerAddressEntity == null)
                return NotFound();

            _unitOfWork.CustomerAddressRepository.UpdateProperties(id, entity =>
            {
                entity.Customer.customerID = updatedCustomerAddressModel.customerID;
                entity.Address.addressID = updatedCustomerAddressModel.addressID;

            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerAddress(int id)
        {
            var customerAddressEntity = _unitOfWork.CustomerAddressRepository.GetById<CustomerAddressDto>(id);

            if (customerAddressEntity == null)
                return NotFound();

            _unitOfWork.CustomerAddressRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
