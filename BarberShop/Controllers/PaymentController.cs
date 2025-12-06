using BarberShop.Entity;
using BarberShop.DTO;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public PaymentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetPayment()
        {
            var payment = _unitOfWork.PaymentRepository.GetAll<PaymentDto>();
            return Ok(payment);
        }

        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _unitOfWork.PaymentRepository.GetById<PaymentDto>(id);

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpPost]
        public IActionResult CreatePayment(PaymentDto paymentModel)
        {
            if (paymentModel == null)
                return BadRequest();

            var paymentEntity = _unitOfWork.Mapper.Map<Payment>(paymentModel);
            _unitOfWork.PaymentRepository.Add(paymentEntity);
            _unitOfWork.Commit();

            var paymentDto = _unitOfWork.Mapper.Map<PaymentDto>(paymentEntity);

            return CreatedAtAction(nameof(GetPayment), new { id = paymentDto.payID }, paymentDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, PaymentDto updatedPaymentModel)
        {
            var existingPaymentEntity = _unitOfWork.PaymentRepository.GetById<PaymentDto>(id);

            if (existingPaymentEntity == null)
                return NotFound();

            _unitOfWork.PaymentRepository.UpdateProperties(id, entity =>
            {
                entity.payMethod = updatedPaymentModel.payMethod;
                entity.payStatus = updatedPaymentModel.payStatus;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var paymentEntity = _unitOfWork.PaymentRepository.GetById<PaymentDto>(id);

            if (paymentEntity == null)
                return NotFound();

            _unitOfWork.PaymentRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
