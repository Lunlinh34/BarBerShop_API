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
    public class CountryController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CountryController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetCountry()
        {
            var country = _unitOfWork.CountryRepository.GetAll<CountryDto>();
            return Ok(country);
        }

        [HttpGet("{id}")]
        public IActionResult GetCountry(int id)
        {
            var country = _unitOfWork.CountryRepository.GetById<CountryDto>(id);

            if (country == null)
                return NotFound();

            return Ok(country);
        }

        [HttpPost("bulk")]
        //public IActionResult CreateCountry(CountryDto countryModel)
        public IActionResult CreateCountries(List<CountryDto> countries)

        {
            /* if (countryModel == null)
                 return BadRequest();

             var countryEntity = _unitOfWork.Mapper.Map<Country>(countryModel);
             _unitOfWork.CountryRepository.Add(countryEntity);
             _unitOfWork.Commit();

             var countryDto = _unitOfWork.Mapper.Map<CountryDto>(countryEntity);

             return CreatedAtAction(nameof(GetCountry), new { id = countryDto.countryID }, countryDto);
                */
            if (countries == null || !countries.Any())
                return BadRequest();

            foreach (var countryModel in countries)
            {
                var countryEntity = _unitOfWork.Mapper.Map<Country>(countryModel);
                _unitOfWork.CountryRepository.Add(countryEntity);
            }
            _unitOfWork.Commit();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCountry(int id, CountryDto updatedCountryModel)
        {
            var existingCountryEntity = _unitOfWork.CountryRepository.GetById<CountryDto>(id);

            if (existingCountryEntity == null)
                return NotFound();

            _unitOfWork.CountryRepository.UpdateProperties(id, entity =>
            {
                entity.countryName = updatedCountryModel.countryName;

            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            var countryEntity = _unitOfWork.CountryRepository.GetById<CountryDto>(id);

            if (countryEntity == null)
                return NotFound();

            _unitOfWork.CountryRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }


        [HttpGet("search")]
        public IActionResult SearchCountrys([FromQuery] string countrykey)
        {
            var countryes = _unitOfWork.CountryRepository.Search<CountryDto>(country =>
                            country.countryName != null &&
                            country.countryName.Contains(countrykey) );
            return Ok(countryes);
        }
    }
}
