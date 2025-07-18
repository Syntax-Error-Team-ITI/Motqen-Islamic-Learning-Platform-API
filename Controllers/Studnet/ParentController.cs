using AutoMapper;
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
        [HttpGet("{id}/children")]
        public IActionResult GetChildren(int id)
        {
            var children = Unit.StudentRepo.getStudentByParentId(id);
            return Ok(Mapper.Map<List<ParentChildDTO>>(children));
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var parents = Unit.ParentRepo.GetAll();
            return Ok(Mapper.Map<List<ParentListDTO>>(parents));
        }
        [HttpGet("{id}")]
        public IActionResult GetParent(int id)
        {
            var parent = Unit.ParentRepo.GetById(id);
            return Ok(Mapper.Map<List<ParentChildDTO>>(parent));
        }
    }
}
