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
        [HttpGet("{id}")]
        public IActionResult getChildren(int id)
        {
            var children = Unit.StudentRepo.getStudentByParentId(id);
            return Ok(Mapper.Map<List<ParentChildDTO>>(children));
        }
    }
}
