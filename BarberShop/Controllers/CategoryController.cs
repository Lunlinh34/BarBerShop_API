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
    public class CategoryController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetCategory()
        {
            var category = _unitOfWork.CategoryRepository.GetAll<CategoryDto>();
            return Ok(category);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetById<CategoryDto>(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryDto categoryModel)
        {
            if (categoryModel == null)
                return BadRequest();

            var categoryEntity = _unitOfWork.Mapper.Map<Category>(categoryModel);
            _unitOfWork.CategoryRepository.Add(categoryEntity);
            _unitOfWork.Commit();

            var categoryDto = _unitOfWork.Mapper.Map<CategoryDto>(categoryEntity);

            return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.cateID }, categoryDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, CategoryDto updatedCategoryModel)
        {
            var existingCategoryEntity = _unitOfWork.CategoryRepository.GetById<CategoryDto>(id);

            if (existingCategoryEntity == null)
                return NotFound();

            _unitOfWork.CategoryRepository.UpdateProperties(id, entity =>
            {
                entity.cateName = existingCategoryEntity.cateName;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryEntity = _unitOfWork.CategoryRepository.GetById<CategoryDto>(id);

            if (categoryEntity == null)
                return NotFound();

            _unitOfWork.CategoryRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }


        [HttpGet("search")]
        public IActionResult SearchCategorys([FromQuery] string categorykey)
        {
            var categoryes = _unitOfWork.CategoryRepository.Search<CategoryDto>(category =>
                            category.cateName != null &&
                            category.cateName.Contains(categorykey));
            return Ok(categoryes);
        }
    }
}
