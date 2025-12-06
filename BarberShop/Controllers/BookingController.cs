using BarberShop.Entity;
using BarberShop.DTO;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetBooking()
        {
            var booking = _unitOfWork.BookingRepository.GetAll<BookingDto>();
            return Ok(booking);
        }

        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }
        /*
        [HttpPost]
        public IActionResult CreateBooking(BookingDto bookingModel)
        {
            Console.WriteLine($"=== DEBUG === employeID: {bookingModel.employeID}, customerID: {bookingModel.customerID}, storeID: {bookingModel.storeID}");

            if (bookingModel == null)
                return BadRequest();

            var bookingEntity = _unitOfWork.Mapper.Map<Booking>(bookingModel);
            _unitOfWork.BookingRepository.Add(bookingEntity);
            _unitOfWork.Commit();

            var bookingDto = _unitOfWork.Mapper.Map<BookingDto>(bookingEntity);

            return CreatedAtAction(nameof(GetBooking), new { id = bookingDto.bookingID }, bookingDto);
        }
        *//*
        [HttpPost]
        public IActionResult CreateBooking([FromBody] BookingDto bookingModel)
        {
            if (bookingModel == null)
                return BadRequest();

            // Debug in ra giá trị
            Console.WriteLine($"===d DEBUG === employeID: {bookingModel.employeID}, customerID: {bookingModel.customerID}, storeID: {bookingModel.storeID}");

            var bookingEntity = new Booking
            {
                startDate = bookingModel.startDate,
                startTime = bookingModel.startTime,
                note = bookingModel.note,
                customerID = bookingModel.customerID,
                storeID = bookingModel.storeID,
                serID = bookingModel.serID,
                employeID = bookingModel.employeID
            };

            _unitOfWork.BookingRepository.Add(bookingEntity);
            _unitOfWork.Commit();

            return Ok(bookingEntity);
        }*/
        [HttpPost]
        public IActionResult CreateBooking([FromBody] BookingDto bookingModel)
        {
            if (bookingModel == null)
                return BadRequest();

            // Lấy tất cả booking và kiểm tra trùng
            var isConflict = _unitOfWork.BookingRepository
                .GetAll<Booking>()
                .Any(b =>
                    b.startDate == bookingModel.startDate &&
                    b.startTime == bookingModel.startTime &&
                    b.employeID == bookingModel.employeID &&
                    b.storeID == bookingModel.storeID
                );

            if (isConflict)
                return Conflict(new { message = "Lịch hẹn trùng với một booking đã tồn tại." });

            var bookingEntity = new Booking
            {
                startDate = bookingModel.startDate,
                startTime = bookingModel.startTime,
                note = bookingModel.note,
                customerID = bookingModel.customerID,
                storeID = bookingModel.storeID,
                serID = bookingModel.serID,
                employeID = bookingModel.employeID
            };

            _unitOfWork.BookingRepository.Add(bookingEntity);
            _unitOfWork.Commit();

            return Ok(bookingEntity);
        }


        /*
        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, BookingDto updatedBookingModel)
        {
            var existingBookingEntity = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (existingBookingEntity == null)
                return NotFound();

            _unitOfWork.BookingRepository.UpdateProperties(id, entity =>
            {
                entity.startTime = updatedBookingModel.startTime;
                entity.customer.customerID = updatedBookingModel.customerID;
                //entity.payment.payID = updatedBookingModel.payID;

            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }*//*
        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, BookingDto updatedBookingModel)
        {
            var existingBookingEntity = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (existingBookingEntity == null)
                return NotFound();

            _unitOfWork.BookingRepository.UpdateProperties(id, entity =>
            {
                entity.startDate = updatedBookingModel.startDate;
                entity.startTime = updatedBookingModel.startTime;
                entity.note = updatedBookingModel.note;

                // Cập nhật trực tiếp ID thay vì entity.customer
                entity.customerID = updatedBookingModel.customerID;
                entity.storeID = updatedBookingModel.storeID;
                entity.serID = updatedBookingModel.serID;
                entity.employeID = updatedBookingModel.employeID;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }
        */
        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, BookingDto updatedBookingModel)
        {
            var existingBookingEntity = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (existingBookingEntity == null)
                return NotFound();

            // Kiểm tra lịch trùng, bỏ qua chính booking hiện tại
            var isConflict = _unitOfWork.BookingRepository
                .GetAll<Booking>()
                .Any(b =>
                    b.bookingID != id &&
                    b.startDate == updatedBookingModel.startDate &&
                    b.startTime == updatedBookingModel.startTime &&
                    b.employeID == updatedBookingModel.employeID &&
                    b.storeID == updatedBookingModel.storeID
                );

            if (isConflict)
                return Conflict(new { message = "Lịch hẹn trùng với một booking đã tồn tại." });

            _unitOfWork.BookingRepository.UpdateProperties(id, entity =>
            {
                entity.startDate = updatedBookingModel.startDate;
                entity.startTime = updatedBookingModel.startTime;
                entity.note = updatedBookingModel.note;
                entity.customerID = updatedBookingModel.customerID;
                entity.storeID = updatedBookingModel.storeID;
                entity.serID = updatedBookingModel.serID;
                entity.employeID = updatedBookingModel.employeID;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            var bookingEntity = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (bookingEntity == null)
                return NotFound();

            _unitOfWork.BookingRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
