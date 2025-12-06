/*using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ServiceCategoryController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetServiceCategory()
        {
            var serviceCategory = _unitOfWork.ServiceCategoryRepository.GetAll<ServiceCategoryDto>();
            return Ok(serviceCategory);
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceCategory(int id)
        {
            var serviceCategory = _unitOfWork.ServiceCategoryRepository.GetById<ServiceCategoryDto>(id);

            if (serviceCategory == null)
                return NotFound();

            return Ok(serviceCategory);
        }

        [HttpPost]
        public IActionResult CreateServiceCategory(ServiceCategoryDto serviceCategoryModel)
        {
            if (serviceCategoryModel == null)
                return BadRequest();

            var serviceCategoryEntity = _unitOfWork.Mapper.Map<ServiceCategory>(serviceCategoryModel);
            _unitOfWork.ServiceCategoryRepository.Add(serviceCategoryEntity);
            _unitOfWork.Commit();

            var serviceCategoryDto = _unitOfWork.Mapper.Map<ServiceCategoryDto>(serviceCategoryEntity);

            return CreatedAtAction(nameof(GetServiceCategory), new { id = serviceCategoryDto.serCateID }, serviceCategoryDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateServiceCategory(int id, ServiceCategoryDto updatedServiceCategoryModel)
        {
            var existingServiceCategoryEntity = _unitOfWork.ServiceCategoryRepository.GetById<ServiceCategoryDto>(id);

            if (existingServiceCategoryEntity == null)
                return NotFound();

            _unitOfWork.ServiceCategoryRepository.UpdateProperties(id, entity =>
            {
                entity.serCateName = updatedServiceCategoryModel.serCateName;
                entity.description = updatedServiceCategoryModel.description;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteServiceCategory(int id)
        {
            var serviceCategoryEntity = _unitOfWork.ServiceCategoryRepository.GetById<ServiceCategoryDto>(id);

            if (serviceCategoryEntity == null)
                return NotFound();

            _unitOfWork.ServiceCategoryRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }


        [HttpGet("search")]
        public IActionResult SearchServiceCategorys([FromQuery] string serviceCategorykey)
        {
            var serviceCategoryes = _unitOfWork.ServiceCategoryRepository.Search<ServiceCategoryDto>(serviceCategory =>
                            serviceCategory.serCateName != null &&
                            serviceCategory.serCateName.Contains(serviceCategorykey));
            return Ok(serviceCategoryes);
        }
    }
}
*/using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Unit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCategoryController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public ServiceCategoryController(UnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        // ✅ Lấy tất cả
        [HttpGet]
        public IActionResult GetServiceCategory()
        {
            var serviceCategory = _unitOfWork.ServiceCategoryRepository.GetAll<ServiceCategoryDto>();
            return Ok(serviceCategory);
        }

        // ✅ Lấy theo ID
        [HttpGet("{id}")]
        public IActionResult GetServiceCategory(int id)
        {
            var serviceCategory = _unitOfWork.ServiceCategoryRepository.GetById<ServiceCategoryDto>(id);
            if (serviceCategory == null)
                return NotFound();

            return Ok(serviceCategory);
        }

        // ✅ Tạo mới (có upload ảnh)
        [HttpPost]
        public IActionResult CreateServiceCategory([FromForm] ServiceCategoryDto serviceCategoryModel, IFormFile? imageFile)
        {
            if (serviceCategoryModel == null)
                return BadRequest();

            string? imageUrl = null;

            // Nếu có file ảnh thì lưu vào wwwroot/images/service-categories
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images", "service-categories");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                imageUrl = $"/images/service-categories/{fileName}";
            }

            var serviceCategoryEntity = _unitOfWork.Mapper.Map<ServiceCategory>(serviceCategoryModel);
            serviceCategoryEntity.ImageUrl = imageUrl; // Gán link ảnh

            _unitOfWork.ServiceCategoryRepository.Add(serviceCategoryEntity);
            _unitOfWork.Commit();

            var serviceCategoryDto = _unitOfWork.Mapper.Map<ServiceCategoryDto>(serviceCategoryEntity);

            return CreatedAtAction(nameof(GetServiceCategory), new { id = serviceCategoryDto.serCateID }, serviceCategoryDto);
        }
        /*
                // ✅ Cập nhật
                [HttpPut("{id}")]
                public IActionResult UpdateServiceCategory(int id, [FromForm] ServiceCategoryDto updatedServiceCategoryModel, IFormFile? imageFile)
                {
                    var existingServiceCategory = _unitOfWork.ServiceCategoryRepository.GetEntityById(id);

                    if (existingServiceCategory == null)
                        return NotFound();

                    string? imageUrl = existingServiceCategory.ImageUrl;

                    // Nếu có ảnh mới thì thay ảnh cũ
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadDir = Path.Combine(_env.WebRootPath, "images", "service-categories");
                        if (!Directory.Exists(uploadDir))
                            Directory.CreateDirectory(uploadDir);

                        var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                        var filePath = Path.Combine(uploadDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }

                        imageUrl = $"/images/service-categories/{fileName}";
                    }

                    _unitOfWork.ServiceCategoryRepository.UpdateProperties(id, entity =>
                    {
                        entity.serCateName = updatedServiceCategoryModel.serCateName;
                        entity.description = updatedServiceCategoryModel.description;
                        entity.ImageUrl = imageUrl;
                    });

                    _unitOfWork.Commit();

                    return Ok(new { message = "Cập nhật danh mục thành công!" });
                }
        */
        // ✅ Cập nhật loại dịch vụ (có hỗ trợ thay ảnh và xóa ảnh cũ)
        [HttpPut("{id}")]
        public IActionResult UpdateServiceCategory(
    int id,
    [FromForm] ServiceCategoryDto updatedServiceCategoryModel,
    IFormFile? imageFile)
        {
            var existingEntity = _unitOfWork.ServiceCategoryRepository.GetEntityById(id);

            if (existingEntity == null)
                return NotFound(new { message = "Không tìm thấy danh mục cần cập nhật" });

            string? imageUrl = existingEntity.ImageUrl;

            // 1️⃣ Nếu upload file mới, lưu file và xóa file cũ
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images", "service-categories");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
                var filePath = Path.Combine(uploadDir, fileName);

                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(existingEntity.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, existingEntity.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                imageUrl = $"/images/service-categories/{fileName}";
            }
            // 2️⃣ Nếu frontend gửi ImageUrl text (link) → lưu link này
            else if (!string.IsNullOrEmpty(updatedServiceCategoryModel.ImageUrl))
            {
                imageUrl = updatedServiceCategoryModel.ImageUrl;
            }

            // 3️⃣ Cập nhật dữ liệu
            _unitOfWork.ServiceCategoryRepository.UpdateProperties(id, entity =>
            {
                entity.serCateName = updatedServiceCategoryModel.serCateName;
                entity.description = updatedServiceCategoryModel.description;
                entity.ImageUrl = imageUrl;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật loại dịch vụ thành công!", ImageUrl = imageUrl });
        }



        // ✅ Xóa
        [HttpDelete("{id}")]
        public IActionResult DeleteServiceCategory(int id)
        {
            var serviceCategoryEntity = _unitOfWork.ServiceCategoryRepository.GetById<ServiceCategoryDto>(id);
            if (serviceCategoryEntity == null)
                return NotFound();

            _unitOfWork.ServiceCategoryRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa danh mục thành công!" });
        }

        // ✅ Tìm kiếm
        [HttpGet("search")]
        public IActionResult SearchServiceCategorys([FromQuery] string serviceCategorykey)
        {
            var serviceCategoryes = _unitOfWork.ServiceCategoryRepository.Search<ServiceCategoryDto>(serviceCategory =>
                serviceCategory.serCateName != null &&
                serviceCategory.serCateName.Contains(serviceCategorykey));

            return Ok(serviceCategoryes);
        }
    }
}
