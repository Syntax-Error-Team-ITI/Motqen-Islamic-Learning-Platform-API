using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotqenIslamicLearningPlatform_API.DTOs.ParentDTOs;
using MotqenIslamicLearningPlatform_API.UnitOfWorks;

namespace MotqenIslamicLearningPlatform_API.Controllers.Studnet
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        UnitOfWork Unit { get; }
        public IMapper Mapper { get; }

        public ParentController(UnitOfWork unit, IMapper mapper)
        {
            Unit = unit;
            Mapper = mapper;
        }
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet("{id}/children")]
        public IActionResult GetChildren(int id)
        {
            var children = Unit.StudentRepo.getStudentByParentId(id);
            return Ok(Mapper.Map<List<ParentChildDTO>>(children));
        }
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet]
        public IActionResult GetAll()
        {
            var parents = Unit.ParentRepo.GetAll();
            return Ok(Mapper.Map<List<ParentListDTO>>(parents));
        }
        [Authorize(Roles = "Admin , Parent")]

        [HttpGet("{id}")]
        public IActionResult GetParent(int id)
        {
            var parent = Unit.ParentRepo.GetById(id);
            return Ok(Mapper.Map<List<ParentChildDTO>>(parent));
        }
    }
}
