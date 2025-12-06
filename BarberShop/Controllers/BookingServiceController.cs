//using BarberShop.Entity;
//using BarberShop.DTO;
//using BarberShop.Unit;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;

//namespace BarberShop.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BookingServiceController : ControllerBase
//    {
//        private readonly UnitOfWork _unitOfWork;

//        public BookingServiceController(UnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [HttpGet]
//        public IActionResult GetBookingService()
//        {
//            var bookingService = _unitOfWork.BookingServiceRepository.GetAll<BookingServiceDto>();
//            return Ok(bookingService);
//        }

//        [HttpGet("{id}")]
//        public IActionResult GetBookingService(int id)
//        {
//            var bookingService = _unitOfWork.BookingServiceRepository.GetById<BookingServiceDto>(id);

//            if (bookingService == null)
//                return NotFound();

//            return Ok(bookingService);
//        }

//        [HttpPost, Authorize(Roles = "1, 2")]
//        public IActionResult CreateBookingService(BookingServiceDto bookingServiceModel)
//        {
//            if (bookingServiceModel == null)
//                return BadRequest();

//            var bookingServiceEntity = _unitOfWork.Mapper.Map<BookingService>(bookingServiceModel);
//            _unitOfWork.BookingServiceRepository.Add(bookingServiceEntity);
//            _unitOfWork.Commit();

//            var bookingServiceDto = _unitOfWork.Mapper.Map<BookingServiceDto>(bookingServiceEntity);

//            return CreatedAtAction(nameof(GetBookingService), new { id = bookingServiceDto.bookingServiceID }, bookingServiceDto);
//        }

//        [HttpPut("{id}"), Authorize(Roles = "1, 2")]
//        public IActionResult UpdateBookingService(int id, BookingServiceDto updatedBookingServiceModel)
//        {
//            var existingBookingServiceEntity = _unitOfWork.BookingServiceRepository.GetById<BookingServiceDto>(id);

//            if (existingBookingServiceEntity == null)
//                return NotFound();

//            _unitOfWork.BookingServiceRepository.UpdateProperties(id, entity =>
//            {
//                entity.bookingID = updatedBookingServiceModel.bookingID;
//                entity.serviceID = updatedBookingServiceModel.serviceID;
//            });

//            _unitOfWork.Commit();

//            return Ok(new { message = "Cập nhật thành công" });
//        }


//        [HttpDelete("{id}"), Authorize(Roles = "1, 2")]
//        public IActionResult DeleteBookingService(int id)
//        {
//            var bookingServiceEntity = _unitOfWork.BookingServiceRepository.GetById<BookingServiceDto>(id);

//            if (bookingServiceEntity == null)
//                return NotFound();

//            _unitOfWork.BookingServiceRepository.Delete(id);
//            _unitOfWork.Commit();

//            return Ok(new { message = "Xóa thành công" });
//        }
//    }
//}
