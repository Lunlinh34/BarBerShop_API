using AutoMapper;
using BarberShop.Entity;
using BarberShop.Helpers;
using BarberShop.DTO;
using BarberShop.Unit;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public RoleController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _unitOfWork.RoleRepository.GetAll<RoleDto>();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult GetRole(int id)
        {
            var role = _unitOfWork.RoleRepository.GetById<RoleDto>(id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public IActionResult CreateRole(RoleDto roleModel)
        {
            if (roleModel == null)
                return BadRequest();

            var roleEntity = _unitOfWork.Mapper.Map<Role>(roleModel);
            _unitOfWork.RoleRepository.Add(roleEntity);
            _unitOfWork.Commit();

            var roleDto = _unitOfWork.Mapper.Map<RoleDto>(roleEntity);

            return CreatedAtAction(nameof(GetRole), new { id = roleDto.roleID }, roleDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRole(int id, RoleDto updatedRoleModel)
        {
            var existingRoleEntity = _unitOfWork.RoleRepository.GetById<RoleDto>(id);

            if (existingRoleEntity == null)
                return NotFound();

            _unitOfWork.RoleRepository.UpdateProperties(id, entity =>
            {
                entity.roleName = updatedRoleModel.roleName;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            var roleEntity = _unitOfWork.RoleRepository.GetById<RoleDto>(id);

            if (roleEntity == null)
                return NotFound();

            _unitOfWork.RoleRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }


        [HttpGet("search")]
        public IActionResult SearchRoles([FromQuery] string roleName)
        {
            var roles = _unitOfWork.RoleRepository.Search<RoleDto>(role => role.roleName!.Contains(roleName));

            if (roles != null)
            {
                return Ok(roles);
            }
            else
            {
                return Ok(new { message = "Không tìm thấy chuỗi cần tìm" });
            }
        }
    }

}

