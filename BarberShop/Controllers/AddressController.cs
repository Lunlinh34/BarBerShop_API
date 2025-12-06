using BarberShop.Entity;
using BarberShop.DTO;
using BarberShop.Unit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public AddressController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAddress()
        {
            var address = _unitOfWork.AddressRepository.GetAll<AddressDto>();
            return Ok(address);
        }

        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {
            var address = _unitOfWork.AddressRepository.GetById<AddressDto>(id);

            if (address == null)
                return NotFound();

            return Ok(address);
        }
        /*
        [HttpPost]
        public IActionResult CreateAddress([FromBody] AddressDto addressModel)
        {
            if (addressModel == null)
                return BadRequest();

            var addressEntity = _unitOfWork.Mapper.Map<Address>(addressModel);
            _unitOfWork.AddressRepository.Add(addressEntity);
            _unitOfWork.Commit();

            var addressDto = _unitOfWork.Mapper.Map<AddressDto>(addressEntity);

            return CreatedAtAction(nameof(GetAddress), new { id = addressDto.addressID }, addressDto);
        }
        */
        [HttpPost]
        public IActionResult CreateAddress([FromBody] AddressDto addressModel)
        {
            if (addressModel == null)
                return BadRequest();

            var cityExists = _unitOfWork.CityRepository.GetById<City>(addressModel.cityID);
            if (cityExists == null)
                return BadRequest("CityID không hợp lệ");

            var addressEntity = _unitOfWork.Mapper.Map<Address>(addressModel);
            _unitOfWork.AddressRepository.Add(addressEntity);
            _unitOfWork.Commit();

            var addressDto = _unitOfWork.Mapper.Map<AddressDto>(addressEntity);

            return CreatedAtAction(nameof(GetAddress), new { id = addressDto.addressID }, addressDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, AddressDto updatedAddressModel)
        {
            var existingAddressEntity = _unitOfWork.AddressRepository.GetById<AddressDto>(id);

            if (existingAddressEntity == null)
                return NotFound();

            _unitOfWork.AddressRepository.UpdateProperties(id, entity =>
            {
                entity.currentAddress = updatedAddressModel.currentAddress;
                entity.subDistrict = updatedAddressModel.subDistrict;
                entity.district = updatedAddressModel.district;
                entity.cityID = updatedAddressModel.cityID;

            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var addressEntity = _unitOfWork.AddressRepository.GetById<AddressDto>(id);

            if (addressEntity == null)
                return NotFound();

            _unitOfWork.AddressRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }


        [HttpGet("search")]
        public IActionResult SearchAddresss([FromQuery] string addresskey)
        {
            var addresses = _unitOfWork.AddressRepository.Search<AddressDto>(address =>
                            address.subDistrict != null && 
                            address.subDistrict.Contains(addresskey) || 
                            address.district != null && address.district.Contains(addresskey) || 
                            address.currentAddress != null && address.currentAddress.Contains(addresskey) );
            return Ok(addresses);
        }
    }
}
